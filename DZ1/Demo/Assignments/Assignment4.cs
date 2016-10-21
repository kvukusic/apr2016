using System;

namespace DZ1.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M4_A.txt");
            Matrix b = new Matrix("Input/M4_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);

            // The difference between LU and LUP is that if you divide by smaller
            // numbers you get a smaller error, bit if you divide by larger numbers
            // you get a larger error (this is referred to as stability of the algorithm)
        }
    }
}