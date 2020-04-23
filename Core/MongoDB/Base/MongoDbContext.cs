#region <<copyright>>
/* ==============================================================================
// <copyright file="MongoDbContext.cs" company="HomePartenter.com">
// Copyright (c) HomePartenter.com. All rights reserved.
// </copyright>
* ==============================================================================*/
#endregion
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Core.MongoDB
{
    public class MongoDbContext
    {
        private readonly string DatabaseDefault = "HalTest";

        /// <summary>
        /// Mongo上下文.
        /// </summary>
        public IMongoDatabase Database { get; set; }

        public MongoDbContextOptions Options { get; }

        public MongoDbContext(IOptions<MongoDbContextOptions> options)
        {
            if (MongoDbContextFactory.Providers.TryGetValue(this.GetType(), out var option))
            {
                this.Options = option;
                var client = this.InitalClient(this.Options.ConnectionString, this.Options.UseSSL);
                this.Database = client.GetDatabase(this.Options.Database ?? this.DatabaseDefault);
            }
        }

        private MongoClient InitalClient(string mongoUrl, bool ussSSL)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoUrl));
            settings.SslSettings = new SslSettings
            {
                CheckCertificateRevocation = false
            };
            settings.UseTls = ussSSL;
            settings.AllowInsecureTls = false;
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
            settings.ReadPreference = ReadPreference.PrimaryPreferred;
            settings.ConnectTimeout = TimeSpan.FromSeconds(15);
            settings.ConnectionMode = ConnectionMode.Automatic;
            settings.RetryWrites = true;
            return new MongoClient(settings);
        }
    }
}
