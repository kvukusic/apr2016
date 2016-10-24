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

            double scale1 = 1e9;
            double scale2 = 10e9;
            a.SetRow(0, a.GetRow(0)/scale1);
            b.SetRow(0, b.GetRow(0)/scale1);
            a.SetRow(2, a.GetRow(2)*scale2);
            b.SetRow(2, b.GetRow(2)*scale2);
            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);

            // If we use EPSILON 10e-6 the algorithm will fail because we treat
            // 0.0000000003 as zero and return as divide by zero error.
            // The solution is to multiply a single equation with a scale factor
            // so that there is a value larger than the matrix precision.
        }
    }
}