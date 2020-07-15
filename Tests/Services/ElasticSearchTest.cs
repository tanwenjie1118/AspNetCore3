using Hal.AspDotNetCore3;
using Xunit;
using Hal.Core.ElasticSearch;
using Hal.Core.ElasticSearch.Model;
using System;

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
        public void TestPost()
        {
            var model = new ESInformation()
            {
                Title = "FFFFFF112225113",
                Business_type = 1,
                BusNo = 11,
                CompanyId = 2144233114,
                CreateDate = DateTime.Now,
                Level = "info",
                Description = "This is just a test request for es 1",
                Online = false
            };

            var list1 = repository.PostData(model);

            Assert.True(list1);
        }
    }
}
