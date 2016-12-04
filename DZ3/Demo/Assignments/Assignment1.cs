using System;
using APR.DZ2.Functions;

namespace APR.DZ3.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            var function = new F5(2, -3);

            var method = new SteepestDescentMinimizer();

            method.OptimizeStepSize = false;
            method.Minimize(function, new double[] {0, 0});

            method.OptimizeStepSize = true;
            method.Minimize(function, new double[] {0, 0});
        }
    }
}