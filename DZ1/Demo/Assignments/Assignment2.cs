using System;

namespace DZ1.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M2_A.txt");
            Matrix b = new Matrix("Input/M2_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);
        }
    }
}