using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Attributes
{
    /// <summary>
    /// Swagger api hidden attribute
    /// </summary>
    /// <summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenApiAttribute : Attribute
    {
    }
}
