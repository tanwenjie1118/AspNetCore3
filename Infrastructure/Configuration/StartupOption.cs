using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Configuration
{
    public class StartupOption
    {
        public string ApiName { get; set; }
        public ConsulServiceOption ConsulService { get; set; }
        public JwtBearerOption Jwt { get; set; }
    }
}
