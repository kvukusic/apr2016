using System;
using System.Linq;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// Almost Schaffer's function:
    /// <para>X_min = [0, 0, ...]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F7 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            var quadSum = x.Sum(t => Math.Pow(t, 2));
            return Math.Pow(quadSum, 0.25)*(1 + Math.Pow(Math.Sin(50*Math.Pow(quadSum, 0.1)), 2));
        }
    }
}
