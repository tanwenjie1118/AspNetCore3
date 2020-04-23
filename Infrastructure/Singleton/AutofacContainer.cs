using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Singleton
{
    public class AutofacContainer
    {
        private AutofacContainer()
        {

        }

        private static ILifetimeScope container;
        public static ILifetimeScope Container
        {
            get
            {

                return container;
            }
            set
            {
                if (container == null)
                {
                    container = value;
                }
            }
        }
    }
}
