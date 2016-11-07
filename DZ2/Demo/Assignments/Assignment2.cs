using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ2.Extensions;
using APR.DZ1.Extensions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            var result = new List<Tuple<string, string, string, string>>();
            var functions = new List<Tuple<Function, double[]>>()
            {
                new Tuple<Function, double[]>(new F1(), new double[] {-1.9, 2.0}),
                new Tuple<Function, double[]>(new F2(), new double[] {0.1, 0.3}),
                new Tuple<Function, double[]>(new F3(), new double[] {0,0,0,0,0}),
                new Tuple<Function, double[]>(new F4(), new double[] {5.1, 1.1})
            };

            for (int i = 0; i < functions.Count; i++)
            {
                var item = functions[i];
                var opt1 = new NelderMeadSearch();
                opt1.IsOutputEnabled = false;
                var min1 = opt1.Minimize(item.Item1, item.Item2.Copy());
                var opt1Evals = item.Item1.Evaluations;

                var opt2 = new HookeJeevesSearch();
                opt2.IsOutputEnabled = false;
                var min2 = opt2.Minimize(item.Item1, item.Item2.Copy());
                var opt2Evals = item.Item1.Evaluations;

                var opt3 = new CoordinateDescent();
                opt3.IsOutputEnabled = false;
                var min3 = opt3.Minimize(item.Item1, item.Item2.Copy());
                var opt3Evals = item.Item1.Evaluations;

                result.Add(new Tuple<string, string, string, string>((i + 1).ToString(),
                    opt1Evals.ToString() + " (min: " + item.Item1.Value(min1).ToString("F20") + ")",
                    opt2Evals.ToString() + " (min: " + item.Item1.Value(min2).ToString("F20") + ")",
                    opt3Evals.ToString() + " (min: " + item.Item1.Value(min3).ToString("F20") + ")"));
            }

            Console.WriteLine(result.ToStringTable(
                new[] {"Function", "Nelder and Mead", "Hooke-Jeeves", "Coordinate Descent"},
                a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4));
            Console.WriteLine();

            // Hooke-Jeeves will stuck at function F4 because of the nature of exploration which explores a better point only
            // in one axis, not all at once, and because at point [3.1 3.1] only [2.975 2.975] is fitter, the algorithm
            // can't determine that there is a better point in the environment
        }
    }
}