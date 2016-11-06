using System;
using APR.DZ1.Extensions;
using APR.DZ2.Functions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            var function = new F4();

            // Generate simplex with offset from 1 to 20 and in point [0.5, 0.5]
            Console.WriteLine("Case 1: Generate simplex with offset from 1 to 20 and in point [0.5, 0.5].");
            for (int i = 1; i <= 20; i++)
            {
                var opt = new NelderMeadSearch();
                opt.IsOutputEnabled = false;
                opt.IsOutputPerIterationEnabled = false;
                opt.SimplexOffset = i;
                double[] min = opt.Minimize(function, new double[] {0.5, 0.5});
                Console.WriteLine($"offset={i}, evals={function.Evaluations}, x_min={min.Format(16)}, f(x_min)={function.Value(min).ToString("F16")}");
                function.Clear();
            }

            Console.WriteLine();

            // Generate simplex with offset from 1 to 20 and in point [20, 20]
            Console.WriteLine("Case 2: Generate simplex with offset from 1 to 20 and in point [20, 20].");
            for (int i = 1; i <= 20; i++)
            {
                var opt = new NelderMeadSearch();
                opt.IsOutputEnabled = false;
                opt.IsOutputPerIterationEnabled = false;
                opt.SimplexOffset = i;
                double[] min = opt.Minimize(function, new double[] { 20, 20 });
                Console.WriteLine($"offset={i}, evals={function.Evaluations}, x_min={min.Format(16)}, f(x_min)={function.Value(min).ToString("F16")}");
                function.Clear();
            }
        }
    }
}