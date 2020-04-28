using AspDotNetCore3;
using Core.Redis;
using Core.SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.Services
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
        public void GetList()
        {
            var coms = sqlSugar.GetList<COMPANY>();

            Assert.NotNull(coms);
        }

        [Fact]
        public void GetFirst()
        {
            var coms = sqlSugar.Get<COMPANY>(t => t.ID > 0);

            Assert.NotNull(coms);
        }

        [Fact]
        public void InsertModel()
        {
            var coms = sqlSugar.Insert(new COMPANY()
            {
                ID = 8,
                ADDRESS = "aAAAAAAAAAAA",
                AGE = 98,
                NAME = "oooooooooooooooopppp",
                SALARY = 44444444
            });

            Assert.True(coms > 0);
        }

        public class COMPANY
        {
            [Key]
            public int ID { get; set; }
            public string NAME { get; set; }
            public int AGE { get; set; }
            public string ADDRESS { get; set; }
            public decimal SALARY { get; set; }
        }
    }
}
