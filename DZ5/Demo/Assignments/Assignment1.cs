using System;
using APR.DZ1;

namespace APR.DZ5.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            try
            {
                Matrix a = new Matrix("Input/M3_A.txt");
            Matrix x = new Matrix("Input/M3_X.txt");

            Console.WriteLine("M1:");
            a.WriteToConsole();
            Console.WriteLine("M2:");
            x.WriteToConsole();

            RungeKuttaMethod rk4 = new RungeKuttaMethod();
            rk4.Solve(a, null, x, 20, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}