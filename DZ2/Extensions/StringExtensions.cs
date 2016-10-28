using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APR.DZ2.Extensions
{
    public static class StringExtensions
    {
        public static string PadCenter(this string s, int width, char c)
        {
            if (s == null || width <= s.Length) return s;

            int padding = width - s.Length;
            return s.PadLeft(s.Length + padding / 2, c).PadRight(width, c);
        }
    }
}
