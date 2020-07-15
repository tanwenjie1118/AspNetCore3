using Hal.AspDotNetCore3;
using Hal.Core.Redis;
using System;
using System.IO;
using Xunit;

namespace Tests.Hal.Services
{
    public class RedisTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IRedisRepository redis;
        public RedisTest()
        {
            redis = testWebServer.Resolve<IRedisRepository>();
        }

        [Fact]
        public void Test1()
        {
            // var val = redis.SetKeyValue("date1","202020202020202020",100000);
            var val = redis.GetKeyValue("date1");

            Assert.NotNull(val);
        }
    }
}
