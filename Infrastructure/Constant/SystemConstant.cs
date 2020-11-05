using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Constant
{
    public class SystemConstant
    {
        private SystemConstant() { }

        public const string Name = "Hal";
        public const string WebDllName = "Hal.AspDotNetCore3.xml";
        public const string AllDll = "*.dll";
        public const string AuthSchema = "MyScheme";
        public const string AuthSchemaDescription = "MySchemeDemo";



        public const string Hangfire = "/hangfire";
        public const string Health = "/health";
        public const string HttpReports = "httpreports";
        public const string SignalRPage = "signal";
        public const string SignalRHub = "/myHub";
        public const string MiniProfiler = "/profiler";
        public const string CorsPolicy = "cors";
        public const string SwaggerIndex = "Hal.AspDotNetCore3.index.html";
    }
}
