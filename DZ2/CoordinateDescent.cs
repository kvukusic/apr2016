using System;
using System.Linq;
using APR.DZ1;
using APR.DZ1.Extensions;
using APR.DZ2.Functions;

namespace APR.DZ2
{
    public class CoordinateDescent : IMinimizer
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private static readonly int MAX_ITERATIONS = 100000;

        /// <summary>
        /// The upper bound of the interval used in finding the optimal <c>lambda</c>
        /// for the minimization function.
        /// </summary>
        private static readonly double INITIAL_GOLDEN_SECTION_INTERVAL = 0.01;

        public bool IsOutputEnabled { get; set; }

        public bool IsOutputPerIterationEnabled { get; set; }

        public CoordinateDescent()
        {
            IsOutputPerIterationEnabled = true;
            IsOutputEnabled = true;
        }

        public double[] Minimize(Function f, double[] start)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            // Clear f
            f.Clear();

            double[] e = new double[start.Length].Fill(EPSILON);

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("**********************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Coordinate Descent method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine("e = " + e.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            int iterations = 0;

            var x = new Vector(start);
            var xs = x.Copy();
            do
            {
                iterations++;

                xs = x.Copy();
                for (int i = 0; i < start.Length; i++)
                {
                    Vector identity = Vector.Identity(start.Length, i);
                    double minLambda = CalculateOptimalStepSize(f, x, identity);
                    x[i] += minLambda*identity[i];
                }

                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    f.DisableStatistics();
                    LogIteration(iterations, x, f.Value(x.AsArray()));
                    f.EnableStatistcs();
                }

                if(iterations == MAX_ITERATIONS)
                {
                    break;
                }

            } while (x.Select((xi, i) => Math.Abs(xi - xs[i]) > e[i]).Any(v => !v));

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            if (IsOutputEnabled)
            {
                var evaluations = f.Evaluations;
                var cachedCalls = f.CachedCalls;
                ConsoleEx.WriteLineGreen("Final position found. Returning value: x = " + x.AsArray().Format(PRECISION));
                if(iterations == MAX_ITERATIONS) ConsoleEx.WriteLineRed($"Maximum number of iterations were hit [{MAX_ITERATIONS}]");
                ConsoleEx.WriteLineGreen("Function value of final position is: " + f.Value(x.AsArray()).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + iterations);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine(); 
            }

            return x.AsArray();
        }

        private void LogIteration(int iteration, Vector x, double fx)
        {
            var precision = "F" + PRECISION;
            Console.Write("[{0,3:D3}]", iteration);
            Console.Write(" x = " + x.AsArray().Format(PRECISION).PadRight(PRECISION + 4));
            Console.Write(" f(x) = " + fx.ToString(precision).PadRight(PRECISION + 4));
            Console.WriteLine();
        }

        private double CalculateOptimalStepSize(Function f, Vector currentPoint, Vector identity)
        {
            var g = new DelegateFunction(lambda =>
            {
                return f.Value(currentPoint.Select((d, i) => d + lambda[0]*identity[i]).ToArray());
            });

            // Find the upper interval for the golden section line search
            var s = INITIAL_GOLDEN_SECTION_INTERVAL;
            var functionValue = f.Value(currentPoint.AsArray());

            // Double s until f(x - s*grad) > f(x)
            while (f.Value(currentPoint.Select((d, i) => d + s*identity[i]).ToArray()) <= functionValue)
            {
                s *= 2;
            }

            var minimizer = new GoldenSectionSearch();
            minimizer.IsOutputEnabled = false;
            return minimizer.Minimize(g, new GoldenSectionSearch.Interval(0, s));
        }
    }
}