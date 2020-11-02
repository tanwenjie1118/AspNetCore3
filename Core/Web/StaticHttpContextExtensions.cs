using Autofac;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Hal.Core.Web
{
    public static class StaticHttpContextExtensions
    {
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = AutofacContainer.Container.Resolve<IHttpContextAccessor>();
            Infrastructure.Singleton.HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
