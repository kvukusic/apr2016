using System;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ1.Extensions;

namespace APR.DZ2
{
    public class HookeJeevesSearch : IMinimizer
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private static readonly double DEFAULT_DX = 0.5;
        private static readonly double DEFAULT_E = EPSILON;

        /// <summary>
        /// Initializes a new instance of the <see cref="HookeJeevesSearch"/> class.
        /// </summary>
        public HookeJeevesSearch()
        {
            IsOutputPerIterationEnabled = true;
            IsOutputEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whetehr values of every iteration
        /// should be output.
        /// </summary>
        public bool IsOutputPerIterationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating this algorithm produces any output.
        /// </summary>
        public bool IsOutputEnabled { get; set; }

        public double[] Minimize(Function f, double[] start)
        {
            double[] dx = new double[start.Length].Fill(DEFAULT_DX);
            double[] e = new double[start.Length].Fill(DEFAULT_E);

            return Minimize(f, start, dx, e);
        }

        public double[] Minimize(Function f, double[] start, double[] dx, double[] eps)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if(dx == null)
            {
                throw new ArgumentNullException(nameof(dx));
            }

            if(eps == null)
            {
                throw new ArgumentNullException(nameof(eps));
            }

            if(start.Length != dx.Length)
            {
                throw new ArgumentException("Invalid parameter dimension.", nameof(dx));
            }

            if(start.Length != eps.Length)
            {
                throw new ArgumentException("Invalid parameter dimension.", nameof(eps));
            }

            // Clear f
            f.Clear();

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Hooke-Jeeves Pattern Search method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("dx = " + dx.Format(PRECISION));
                ConsoleEx.WriteLine("e = " + eps.Format(PRECISION));
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            int iterations = 0;

            double[] x0 = start.Copy();
            double[] xb = x0.Copy();
            double[] xp = x0.Copy();
            
            do
            {
                iterations++;

                double[] xn = Explore(f, xp, dx);

                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    f.DisableStatistics();
                    LogIteration(iterations, xb, xp, xn, f.Value(xb), f.Value(xp), f.Value(xn), dx);
                    f.EnableStatistcs();
                }

                if (f.Value(xn) < f.Value(xb))
                {
                    for (int i = 0; i < xp.Length; i++)
                    {
                        xp[i] = 2*xn[i] - xb[i];
                    }

                    xb = xn.Copy();
                }
                else
                {
                    for (int i = 0; i < dx.Length; i++)
                    {
                        dx[i] = 0.5*dx[i];
                    }

                    xp = xb.Copy();
                }
            } while (dx.Where((t, i) => t > eps[i]).Any());

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            var evaluations = f.Evaluations;
            var cachedCalls = f.CachedCalls;

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLineGreen("Final position found. Returning value: x = " + xb.Format(PRECISION));
                ConsoleEx.WriteLineGreen("Function value of final position is: " + f.Value(xb).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + iterations);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine(); 
            }

            return xb;
        }

        private double[] Explore(Function f, double[] xp, double[] dx)
        {
            double[] x = xp.Copy();

            for (int i = 0; i < x.Length; i++)
            {
                double p = f.Value(x);
                x[i] = x[i] + dx[i];
                double n = f.Value(x);
                if (n > p)
                {
                    x[i] = x[i] - 2*dx[i];
                    n = f.Value(x);
                    if (n > p)
                    {
                        x[i] = x[i] + dx[i];
                    }
                }
            }

            return x;
        }

        private void LogIteration(int iterations, double[] xb, double[] xp, double[] xn, double fxb, double fxp, double fxn, double[] dx)
        {
            var precision = "F" + PRECISION;
            Console.Write("[{0,3:D3}]", iterations);
            ConsoleEx.Write(" xb = " + xb.Format(PRECISION).PadRight(PRECISION + 4));
            ConsoleEx.Write(" xp = " + xp.Format(PRECISION).PadRight(PRECISION + 4));
            ConsoleEx.Write(" xn = " + xn.Format(PRECISION).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(xb) = " + fxb.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(xp) = " + fxp.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(xn) = " + fxn.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" dx = " + dx.Format(PRECISION).PadRight(PRECISION + 4));
            ConsoleEx.WriteLine();
        }
    }
}
