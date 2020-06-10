using Hal.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hal.Infrastructure.Domain
{
    public class MyHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies["myCookie"];
            if (string.IsNullOrEmpty(cookie))
            {
                return Task.FromResult(AuthenticateResult.NoResult()) ;
            }

            return Task.FromResult(AuthenticateResult.Success(TicketSerializer.Default.Deserialize(ObjectHelper.ToBytes(cookie))));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);

            Context.Response.Cookies.Append("myCookie",
                ObjectHelper.ToString(TicketSerializer.Default.Serialize(ticket)),
                new CookieOptions()
                {
                    Expires = DateTime.Now.AddHours(1)
                });

            AuthenticateResult.Success(ticket);
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete("myCookie");
            return Task.CompletedTask;
        }
    }
}
