using System;
using APR.DZ2.Functions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            var function = new F3(3);

            for (int i = 10; i <= 15; i++)
            {
                // Golden section search
                var opt1 = new GoldenSectionSearch();
                opt1.IsOutputPerIterationEnabled = false;
                opt1.Minimize(function, i);

                // Nelder-Mead search
                var opt2 = new NelderMeadSearch();
                opt2.IsOutputPerIterationEnabled = false;
                opt2.Minimize(function, new double[] {i});

                // Hooke-Jeeves pattern search
                var opt3 = new HookeJeevesSearch(new double[] { 0.5, 0.5 }, new double[] { 10e-6, 10e-6 });
                opt3.IsOutputPerIterationEnabled = false;
                opt3.Minimize(function, new double[] { i });
            }
        }
    }
}