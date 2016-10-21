using System;

namespace DZ1.Demo.Assignments
{
    public class Assignment5 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M5_A.txt");
            Matrix b = new Matrix("Input/M5_B.txt");

            LinearEquationSolver.SolveLU(a, b);
            LinearEquationSolver.SolveLUP(a, b);

            // Because there are no fractions there is a slight error in real
            // result vs the computer result
            // For example 9 - 3*2/3 will result in 7 on paper, but 7.0002 in
            // a double variable if we treat 2/3 as 0.6666
            // The error is smaller the more precision we use, in C# double it
            // is the precision of 15 decimals
        }
    }
}