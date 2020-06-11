using Hal.Core.SqlSugar.Base;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hal.Core.Extensions
{
    /// <summary>
    /// SqlSugar
    /// </summary>
    public static class SqlSugarExtension
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection serviceCollection,Action<SugarOption> option)
        {
            serviceCollection.AddOptions();

            serviceCollection.Configure(option);

            serviceCollection.AddScoped<SugarContext>();

            return serviceCollection;
        }
    }
}
