using Hal.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hal.Core.Extensions
{
    public static class ConfigBuilderExtension
    {
        public static IServiceCollection AddConfig<TConfig>(this IServiceCollection serviceCollection, IConfiguration config)
        where TConfig : class, new()
        {
            var conf = config.BindConfig<TConfig>();
            serviceCollection.AddSingleton(conf);
            return serviceCollection;
        }
    }
}
