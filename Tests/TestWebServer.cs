using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using System;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;
using Microsoft.Extensions.Hosting;
using Autofac;

namespace Tests
{
    /// <summary>
    /// Run web service by startup class
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class TestWebServer<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer testServer;

        public TestWebServer()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var builder = Host.CreateDefaultBuilder()
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<TStartup>()
               .ConfigureLogging(log => { log.ClearProviders(); })
               .UseNLog();
           });

            testServer = new TestServer(builder.Build().Services);
        }

        public T Resolve<T>()
        {
            return testServer.Services.GetAutofacRoot().Resolve<T>();
        }

        public void Dispose()
        {
            testServer.Dispose();
        }
    }
}
