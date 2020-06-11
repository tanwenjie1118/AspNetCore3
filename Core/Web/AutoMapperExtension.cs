using Autofac;
using AutoMapper;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;

namespace Hal.Core.Extensions
{
    public static class AutoMapperExtension
    {
        public static void AddStaticAutoMapper(this IApplicationBuilder app)
        {
            IMapperInstance.ItSelf = AutofacContainer.Container.Resolve<IMapper>();
        }
    }
}
