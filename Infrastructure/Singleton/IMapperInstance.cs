using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Singleton
{
    public class IMapperInstance
    {
        private IMapperInstance()
        {
        }

        private static readonly object obj = new object();
        private static IMapper itself;
        public static IMapper ItSelf
        {
            get
            {

                return itself;
            }
            set
            {
                lock (obj)
                {
                    if (itself == null)
                    {
                        itself = value;
                    }
                }
            }
        }
    }
}
