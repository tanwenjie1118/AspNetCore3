using System;
using System.Collections.Generic;
using System.Threading;
using Hal.Core.ElasticSearch.Model;
using Hal.Infrastructure.Configuration;
using Nest;

namespace Hal.Core.ElasticSearch.Imp
{
    public class ESRepository : IESRepository
    {
        private readonly ElasticClient client;
        private readonly DatabaseOption option;
        public ESRepository(DatabaseOption option)
        {
            this.option = option;
            var node = new Uri(option.ElasticSearch.Host);
            var settings = new ConnectionSettings(node).DefaultIndex(option.ElasticSearch.Index);
            client = new ElasticClient(settings);
        }

        public bool CreateIndex<T>(string indexName) where T : class
        {
            var existsResponse = client.Indices.Exists(indexName);
            // create if not exist 
            if (existsResponse.Exists)
            {
                return true;
            }

            // basic setting
            IIndexState indexState = new IndexState
            {
                Settings = new IndexSettings
                {
                    NumberOfReplicas = 1, // replicas
                    NumberOfShards = 1 // shards
                    // nodes should bigger or equal than shards + replicas
                }
            };

            CreateIndexResponse response = client.Indices.Create(indexName, p => p
                .InitializeUsing(indexState).Map<T>(r => r.AutoMap())
            );

            return response.IsValid;
        }

        public bool PostData(ESInformation eSInformation)
        {
            var index = client.IndexDocument(eSInformation);
            return index.IsValid;
        }

        public void PostBatchData(List<ESInformation> list)
        {
            bool finish = false;
            var tokenSource = new CancellationTokenSource();

            var observableBulk = client.BulkAll(list, f => f
                    .MaxDegreeOfParallelism(8)
                    .BackOffTime(TimeSpan.FromSeconds(10))
                    .BackOffRetries(2)
                    .Size(list.Count)
                    .RefreshOnCompleted()
                    .Index(option.ElasticSearch.Index)
                    .BufferToBulk((r, buffer) => r.IndexMany(buffer))
                , tokenSource.Token);

            var countdownEvent = new CountdownEvent(1);

            var bulkAllObserver = new BulkAllObserver(
                 onNext: response =>
                 {
                 },
                 onError: ex =>
                 {
                     countdownEvent.Signal();
                     throw ex;
                 },
                 () =>
                 {
                     countdownEvent.Signal();
                     finish = true;
                 });

            observableBulk.Subscribe(bulkAllObserver);

            countdownEvent.Wait(tokenSource.Token);

            while (!finish)
            {
                Thread.Sleep(2000);
            }
        }
    }
}
