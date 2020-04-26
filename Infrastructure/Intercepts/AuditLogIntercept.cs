using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Intercepts
{
    public class AuditLogIntercept : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
