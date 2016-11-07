using System;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// <para>f(X) = (x1 - 4)^2 + 4*(x2 - 2)^2</para>
    /// <para>X0 = [0.1, 0.3]</para>
    /// <para>X_min = [4, 2]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F2 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            var x1 = x[0];
            var x2 = x[1];
            return Math.Pow(x1 - 4, 2) + 4 * Math.Pow(x2 - 2, 2);
        }
    }
}
