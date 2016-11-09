using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ1.Extensions;

namespace APR.DZ2
{
    public class NelderMeadSearch : IMinimizer
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private readonly int MAX_ITERATIONS = 100000;
        private readonly double ALPHA = 1.0;
        private readonly double BETA = 0.5;
        private readonly double GAMMA = 2.0;
        private readonly double SIGMA = 0.5;
        
        private double SIMPLEX_OFFSET = 1.0;

        public NelderMeadSearch()
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

        public double[] Minimize(Function function, double[] start)
        {
            return Minimize(function, start, SIMPLEX_OFFSET);
        }

        public double[] Minimize(Function function, double[] start, double offset)
        {
            return Minimize(function, start, offset, EPSILON);
        }

        public double[] Minimize(Function function, double[] start, double offset, double eps)
        {
            return Minimize(function, start, offset, eps, ALPHA, BETA, GAMMA, SIGMA);
        }

        public double[] Minimize(Function function, double[] start, double offset, double eps, 
                                double alpha, double beta, double gamma, double sigma)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (start.Length == 0)
            {
                throw new ArgumentException("Dimension of given starting point is invalid");
            }

            // Clear function
            function.Clear();

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Nelder Mead Simplex Search method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine("eps = " + eps.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("alpha = " + alpha.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("beta = " + beta.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("gamma = " + gamma.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("sigma = " + sigma.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("t = " + offset.ToString("F" + PRECISION));
                ConsoleEx.WriteLine();
            }

            // Dimension
            int n = start.Length;

            // Create the initial simplex
            double[][] v = CalculateInitialSimplex(start.Copy(), offset);

            // Evaluate function at each simplex point
            double[] f = v.Select(xi => function.Value(xi)).ToArray();

            // Print out initial simplex
            if (IsOutputEnabled)
            {
                // Print out the initial simplex points
                ConsoleEx.WriteLine("Initial simplex points:");
                for (int i = 0; i <= n; i++)
                {
                    ConsoleEx.Write($"x{i}={v[i].Format(PRECISION)}, ");
                    ConsoleEx.Write($"f(x{i})={function.Value(v[i]).ToString("F" + PRECISION)}");
                    ConsoleEx.WriteLine();
                }

                ConsoleEx.WriteLine();
            }

            int iteration = 0;

            // Begin the main loop of the minimization
            for (iteration = 1; iteration <= MAX_ITERATIONS; iteration++)
            {
                // Find the index of the highest value
                int h = FindHighestValueIndex(f);

                // Find the index of the lowest value
                int l = FindLowestValueIndex(f);

                // Calculate the centroid
                double[] xc = CalculateCetroid(v, h);

                // Calculate the reflection point
                double[] xr = Reflection(v, xc, h, alpha);
                double fxr = function.Value(xr);

                if(fxr < f[l])
                {
                    // Calculate the expansion point
                    double[] xe = Expansion(xc, xr, gamma);
                    double fxe = function.Value(xe);

                    if(fxe < fxr) /* < f[l] */
                    {
                        v[h] = xe;
                        f[h] = fxe;
                    }
                    else
                    {
                        v[h] = xr;
                        f[h] = fxr;
                    }
                }
                else
                {

                    // if(fxr > fxi) for every i != h
                    if(f.Where((fxi, i) => (i != h) && fxr >= fxi).Count() == n) // fxr > fxi
                    {
                        if(fxr < f[h])
                        {
                            v[h] = xr;
                            f[h] = fxr;
                        }

                        double[] xk = Contraction(v, xc, h, beta);
                        double fxk = function.Value(xk);

                        if(fxk < f[h])
                        {
                            v[h] = xk;
                            f[h] = fxk;
                        }
                        else
                        {
                            // Shrink all but the best (xl) simplex points 
                            // xi = SIGMA * (xl + xi)
                            for (int i = 0; i <= n; i++)
                            {
                                if(i != l)
                                {
                                    for (int j = 0; j < n; j++)
                                    {
                                        v[i][j] = sigma * (v[l][j] + v[i][j]);
                                    }
                                }
                            }

                            // Evaluate function at each simplex point
                            f = v.Select(xi => function.Value(xi)).ToArray();
                        }
                    }
                    else
                    {
                        v[h] = xr;
                        f[h] = fxr;
                    }
                }

                // Print out the value at each iteration
                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    function.DisableStatistics();
                    LogIteration(iteration, xc, function.Value(xc));
                    function.EnableStatistcs();
                }

                // Test for convergence
                if(HasConverged(v, f, eps)) break;
            }
            
            // Find the index of the smallest value
            double[] xmin = v[FindLowestValueIndex(f)];

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            if (IsOutputEnabled)
            {
                var evaluations = function.Evaluations;
                var cachedCalls = function.CachedCalls;
                ConsoleEx.WriteLineGreen("Final position found. Returning value: x = " + xmin.Format(PRECISION));
                if(iteration == MAX_ITERATIONS) ConsoleEx.WriteLineRed($"Maximum number of iterations were hit [{MAX_ITERATIONS}]");
                ConsoleEx.WriteLineGreen("Function value of final position is: " + function.Value(xmin).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine();
            }

            return xmin;
        }

        private int FindHighestValueIndex(double[] values)
        {
            int h = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > values[h])
                {
                    h = i;
                }
            }
            return h;
        }

        private int FindLowestValueIndex(double[] values)
        {
            int l = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < values[l])
                {
                    l = i;
                }
            }
            return l;
        }

        private double[][] CalculateInitialSimplex(double[] start, double offset)
        {
            int n = start.Length;
            double pn = offset*(Math.Sqrt(n + 1) - 1 + n)/(n*Math.Sqrt(2));
            double qn = offset*(Math.Sqrt(n + 1) - 1)/(n*Math.Sqrt(2));

            double[][] simplex = new double[n + 1][];

            simplex[0] = start;
            for (int i = 1; i <= n; i++)
            {
                simplex[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    if (i - 1 == j)
                    {
                        simplex[i][j] = pn + start[j];
                    }
                    else
                    {
                        simplex[i][j] = qn + start[j];
                    }
                }
            }

            return simplex;
        }

        private double[] CalculateCetroid(double[][] v, int h)
        {
            int n = v.Length - 1;

            // xc = 1/n*(xj), j != h
            double[] xc = new double[n];
            for (int i = 0; i < n; i++)
            {
                double cent = 0.0;
                for (int j = 0; j <= n; j++)
                {
                    if (j != h)
                    {
                        cent += v[j][i];
                    }
                }
                xc[i] = cent / n;
            }

            return xc;
        }

        private double[] Reflection(double[][] v, double[] xc, int h, double alpha)
        {
            int n = v.Length - 1;

            // xr = (1+ALPHA)*xc - ALPHA*(xh)
            double[] xr = new double[n];
            for (int i = 0; i < n; i++)
            {
                xr[i] = (1 + alpha)*xc[i] - alpha*v[h][i];
            }

            return xr;
        }

        private double[] Expansion(double[] xc, double[] xr, double gamma)
        {
            int n = xc.Length;

            // xe = (1-GAMMA)*xc + GAMMA*xr
            double[] xe = new double[n];
            for (int i = 0; i < n; i++)
            {
                xe[i] = (1 - gamma) * xc[i] + gamma * xr[i];
            }

            return xe;
        }

        private double[] Contraction(double[][] v, double[] xc, int h, double beta)
        {
            int n = v.Length - 1;

            // xk = (1-BETA)*xc + BETA*xh
            double[] xk = new double[n];
            for (int i = 0; i < n; i++)
            {
                xk[i] = (1 - beta)*xc[i] + beta*v[h][i];
            }

            return xk;
        }

        private bool HasConverged(double[][] v, double[] f, double eps)
        {
            int n = v.Length - 1;

            double sum = 0.0;
            for (int i = 0; i <= n; i++)
            {
                sum += f[i];
            }
            double favg = sum / (n + 1);
            double s = 0.0;
            for (int j = 0; j <= n; j++)
            {
                s += Math.Pow((f[j] - favg), 2.0) / n;
            }
            s = Math.Sqrt(s);
            if (s < EPSILON) return true;

            return false;
        }

        private void LogIteration(int iteration, double[] xc, double fxc)
        {
            var precision = "F" + PRECISION;
            Console.Write("[{0,3:D3}]", iteration);
            ConsoleEx.Write(" xc = " + xc.Format(PRECISION).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(xc) = " + fxc.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.WriteLine();
        }
    }
}
