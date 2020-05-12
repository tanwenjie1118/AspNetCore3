using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;

namespace Infrastructure
{
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        public Appsettings(string contentPath)
        {
            // get configs by ASPNETCORE_ENVIRONMENT
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Path = !string.IsNullOrWhiteSpace(env) ? $"appsettings.{env}.json" : "appsettings.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
               .Build();
        }

        /// <summary>
        /// Get value your want
        /// </summary>
        /// <param name="sections">nodes</param>
        /// <returns></returns>
        public static string Get(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
                else
                {
                    throw new ArgumentNullException("sections");
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("No such value");
            }
        }
    }
}
