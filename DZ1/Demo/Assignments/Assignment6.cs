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
        }
    }
}