using Hal.Core.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hal.Core.Extensions
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<RedisContextOptions> optionsAction)
            where TContext : RedisContext
        {
            serviceCollection.AddOptions();

            serviceCollection.Configure(optionsAction);

            serviceCollection.AddScoped<TContext>();

            return serviceCollection;
        }
    }
}
