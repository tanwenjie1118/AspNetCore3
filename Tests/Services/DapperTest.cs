using AspDotNetCore3;
using Core.Dapper;
using Core.Entities;
using Core.SqlSugar;
using Infrastructure.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Tests.Services
{
    public class DapperTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IDapperRepository dapper;
        public DapperTest()
        {
            dapper = testWebServer.Resolve<IDapperRepository>();
        }


        [Fact]
        public void GetFirst()
        {
            var coms = dapper.Get<Company>();

            Assert.NotNull(coms);
        }
    }
}
