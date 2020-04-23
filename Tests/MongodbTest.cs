using AspDotNetCore3;
using Core.MongoDB;
using System;
using Xunit;

namespace Tests
{
    public class MongodbTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IMongoDbRepository<TableFirst> db;
        public MongodbTest()
        {
            db = testWebServer.Resolve<IMongoDbRepository<TableFirst>>();
        }

        [Fact]
        public void Test1()
        {
            var entity = new TableFirst()
            {
                QI = "12",
                G1 = "12",
                X1 = "12",
                YY = "12",
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            };
            db.Add(entity);

            Assert.NotNull(entity.Id);
        }

        [Fact]
        public void Test2()
        {
            var entity = db.Get("ae91eff9-f70b-48c5-83ed-25e9a9708006");

            Assert.NotNull(entity);
        }

        public class TableFirst
        {
            public string Id { get; set; }
            public string QI { get; set; }
            public string X1 { get; set; }
            public string G1 { get; set; }
            public string YY { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
