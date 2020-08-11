using Hal.AspDotNetCore3;
using Xunit;
using Hal.Core.ElasticSearch;
using Hal.Core.ElasticSearch.Model;
using System;
using System.Linq;
using Nest;
using System.Collections.Generic;

namespace Tests.Hal.Services
{
    public class ElasticSearchTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IESRepository repository;
        public ElasticSearchTest()
        {
            repository = testWebServer.Resolve<IESRepository>();
        }

        [Fact]
        public void TestPostBatch()
        {
            var list = new List<ESInformation>();
            var ids = Enumerable.Range(10000, 50000);
            foreach (var id in ids)
            {
                list.Add(new ESInformation()
                {
                    Title = $"FFFFFF{id}",
                    Business_type = 1,
                    BusNo = 11,
                    CompanyId = 2144233114,
                    CreateDate = DateTime.Now,
                    Level = "info",
                    Description = $"This is just a test request for es {id}",
                    Online = true
                });
            }

            repository.PostBatchData(list);

            Assert.True(true);
        }
    }
}
