using Hal.Core.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hal.Core.Extensions
{
    public static class MongoServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TDbcontext>(
           this IServiceCollection serviceCollection,
           (string connectionstring, bool UseSSL, string DataBase, TimeSpan Timeout) tuple)
           where TDbcontext : MongoDbContext
        {
            //serviceCollection.AddOptions();

            //serviceCollection.Configure(optionsAction);

            var options = new MongoDbContextOptions();

            options.UseMongoDb(tuple.connectionstring, tuple.UseSSL, tuple.DataBase, tuple.Timeout);

            options.AddFactory<TDbcontext>();

            serviceCollection.AddScoped<TDbcontext>();

            return serviceCollection;
        }
    }
}
