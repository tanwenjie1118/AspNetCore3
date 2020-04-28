using Autofac;
using AutoMapper;
using Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;

namespace AspDotNetCore3.Extensions
{
    public static class AutoMapperExtension
    {
        public static void AddStaticAutoMapper(this IApplicationBuilder app)
        {
            IMapperInstance.ItSelf = AutofacContainer.Container.Resolve<IMapper>();
        }
    }
}
