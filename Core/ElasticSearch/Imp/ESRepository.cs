using System;
using Hal.Core.ElasticSearch.Model;
using Hal.Infrastructure.Configuration;
using Nest;

namespace Hal.Core.ElasticSearch.Imp
{
    public class ESRepository: IESRepository
    {
        private readonly ElasticClient client;
        public ESRepository(DatabaseOption option)
        {
            var node = new Uri(option.ElasticSearch.Host);
            var settings = new ConnectionSettings(node).DefaultIndex(option.ElasticSearch.Index);
            client = new ElasticClient(settings);
        }

        public bool PostData(ESInformation eSInformation)
        {
            var index = client.IndexDocument(eSInformation);
            return index.IsValid;
        }
    }
}
