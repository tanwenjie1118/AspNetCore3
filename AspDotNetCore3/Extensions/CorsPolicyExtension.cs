using Hal.Core.SqlSugar.Base;
using Hal.Infrastructure.Constant;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hal.AspDotNetCore3.Extensions
{
    /// <summary>
    /// CorsPolicyExtension
    /// </summary>
    public static class CorsPolicyExtension
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(option => option.AddPolicy(
                SystemConstant.CorsPolicy,
                policy =>
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost")));

            return serviceCollection;
        }
    }
}
