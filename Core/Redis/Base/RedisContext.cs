using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;

namespace Hal.Core.Redis
{
    public class RedisContext : RedisCient
    {
        public RedisContext(IOptions<RedisContextOptions> options) : base(options)
        {
        }
    }
}
