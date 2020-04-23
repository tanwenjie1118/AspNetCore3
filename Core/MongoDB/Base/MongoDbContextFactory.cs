using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Core.MongoDB
{
    public static class MongoDbContextFactory
    {
        public static readonly ConcurrentDictionary<Type, MongoDbContextOptions> Providers 
            = new ConcurrentDictionary<Type, MongoDbContextOptions>();
    }
}
