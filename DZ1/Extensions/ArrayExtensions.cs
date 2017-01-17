using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace APR.DZ1.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Copy<T>(this T[] src)
        {
            if (src is BitArray[])
            {
                return (src as BitArray[]).Copy() as T[];
            }

            var dest = new T[src.Length];
            Array.Copy(src, dest, dest.Length);
            return dest;
        }

        public static double[] Copy(this double[] src)
        {
            var dest = new double[src.Length];
            Array.Copy(src, dest, dest.Length);
            return dest;
        }

        public static BitArray[] Copy(this BitArray[] src)
        {
            var dest = new BitArray[src.Length];
            for (int i = 0; i < dest.Length; i++)
            {
                dest[i] = new BitArray(src[i]);
            }
            return dest;
        }

        public static string Format(this int[] src)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i = 0; i < src.Length; i++)
            {
                sb.Append(src[i]);
                if (i != src.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append("]");

            return sb.ToString();
        }

        public static string Format(this double[] src, int precision = 3, bool addBracket = true, bool addSpaces = true)
        {
            StringBuilder sb = new StringBuilder();

            if (addBracket)
                sb.Append("[");

            for (int i = 0; i < src.Length; i++)
            {
                sb.Append(src[i].ToString("F" + precision));
                if (i != src.Length - 1)
                {
                    if (addSpaces)
                        sb.Append(", ");
                    else
                        sb.Append(",");
                }
            }

            if (addBracket)
                sb.Append("]");

            return sb.ToString();
        }

        public static string Format(this double[] src, int precision = 3)
        {
            return src.Format(3, true);
        }

        public static double[] Fill(this double[] src, double value)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = value;
            }
            
            return src;
        }

        public static Vector ToVector(this double[] src)
        {
            return new Vector(src.Copy());
        }

        public static double Norm(this double[] src)
        {
            var srcCopy = src.Copy();
            return Math.Sqrt(srcCopy.Sum(d => d*d));
        }
    }
}
