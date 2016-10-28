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
            var result = new List<Tuple<string, string, string>>();
            var functions = new List<Tuple<Function, double[]>>()
            {
                new Tuple<Function, double[]>(new F1(), new double[] {-1.9, 2.0}),
                new Tuple<Function, double[]>(new F2(), new double[] {0.1, 0.3}),
                new Tuple<Function, double[]>(new F3(1,1,1,1,1), new double[] {0,0,0,0,0}),
                new Tuple<Function, double[]>(new F4(), new double[] {5.1, 1.1})
            };

            for (int i = 0; i < functions.Count; i++)
            {
                var item = functions[i];
                var opt1 = new NelderMeadSearch();
                opt1.IsOutputEnabled = false;
                var min1 = opt1.Minimize(item.Item1, item.Item2.Copy());
                var opt1Evals = item.Item1.Evaluations;

                var opt2 = new HookeJeevesSearch(Enumerable.Repeat(0.5, item.Item2.Length).ToArray(),
                    Enumerable.Repeat(10e-6, item.Item2.Length).ToArray());
                opt2.IsOutputEnabled = false;
                var min2 = opt2.Minimize(item.Item1, item.Item2.Copy());
                var opt2Evals = item.Item1.Evaluations;

                result.Add(new Tuple<string, string, string>((i + 1).ToString(),
                    opt1Evals.ToString() + " (min: " + item.Item1.Value(min1).ToString("F20") + ")",
                    opt2Evals.ToString() + " (min: " + item.Item1.Value(min2).ToString("F20") + ")"));
            }

            Console.WriteLine(result.ToStringTable(
                new[] {"Function", "Nelder and Mead", "Hooke-Jeeves"},
                a => a.Item1, a => a.Item2, a => a.Item3));
            Console.WriteLine();
        }
    }
}