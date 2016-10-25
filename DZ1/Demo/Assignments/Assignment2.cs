using System;

namespace APR.DZ1.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M2_A.txt");
            Matrix b = new Matrix("Input/M2_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);

            // LU decomposition does not work in this example because we get
            // a zero diagonal (pivot) element and a division by zero error
            // The solution is to use LUP to improve the stability
        }
    }
}