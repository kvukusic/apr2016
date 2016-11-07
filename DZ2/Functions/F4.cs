using System;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// Jakobovićeva funkcija:
    /// <para>f(X) = |(x1 - x2)*(x1 + x2)| + SQRT(x1^2 + x2^2)</para>
    /// <para>X0 = [5.1, 1.1]</para>
    /// <para>X_min = [0, 0]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F4 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            var x1 = x[0];
            var x2 = x[1];
            return Math.Abs((x1 - x2) * (x1 + x2)) + Math.Sqrt(x1 * x1 + x2 * x2);
        }
    }
}
