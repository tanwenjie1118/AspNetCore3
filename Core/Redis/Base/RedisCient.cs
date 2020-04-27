using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace Core.Redis
{
    public class RedisCient
    {
        private readonly int DatabaseDefault = 0;

        /// <summary>
        /// Redis上下文.
        /// </summary>
        public IDatabase Redis { get; set; }

        public RedisContextOptions Options { get; }

        public RedisCient(IOptions<RedisContextOptions> options)
        {
            this.Options = options.Value;
            var configuration = ConfigurationOptions.Parse(this.Options.ConnectionString);
            configuration.DefaultDatabase = DatabaseDefault;
            if (this.Options.Database > 0)
            {
                configuration.DefaultDatabase = this.Options.Database;
            }
            if (this.Options.ConnectTimeout != default)
            {
                configuration.ConnectTimeout = this.Options.ConnectTimeout.Milliseconds;
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