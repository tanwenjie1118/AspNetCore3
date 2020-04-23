using Microsoft.AspNetCore.Http;

namespace Infrastructure.Domain
{
    public static class StaticHttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static HttpContext Current => _accessor.HttpContext;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
