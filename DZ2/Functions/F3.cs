using System;
using System.Linq;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// <para>f(X) = sum_i(x_i - i)^2</para>
    /// <para>X0 = [0, 0, ...]</para>
    /// <para>X_min = [1, 2, 3, ... , n]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F3 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            // return x.Select((t, i) => Math.Pow(t - _parameters[i], 2)).Sum();
            return x.Select((xi, i) => Math.Pow(xi - (i + 1), 2)).Sum();
        }
    }
}
