using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Singleton
{
    public class AutofacContainer
    {
        private AutofacContainer()
        {

        }
        private static readonly object obj = new object();
        private static ILifetimeScope container;
        public static ILifetimeScope Container
        {
            get
            {

                return container;
            }
            set
            {
                lock (obj)
                {
                    if (container == null)
                    {
                        container = value;
                    }
                }
            }
        }
    }
}
