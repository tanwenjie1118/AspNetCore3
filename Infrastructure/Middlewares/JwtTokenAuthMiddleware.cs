using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Hal.Infrastructure.Helpers;

namespace Hal.Infrastructure.Middlewares
{
    public class JwtTokenAuthMiddleware
    {
        // this delegate is next middleware
        private readonly RequestDelegate _next;
        public JwtTokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // is 'Authorization' contains in header
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }

            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                if (tokenHeader.Length >= 128)
                {
                    TokenModelJwt tm = JwtHelper.DeEncrypt(tokenHeader);

                    // add this claim to principal
                    var claimList = new List<Claim>();
                    var claim = new Claim(ClaimTypes.Role, tm.Role);
                    claimList.Add(claim);
                    var identity = new ClaimsIdentity(claimList);
                    var principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} middleware wrong:{e.Message}");
            }

            return _next(httpContext);
        }
    }
}
