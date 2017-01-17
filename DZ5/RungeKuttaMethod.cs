using System;
using APR.DZ1;

namespace APR.DZ5
{
    public class RungeKuttaMethod : IDifferentialEquationSolver
    {
        public RungeKuttaMethod()
        {
        }

        public string Name
        {
            get { return "Runge-Kutta(4)"; }
        }

        public Matrix Solve(Matrix a, Matrix x0, double tmax, double stepSize)
        {
            return Solve(a, Matrix.Zero(x0.Rows, x0.Columns), x0, tmax, stepSize);
        }

        public Matrix Solve(Matrix a, Matrix b, Matrix x0, double tmax, double stepSize)
        {
            return Solve(a, b, x0, 0, tmax, stepSize);
        }

        public Matrix Solve(Matrix a, Matrix b, Matrix x0, double tmin, double tmax, double stepSize)
        {
            // m1 = Ax_k + B
            // m2 = A(x_k + m1*T/2) + B
            // m3 = A(x_k + m2*T/2) + B
            // m4 = A(x_k + m3*T) + B
            // x_k+1 = x_k + T/6*(m1 + 2m2 + 2m3 + m4)

            int iterations = (int)Math.Ceiling((tmax - tmin) / stepSize);
            int k = 0;

            Matrix result = x0.Copy();
            Matrix xk = x0.Copy();

            while (k++ < iterations)
            {
                Matrix m1 = a * xk + b;
                Matrix m2 = a * (xk + m1 * stepSize / 2) + b;
                Matrix m3 = a * (xk + m2 * stepSize / 2) + b;
                Matrix m4 = a * (xk + m3 * stepSize) + b;

                result = xk + stepSize / 6 * (m1 + 2 * m2 + 2 * m3 + m4);
                xk = result.Copy();
            }

            return result;
        }
    }
}
