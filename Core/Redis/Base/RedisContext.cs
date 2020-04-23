using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;

namespace Core.Redis
{
    public class RedisContext : RedisCient
    {
        public RedisContext(IOptions<RedisContextOptions> options) : base(options)
        {
        }
    }
}
