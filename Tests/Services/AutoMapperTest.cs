using Applications.Model;
using AspDotNetCore3;
using AutoMapper;
using Infrastructure.Singleton;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Services
{
    public class AutoMapperTest
    {
        private readonly TestWebServer<Startup> testWebServer = new TestWebServer<Startup>();
        private readonly IMapper mapper;
        public AutoMapperTest()
        {
            mapper = testWebServer.Resolve<IMapper>();
        }

        [Fact]
        public void TestMapper()
        {
            var model = new MyModelX()
            {
                IDFX = "oxa1123ddf打"
            };

            var dto = mapper.Map<MyModelXDto>(model);

            Assert.NotNull(dto);
        }
    }
}
