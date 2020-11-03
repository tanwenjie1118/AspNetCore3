using Autofac;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hal.Core.Web
{
    public static class StaticHttpContextExtensions
    {
        public static IServiceCollection AddSingletonHttpContextAccessor(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return serviceCollection;
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = AutofacContainer.Container.Resolve<IHttpContextAccessor>();
            Infrastructure.Singleton.HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
