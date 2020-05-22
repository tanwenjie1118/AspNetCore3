using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class StartupOption
    {
        public string ApiName { get; set; }
        public ConsulServiceOption ConsulService { get; set; }
    }
}
