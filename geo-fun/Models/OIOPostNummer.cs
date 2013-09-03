using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geo_fun.Models
{
    public class OIOPostNummer
    {
        //public string id { get; set; }
        public string href { get; set; }
        public string nr { get; set; }
        public int fra { get; set; }
        public int til { get; set; }
        public string navn { get; set; }
        public long areal { get; set; }
        public string grænse { get; set; }
        public string naboer { get; set; }

        public Polygon polygon { get; set; }

        public OIOPostNummer()
        {
            polygon = new Polygon();
        }

        public class Polygon
        {
            //public string type { get; set; }
            //public string coordinates { get; set; }

            public string type { get; set; }
            public List<List<List<List<double>>>> coordinates { get; set; }
        }

    }
}
