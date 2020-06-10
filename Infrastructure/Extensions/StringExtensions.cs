using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool ToBool(this string str)
        {
            if (bool.TryParse(str, out var res))
            {
                return res;
            }
            else
            {
                return false;
            }
        }

        public static int ToInt32(this string str)
        {
            if (int.TryParse(str, out var res))
            {
                return res;
            }
            else
            {
                return 0;
            }
        }
    }
}
