using System;
using System.Linq;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// Schaffer's function:
    /// <para>X_min = [0, 0, ...]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F6 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            var quadSum = x.Sum(t => Math.Pow(t, 2));
            return 0.5 + (Math.Pow(Math.Sin(Math.Sqrt(quadSum)), 2) - 0.5) / Math.Pow((1 + 0.001 * quadSum), 2);
        }
    }
}
