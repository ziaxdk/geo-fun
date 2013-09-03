using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace geo_fun
{
    public class BaseElasticController : ApiController
    {
        private ElasticClient _elasticClient;
        protected ElasticClient ElasticClient { get { return _elasticClient; } }

        public BaseElasticController()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200"));
            _elasticClient = new ElasticClient(setting);
        }
    }
}