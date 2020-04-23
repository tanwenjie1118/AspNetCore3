using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace Core.Redis
{
    public class RedisCient
    {
        private readonly int DatabaseDefault = 0;

        private readonly RedisContextOptions options;

        /// <summary>
        /// Redis上下文.
        /// </summary>
        public IDatabase Redis { get; set; }

        public RedisContextOptions Options => options;

        public RedisCient(IOptions<RedisContextOptions> options)
        {
            this.options = options.Value;
            var configuration = ConfigurationOptions.Parse(this.options.ConnectionString);
            configuration.DefaultDatabase = DatabaseDefault;
            if (this.options.Database > 0)
            {
                configuration.DefaultDatabase = this.options.Database;
            }
            if (this.options.ConnectTimeout != default)
            {
                configuration.ConnectTimeout = this.options.ConnectTimeout.Milliseconds;
            }
            try
            {
                var redisConnect = ConnectionMultiplexer.Connect(configuration);
                Redis = redisConnect.GetDatabase();
            }
            catch(Exception ex)
            {
                // do nothing
            }
        }
    }
}