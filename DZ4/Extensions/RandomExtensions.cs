using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="rand">The random number generator.</param>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.
        /// max must be greater than or equal to min.</param>
        /// <returns></returns>
        public static int NextInt(this Random rand, int min, int max)
        {
            if(max < min) throw new ArgumentException("max < min");

            return rand.Next(min, max);
        }

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

        /// <summary>
        /// Generates normally distributed numbers.
        /// </summary>
        /// <param name="r"></param>
        /// <param name = "mu">Mean of the distribution</param>
        /// <param name = "sigma">Standard deviation</param>
        /// <returns>A random normal number.</returns>
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}