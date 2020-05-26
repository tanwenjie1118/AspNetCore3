using AspDotNetCore3;
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
    public class SqlSugarTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly ISqlSugarRepository sqlSugar;
        public SqlSugarTest()
        {
            sqlSugar = testWebServer.Resolve<ISqlSugarRepository>();
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

        [Fact]
        public void GetList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var list = sqlSugar.GetList<Company>();
            sw.Stop();

            var timespan = sw.ElapsedMilliseconds;

            timespan.ShouldNotBe(0);

            list.ShouldNotBeNull();
        }

        [Fact]
        public void InsertUser()
        {
            var me = new User()
            {
                Account = "hal",
                Psw = MD5Helper.MD5Encrypt("{password}"),
                Email = "hal_tan@163.com",
                MobilePhone = "1113333111",
                Name = "hal tan",
                NameIdentifier = "xx1133-111x1gg",
                No = "001",
                Role = "admin"
            };

            var coms = sqlSugar.Insert(me);

            coms.ShouldBeGreaterThan(0);
        }

    }
}
