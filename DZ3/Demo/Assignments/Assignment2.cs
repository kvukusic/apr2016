using System;
using APR.DZ2.Functions;

namespace APR.DZ3.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            var function1 = new F1();
            var function2 = new F2();

            var method1 = new SteepestDescentMinimizer();
            method1.IsOutputPerIterationEnabled = false;

            method1.OptimizeStepSize = true;
            method1.Minimize(function1, new double[] { -1.9, 2.0 });

            method1.OptimizeStepSize = true;
            method1.Minimize(function2, new double[] { 0.1, 0.3 });

            var method2 = new NewtonRaphsonMinimizer();

            method2.OptimizeStepSize = true;
            method2.Minimize(function1, new double[] { -1.9, 2.0 });

            method2.OptimizeStepSize = true;
            method2.Minimize(function2, new double[] { 0.1, 0.3 });
        }
    }
}