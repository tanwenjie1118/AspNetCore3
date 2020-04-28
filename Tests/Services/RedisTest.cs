using AspDotNetCore3;
using Core.Redis;
using System;
using Xunit;

namespace Tests.Services
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