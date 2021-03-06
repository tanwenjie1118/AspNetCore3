using Hal.AspDotNetCore3;
using Hal.Core.Dapper;
using Hal.Core.Entities;
using Hal.Core.SqlSugar;
using Hal.Infrastructure.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Tests.Hal.Services
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

        [Fact]
        public void GetAll()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var list = dapper.GetList<Company>();
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;

            timespan.ShouldNotBe(0);

            list.ShouldNotBeNull();
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
                    Name = "tan",
                    No = new Random().Next(0, 9999).ToString(),
                    Version = x.ToString()
                });
            });

            var coms = dapper.Insert(list);
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;
            coms.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void InsertModelAsync()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var range = Enumerable.Range(1, 1000).ToList();

            range.ForEach(async (x) =>
            {
                await dapper.InsertAsync(new Company()
                {
                    Name = "tan",
                    No = new Random().Next(0, 9999).ToString(),
                    Version = x.ToString()
                });
            });
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;
            range.FirstOrDefault().ShouldBeGreaterThan(0);
        }


        [Fact]
        public void InsertModelSync()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var range = Enumerable.Range(1, 1000).ToList();

            range.ForEach((x) =>
            {
                dapper.Insert(new Company()
                {
                    Name = "tan",
                    No = new Random().Next(0, 9999).ToString(),
                    Version = x.ToString()
                });
            });

            sw.Stop();
            var timespan = sw.ElapsedMilliseconds;
            range.FirstOrDefault().ShouldBeGreaterThan(0);
        }
    }
}
