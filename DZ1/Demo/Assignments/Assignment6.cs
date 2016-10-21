using System;

namespace DZ1.Demo.Assignments
{
    public class Assignment6 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M6_A.txt");
            Matrix b = new Matrix("Input/M6_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);

            double scale = 10e3;
            LinearEquationSolver.SolveLU(a*scale, b*scale);
            LinearEquationSolver.SolveLUP(a*scale, b*scale);

            // If we use EPSILON 10e-6 the algorithm will fail because we treat
            // 0.0000000003 as zero and return as divide by zero error.
            // The solution is to multiply the whole system with a scale factor
            // so that there is a value larger than the matrix precision.
        }
    }
}