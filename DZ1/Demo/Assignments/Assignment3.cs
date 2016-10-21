using System;

namespace DZ1.Demo.Assignments
{
    public class Assignment3 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M3_A.txt");
            Matrix b = new Matrix("Input/M3_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);
        }
    }
}