using geo_fun.Models;
using Nest;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace geo_fun.Controllers
{
    public class FetchController : BaseElasticController
    {
        const string filename = @"D:\Ziax\Desktop\all_post.json";

        public object Get()
        {
            var str = File.ReadAllText(filename);
            List<OIOPostNummer> res = JsonConvert.DeserializeObject<List<OIOPostNummer>>(str);

            ElasticClient.DeleteIndex("testgeo");
            //var reso = elastic.CreateIndex("testgeo", x => x.AddMapping<OIOPostNummer>(m => m.Properties(p => p.GeoShape(g => g.Name(n => n.polygon).Tree(GeoTree.quadtree).TreeLevels(26)))));
            var reso = ElasticClient.CreateIndexRaw("testgeo", @"
{
    ""mappings"" : {
    ""oiopostnummers"" : {
        ""properties"": {
            ""navn"": {
            ""type"": ""string""
            },
            ""polygon"": {
                ""type"": ""geo_shape"",
                ""tree"": ""quadtree"",
                ""precision"": ""1m""
            }
        }
    }
    } 
}
");

            Stopwatch sw = Stopwatch.StartNew();
            foreach (var post in res)
            {
                //ElasticClient.Bulk(b => b.Index<OIOPostNummer>(i => i.Index("testgeo").Object(post)));
                ElasticClient.Index(post, "testgeo");
            }
            sw.Stop();

            return new { NumberOfPosts = res.Count, Elapsed = sw.Elapsed };
        }

        public string Put()
        {

            //            return reso.ConnectionStatus.Request;

            var client = new RestClient("http://geo.oiorest.dk/");
            var req = new RestRequest("postnumre.json", Method.GET);
            var res = client.Execute<List<OIOPostNummer>>(req).Data;

            foreach (var restPostNummer in res)
            {
                var req2 = new RestRequest("postnumre/{nummer}/grænse.json", Method.GET);
                req2.AddUrlSegment("nummer", restPostNummer.nr);
                var res2 = client.Execute<OIOPostNummer.Polygon>(req2).Data;
                restPostNummer.polygon = res2;
            }

            File.WriteAllText(filename, JsonConvert.SerializeObject(res));
            return "OK";
        }
    }
}
