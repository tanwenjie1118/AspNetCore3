using Core.SqlSugar.Base;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCore3.Extensions
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
