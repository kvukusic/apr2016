using System;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ1.Extensions;

namespace APR.DZ2
{
    public class HookeJeevesSearch : IMinimizer
    {
        private const int OutputPrecision = 10;

        private static readonly double[] DEFAULT_DX = new double[] { 0.5, 0.5 };
        private static readonly double[] DEFAULT_E = new double[] {10e-6, 10e-6};

        private readonly double[] _dx;
        private readonly double[] _e;

        /// <summary>
        /// Initializes a new instance of the <see cref="HookeJeevesSearch"/> class.
        /// </summary>
        public HookeJeevesSearch() : this(DEFAULT_DX, DEFAULT_E)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HookeJeevesSearch"/> class.
        /// </summary>
        public HookeJeevesSearch(double[] dx, double[] e)
        {
            if (dx == null)
            {
                throw new ArgumentNullException(nameof(dx));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (dx.Length != e.Length)
            {
                throw new ArgumentException("Dimensions do not match.");
            }

            IsOutputPerIterationEnabled = true;
            IsOutputEnabled = true;

            _dx = dx;
            _e = e;
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
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            // Clear f
            f.Clear();

            if (IsOutputEnabled)
            {
                Console.WriteLine();
                Console.WriteLine("****************************************************************");
                Console.WriteLine("Starting minimization with Hooke-Jeeves Pattern Search method...");

                Console.WriteLine();
                Console.WriteLine("Parameters: ");
                Console.WriteLine("dx = " + _dx.Format(OutputPrecision));
                Console.WriteLine("e = " + _e.Format(OutputPrecision));
                Console.WriteLine("x0 = " + start.Format(OutputPrecision));
                Console.WriteLine();
            }

            int iterations = 0;

            double[] x0 = start.Copy();
            double[] xb = x0.Copy();
            double[] xp = x0.Copy();
            double[] dx = _dx.Copy();

            do
            {
                iterations++;

                double[] xn = Explore(f, xp, dx);
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

                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    LogIteration(iterations, xb, xp, xn, f.Value(xb), f.Value(xp), f.Value(xn), dx);
                }

            } while (dx.Where((t, i) => t > _e[i]).Any());

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                Console.WriteLine();
            }

            var evaluations = f.Evaluations;
            var cachedCalls = f.CachedCalls;

            if (IsOutputEnabled)
            {
                Console.WriteLine("Final position found. Returning value: x = " + xb.Format(OutputPrecision));
                Console.WriteLine("Function value of final position is: " + f.Value(xb).ToString("F" + OutputPrecision));
                Console.WriteLine("Number of algorithm iterations: " + iterations);
                Console.WriteLine("Number of function cached calls: " + cachedCalls);
                Console.WriteLine("Number of function evaluations: " + evaluations);
                Console.WriteLine(); 
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
            var precision = "F" + OutputPrecision;
            Console.Write("[{0,3:D3}]", iterations);
            Console.Write(" xb = " + xb.Format(OutputPrecision).PadRight(OutputPrecision + 4));
            Console.Write(" xp = " + xp.Format(OutputPrecision).PadRight(OutputPrecision + 4));
            Console.Write(" xn = " + xn.Format(OutputPrecision).PadRight(OutputPrecision + 4));
            Console.Write(" f(xb) = " + fxb.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" f(xp) = " + fxp.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" f(xn) = " + fxn.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" dx = " + dx.Format(OutputPrecision).PadRight(OutputPrecision + 4));
            Console.WriteLine();
        }
    }
}
