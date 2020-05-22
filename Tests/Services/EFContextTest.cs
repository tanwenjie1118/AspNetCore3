using AspDotNetCore3;
using Core.Entityframework;
using Core.Entityframework.Entities;
using HttpReports.Dashboard.Implements;
using Infrastructure;
using Infrastructure.Configuration;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests.Services
{
    public class EFContextTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IEFRepository dbcontext;
        private readonly DatabaseOption dbOption;
        public EFContextTest()
        {
            dbcontext = testWebServer.Resolve<IEFRepository>();
            dbOption = testWebServer.Resolve<DatabaseOption>();
        }

        [Fact]
        public void InitDatabaseModel()
        {
            MyDbContext.Init(dbOption.MySql.Conn);
        }

        [Fact]
        public void InsertModel()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var list = new List<Company>();
            var range = Enumerable.Range(1, 10000).ToList();

            range.ForEach((x) =>
            {
                list.Add(new Company()
                {
                    Name = "hal",
                    No = new Random().Next(0, 9999).ToString(),
                    Version = x.ToString()
                });
            });

            var coms = dbcontext.Insert(list);
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;
            coms.ShouldBeGreaterThan(0);
        }
    }
}
