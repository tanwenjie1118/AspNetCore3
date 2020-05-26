using Core.SqlSugar.Base;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspDotNetCore3.Extensions
{
    /// <summary>
    /// CorsPolicyExtension
    /// </summary>
    public static class CorsPolicyExtension
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(option => option.AddPolicy("cors",
                policy =>
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()));

            return serviceCollection;
        }
    }
}
