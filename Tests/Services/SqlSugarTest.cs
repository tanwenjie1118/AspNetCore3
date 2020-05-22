using AspDotNetCore3;
using Core.Entityframework.Entities;
using Core.Redis;
using Core.SqlSugar;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
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
            var coms = sqlSugar.GetList<Company>();

            coms.ShouldNotBe(null);
        }

        [Fact]
        public void GetPagedList()
        {
            var coms = sqlSugar.GetPagedList<Company>(1, 2, out var tcount, out var psize);
            tcount.ShouldBeGreaterThan(0);
            psize.ShouldBeGreaterThan(0);
            coms.ShouldNotBe(null);
        }

        [Fact]
        public void GetFirst()
        {
            var coms = sqlSugar.Get<Company>(t => t.Id > 0);

            Assert.NotNull(coms);
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

            var coms = sqlSugar.Insert(list);
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;
            coms.ShouldBeGreaterThan(0);
        }
    }
}
