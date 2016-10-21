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
        }
    }
}