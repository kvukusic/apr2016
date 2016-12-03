using System;
using APR.DZ1;

namespace APR.DZ2.Functions
{
    /// <summary>
    /// Rosenbrock's 'banana' function:
    /// <para>f(X) = 100*(x2 - x1^2) + (1 - x1)^2</para>
    /// <para>X0 = [-1.9, 2]</para>
    /// <para>X_min = [1, 1]</para>
    /// <para>f(X_min) = 0</para>
    /// </summary>
    public class F1 : Function
    {
        protected override double ValueEx(params double[] x)
        {
            var x1 = x[0];
            var x2 = x[1];
            return 100 * Math.Pow(x2 - Math.Pow(x1, 2), 2) + Math.Pow(1 - x1, 2);
        }

        protected override double[] GradientEx(params double[] x)
        {
            double x1 = x[0];
            double x2 = x[1];

            return new double[]
            {
                2*(200*Math.Pow(x1, 3) - 200*x1*x2 + x1 - 1),
                200*(x2 - x1*x1)
            };
        }

        protected override Matrix HessianEx(params double[] x)
        {
            double x1 = x[0];
            double x2 = x[1];

            Matrix result = new Matrix(2, 2);
            result[0][0] = 1200*x1*x1 - 400*x2 + 2;
            result[0][1] = result[1][0] = -400*x1;
            result[1][1] = 200;
            return result;
        }
    }
}
