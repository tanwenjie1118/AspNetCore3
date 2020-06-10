using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Services.Application
{
    public class ServicesModule : Module
    {
        /// <summary>
        /// Add types to autofac ioc container
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(JobServices)).As(typeof(IJobServices));
        }
    }
}
