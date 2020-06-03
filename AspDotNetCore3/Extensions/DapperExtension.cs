using Core.Dapper.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCore3.Extensions
{
    /// <summary>
    /// DapperExtension
    /// </summary>
    public static class DapperExtension
    {
        public static IServiceCollection AddDapper(this IServiceCollection serviceCollection, Action<DapperOption> option)
        {
            serviceCollection.AddOptions();

            serviceCollection.Configure(option);

            serviceCollection.AddScoped<DapperContext>();

            return serviceCollection;
        }
    }
}
