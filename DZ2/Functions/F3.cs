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
        private readonly double[] _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="F3"/> class.
        /// </summary>
        public F3(params double[] parameters)
        {
            _parameters = parameters;
        }

        protected override double ValueEx(params double[] x)
        {
            return x.Select((t, i) => Math.Pow(t - _parameters[i], 2)).Sum();
        }
    }
}
