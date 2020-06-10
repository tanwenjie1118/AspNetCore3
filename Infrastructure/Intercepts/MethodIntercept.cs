using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Intercepts
{
    public class MethodIntercept : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
