﻿using Microsoft.AspNetCore.Http;

namespace Hal.Infrastructure.Singleton
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
