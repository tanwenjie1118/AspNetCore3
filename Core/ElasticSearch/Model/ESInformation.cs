using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Core.ElasticSearch.Model
{
    public class ESInformation
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public int BusNo { set; get; }
        public int CompanyId { set; get; }
        public bool Online { set; get; }
        public int Business_type { set; get; }
        public string Level { set; get; }
        public DateTime CreateDate { set; get; }
    }
}
