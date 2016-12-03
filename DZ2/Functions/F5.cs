using System;
using System.Linq;
using APR.DZ1;
using APR.DZ1.Extensions;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// <para>f(X) = sum_i(x_i - parameters[i])^2</para>
    /// <para>X0 = [0, 0, ...]</para>
    /// <para>X_min = [-parameters[0], -parameters[1], ..., -parameters[n-1]]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F5 : Function
    {
        private double[] _parameters;

        public F5(params double[] parameters)
        {
            _parameters = parameters.Copy();
        }

        protected override double ValueEx(params double[] x)
        {
            return x.Select((t, i) => Math.Pow(t - _parameters[i], 2)).Sum();
        }

        protected override double[] GradientEx(params double[] x)
        {
            var x1 = x[0];
            var x2 = x[1];

            return new double[]
            {
                2*(x1 - _parameters[0]),
                2*(x2 - _parameters[1])
            };
        }

        protected override Matrix HessianEx(params double[] x)
        {
            Matrix result = new Matrix(2,2);
            result[0][0] = result[1][1] = 2;
            result[0][1] = result[1][0] = 0;
            return result;
        }
    }
}
