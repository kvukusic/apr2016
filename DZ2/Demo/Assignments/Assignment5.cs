using System;
using APR.DZ2.Functions;

namespace APR.DZ2.Demo.Assignments
{
    public class Assignment5 : IAssignment
    {
        public void Run()
        {
            var random = new Random();
            var function = new F6();
            int foundCounter = 0;
            for (int i = 0; i < 10000; i++)
            {
                var opt = new NelderMeadSearch();
                opt.IsOutputEnabled = false;
                opt.IsOutputPerIterationEnabled = false;
                var final = opt.Minimize(function, new double[] { random.Next(-50, 51) });
                if (function.Value(final) < 10e-4) foundCounter++;
            }

            Console.WriteLine("Probability: " + (foundCounter / (double)10000).ToString("F2") + "%");
            Console.WriteLine();
        }
    }
}