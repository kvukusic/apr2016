using System;
using APR.DZ1;

namespace APR.DZ5
{
    public class TrapezodialMethod : IDifferentialEquationSolver
    {
        public TrapezodialMethod()
        {
        }

        public string Name
        {
            get { return "Trapezodial"; }
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
            // x_k+1 = R*x_k + S
            var temp = (Matrix.Identity(x0.Rows) - a * stepSize / 2).ToInverseMatrix();
            Matrix R = temp * (Matrix.Identity(x0.Rows) + a * stepSize / 2);
            Matrix S = temp * stepSize / 2 * b;

            int iterations = (int)Math.Ceiling((tmax - tmin) / stepSize);
            int k = 0;

            Matrix result = x0.Copy();
            Matrix xk = x0.Copy();

            while (k++ < iterations)
            {
                result = R * xk + S;
                xk = result.Copy();
            }

            return result;
        }
    }
}