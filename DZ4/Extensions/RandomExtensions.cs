using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rand, double min, double max)
        {
            if(max < min) throw new ArgumentException("max < min");

            return rand.NextDouble()*(max - min) + min;
        }

        public static bool NextBool(this Random rand)
        {
            return rand.Next(0, 100) < 50;
        }

        public static int[] NextInts(this Random rand, int min, int max, int n, int[] excluded = null)
        {
            if(max < min) throw new ArgumentException("max < min");
            if(max - min - 1 < n) throw new ArgumentException("n is larger than the specified range");

            var result = new HashSet<int>();
            if (excluded != null && excluded.Any())
            {
                var excludedHashSet = new HashSet<int>(excluded);
                for (int i = 0; i < n; i++)
                {
                    var num = rand.Next(min, max);
                    while (result.Contains(num) || excludedHashSet.Contains(num))
                    {
                        num = rand.Next(min, max);
                    }

                    result.Add(num);
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    var num = rand.Next(min, max);
                    while (result.Contains(num))
                    {
                        num = rand.Next(min, max);
                    }

                    result.Add(num);
                }
            }

            return result.ToArray();
        }
    }
}