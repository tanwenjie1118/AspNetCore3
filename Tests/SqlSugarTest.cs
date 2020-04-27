using AspDotNetCore3;
using Core.Redis;
using Core.SqlSugar;
using System;
using Xunit;

namespace Tests
{
    public class SqlSugarTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly ISqlSugarRepository sqlSugar;
        public SqlSugarTest()
        {
            sqlSugar = testWebServer.Resolve<ISqlSugarRepository>();
        }

        [Fact]
        public void TestSugar()
        {
            var coms = sqlSugar.GetList<COMPANY>();

            Assert.NotNull(coms);
        }

        public class COMPANY
        {
            public int ID { get; set; }
            public string NAME { get; set; }
            public int AGE { get; set; }
            public string ADDRESS { get; set; }
            public decimal SALARY { get; set; }
        }
    }
}
