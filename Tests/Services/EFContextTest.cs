using AspDotNetCore3;
using Core.Entityframework;
using Core.Entityframework.Entities;
using Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Services
{
    public class EFContextTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IEFRepository dbcontext;
        public EFContextTest()
        {
            dbcontext = testWebServer.Resolve<IEFRepository>();
        }

        [Fact]
        public void InitDatabaseModel()
        {
            MyDbContext.Init(Appsettings.Get("Database", "MySql", "Conn"));
        }

        [Fact]
        public void InsertModel()
        {
            var coms = dbcontext.Insert(new Company()
            {
                Name = "haltan",
                No = "11axx11244",
                Version = "v 1.0.1"
            });

            coms.ShouldBeGreaterThan(0);
        }
    }
}
