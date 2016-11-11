using System;
using APR.DZ2.Functions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            var function = new F3();
            // var function = new DelegateFunction(x => Math.Pow(x[0] - 3, 2));

            for (int i = 10; i <= 15; i++)
            {
                // Coordinate descent method
                var opt1 = new CoordinateDescent();
                opt1.IsOutputPerIterationEnabled = false;
                opt1.Minimize(function, new double[] {i});

                // Golden section search
                var opt2 = new GoldenSectionSearch();
                opt2.IsOutputPerIterationEnabled = false;
                opt2.Minimize(function, i);

                // Nelder-Mead search
                var opt3 = new NelderMeadSearch();
                opt3.IsOutputPerIterationEnabled = false;
                opt3.Minimize(function, new double[] {i});

                // Hooke-Jeeves pattern search
                var opt4 = new HookeJeevesSearch();
                opt4.IsOutputPerIterationEnabled = false;
                opt4.Minimize(function, new double[] {i});
            }
        }
    }
}