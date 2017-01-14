using System;
using APR.DZ1;

namespace APR.DZ5
{
    public class TrapezodialMethod
    {
        public TrapezodialMethod()
        {
            
        }

        public Matrix Solve(Matrix a, Matrix b, Matrix x0, double tmax, double stepSize) {
                // x_k+1 = R*x_k + S
                Matrix R = (Matrix.Identity(x0.Rows) - a*stepSize/2).ToInverseMatrix()*(Matrix.Identity(x0.Rows) + a*stepSize/2);
                Matrix S = (Matrix.Identity(x0.Rows) - a*stepSize/2).ToInverseMatrix()*stepSize/2*b;

                int iterations = (int)Math.Ceiling(tmax/stepSize);
                int k = 0;

                Matrix result = null;
                Matrix xk = x0.Copy();
                while (k++ < iterations)
                {
                    result = R*xk + S;
                    xk = result.Copy();
                }

                return result;
            }
    }
}