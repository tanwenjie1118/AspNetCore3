using Hal.Applications.Model;
using Hal.AspDotNetCore3;
using AutoMapper;
using Hal.Infrastructure.Singleton;
using Hal.Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Hal.Services
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
            var list = new List<MyModelX>();
            var model = new MyModelX()
            {
                IDFX = "oxa1123ddf打"
            };

            list.Add(model);
            // var dto = mapper.Map<MyModelXDto>(model);
            var list1 = mapper.Map<List<MyModelXDto>>(list);

            //  Assert.NotNull(dto);
            Assert.NotNull(list1);
        }
    }
}
