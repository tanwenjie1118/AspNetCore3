using Autofac;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Hal.Services.Application;
using Hal.Tasks;

namespace Hal.AspDotNetCore3.Extensions
{
    /// <summary>
    /// Job service
    /// </summary>
    public static class JobServiceExtension
    {
        public static void AddJobService(this IServiceCollection services)
        {
            services.AddSingleton<IJobServices, JobServices>();
            services.AddHostedService<SimpleHostedTask>();
        }

        public static IApplicationBuilder UseJob(this IApplicationBuilder app)
        {
            var job = AutofacContainer.Container.Resolve<IJobServices>();
            job.Execute();
            return app;
        }
    }
}
