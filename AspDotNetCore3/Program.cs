using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;

namespace AspDotNetCore3
{
    public class Program
    {
        public static bool DisableProfilingResults { get; internal set; }
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging((hostingContext, builder) =>
            {
                builder.ClearProviders();//remove default providers
                builder.AddConsole();
                builder.AddDebug();
            })
            .ConfigureWebHostDefaults(
            webBuilder =>
            {
                webBuilder
                .UseUrls("http://+:80", "https://+:443")
                .UseStartup<Startup>().UseNLog();
            }).UseNLog();
    }
}
