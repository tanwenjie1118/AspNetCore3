#region <<copyright>>
/* ==============================================================================
// <copyright file="MongoDbContextOptions.cs" company="HomePartenter.com">
// Copyright (c) HomePartenter.com. All rights reserved.
// </copyright>
* ==============================================================================*/
#endregion

using System;

namespace Core.MongoDB
{
    public class MongoDbContextOptions
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public bool UseSSL { get; set; }

        public TimeSpan ConnectTimeout { get; set; }

        public void UseMongoDb(string connectionString, bool useSSL = false, string database = null, TimeSpan connectTimeout = default(TimeSpan))
        {
            this.ConnectionString = connectionString;
            this.Database = database;
            this.ConnectTimeout = connectTimeout;
            this.UseSSL = useSSL;
        }

        public void AddFactory<TDbcontext>()
        {
            MongoDbContextFactory.Providers.TryAdd(typeof(TDbcontext), this);
        }
    }
}
