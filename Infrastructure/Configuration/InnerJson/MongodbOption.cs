using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration.InnerJson
{
   public class MongodbOption
    {
        public string Conn { get; set; }
        public string Ssl { get; set; }
        public string DbNo { get; set; }
    }
}
