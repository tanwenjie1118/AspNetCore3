using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Application
{
    public class ServicesModule : Module
    {
        /// <summary>
        /// 将接口注册到容器autofac中
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(JobServices)).As(typeof(IJobServices));
        }
    }
}
