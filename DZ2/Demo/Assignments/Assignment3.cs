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
            var opt2 = new NelderMeadSearch();
            opt2.IsOutputPerIterationEnabled = false;
            opt2.Minimize(function, new double[] { 5, 5 });

            // Hooke-Jeeves pattern search
            var opt3 = new HookeJeevesSearch();
            opt3.IsOutputPerIterationEnabled = false;
            opt3.Minimize(function, new double[] { 5, 5 });
        }
    }
}