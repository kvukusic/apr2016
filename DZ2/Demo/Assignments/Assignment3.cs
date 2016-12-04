using System;
using APR.DZ2.Functions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment3 : IAssignment
    {
        public void Run()
        {
            var function = new F4();

            // Nelder-Mead search
            var opt1 = new NelderMeadMinimizer();
            opt1.IsOutputPerIterationEnabled = false;
            opt1.Minimize(function, new double[] { 5, 5 });

            // Hooke-Jeeves pattern search
            var opt2 = new HookeJeevesMinimizer();
            opt2.IsOutputPerIterationEnabled = false;
            opt2.Minimize(function, new double[] { 5, 5 });
        }
    }
}