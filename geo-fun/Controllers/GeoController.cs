using geo_fun.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace geo_fun.Controllers
{
    public class GeoController : BaseElasticController
    {
        public object Get(double lat, double lon)
        {

            //var all = elastic.Search<OIOPostNummer>(x => x.MatchAll());
            var geo = ElasticClient.Search<OIOPostNummer>(b => b.QueryRaw(@"
{
    ""geo_shape"": {
        ""polygon"": {
            ""shape"": {
                ""type"": ""point"",
                ""coordinates"": [" + lon.ToString(CultureInfo.InvariantCulture) + ", " + lat.ToString(CultureInfo.InvariantCulture) + @"]
            }
        }
    }
}
").Index("testgeo"));
            return new { res = geo.Hits.Hits.Count == 0 ? null : geo.Hits.Hits.First().Source, stats = new { took = geo.ElapsedMilliseconds } };
        }
    }
}
