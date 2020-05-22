using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull(this object obj)
        {
            return !(obj is null);
        }

        public static bool IsNull(this object obj)
        {
            return obj is null;
        }

        public static bool IsNotNullOrWhiteSpace(this string obj)
        {
            return !string.IsNullOrWhiteSpace(obj);
        }
    }
}
