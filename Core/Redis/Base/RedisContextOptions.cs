using Microsoft.Extensions.Options;
using System;

namespace Hal.Core.Redis
{
    public class RedisContextOptions : IOptions<RedisContextOptions>
    {
        public string ConnectionString { get; set; }

        public int Database { get; set; }

        public TimeSpan ConnectTimeout { get; set; }

        public RedisContextOptions Value => this;

        public void UseCache(string connectionString, int database = 0, TimeSpan connectTimeout = default)
        {
            ConnectionString = connectionString;
            Database = database;
            ConnectTimeout = connectTimeout;
        }
    }
}
