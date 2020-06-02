using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCore3.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseJwtTokenAuthentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuthMiddleware>();
        }
    }
}
