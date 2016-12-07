using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ1.Extensions;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3
{
    public class BoxMinimizer : IMinimizer
    {
        private const int PRECISION = 6;

        private readonly int MAX_ITERATIONS = 10000;

        private readonly double EPSILON = Math.Pow(10, -PRECISION);
        private readonly double ALPHA = 1.3;

        private readonly Random _random;

        private IDictionary<int, double[]> _explicitConstraints = new Dictionary<int, double[]>();
        private List<Constraint> _implicitConstraints = new List<Constraint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxMinimizer"/> class.
        /// </summary>
        public BoxMinimizer()
        {
            _random = new Random();

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

        public void AddExplicitConstraint(int dimension, double min, double max)
        {
            if (_explicitConstraints.ContainsKey(dimension))
            {
                double[] t = _explicitConstraints[dimension];
                _explicitConstraints.Add(dimension, new double[]
                {
                    Math.Min(t[0], min), Math.Max(t[1], max)
                });
            }
            else
            {
                _explicitConstraints.Add(dimension, new double[] {min, max});
            }
        }

        public void AddImplicitConstraint(Constraint c)
        {
            _implicitConstraints.Add(c);
        }

        public void ClearExplicitConstraints()
        {
            _explicitConstraints.Clear();
        }

        public void ClearImplicitConstraints()
        {
            _implicitConstraints.Clear();
        }

        public double[] Minimize(Function function, double[] start)
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
                ConsoleEx.WriteLineGreen("Starting minimization with Box-Constrained optimization method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("e = " + EPSILON.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("alpha = " + ALPHA);
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            var xc = start.Copy();

            if (!ImplicitValid(xc)) { return xc; }
            foreach (var constraint in _explicitConstraints)
            {
                int i = constraint.Key;
                double[] t = constraint.Value;
                if (xc[i] < t[0] || xc[i] > t[1]) { return xc; }
            }

            double[][] d = new double[2 * xc.Length][];
            for (int i = 0; i < d.Length; i++)
            {
                d[i] = new double[xc.Length];
            }
            double[] evals = new double[d.Length];
            int max = 0;
            int secondMax = 0;

            for (int i = 0; i < d.Length; i++)
            {
                for (int j = 0; j < xc.Length; j++)
                {
                    if (_explicitConstraints.ContainsKey(j))
                    {
                        double[] temp = _explicitConstraints[j];
                        if (Double.IsInfinity(temp[0]) || Double.IsInfinity(temp[1]))
                        {
                            d[i][j] = _random.Next();
                        }
                        else {
                            d[i][j] = temp[0] + _random.NextDouble() * (temp[1] - temp[0]);
                        }
                    }
                    else { d[i][j] = _random.Next(); }
                }

                while (!ImplicitValid(d[i]))
                {
                    for (int j = 0; j < xc.Length; j++)
                    {
                        d[i][j] = (d[i][j] + xc[j]) / 2;
                    }
                }
                evals[i] = function.Value(d[i]);
                for (int k = 0; k < xc.Length; k++)
                {
                    xc[k] *= i;
                    xc[k] += d[i][k];
                    xc[k] /= i + 1;
                }
            }

            if (IsOutputEnabled)
            {
                // Print out the initial simplex points
                ConsoleEx.WriteLine("Initial simplex points:");
                for (int i = 0; i < d.Length; i++)
                {
                    ConsoleEx.Write($"x{i}={d[i].Format(PRECISION)}, ");
                    ConsoleEx.Write($"f(x{i})={function.Value(d[i]).ToString("F" + PRECISION)}");
                    ConsoleEx.WriteLine();
                }

                ConsoleEx.WriteLine();
            }

            int iteration = 0;

            while (!IsEnd(function.Value(xc), d, function) && iteration < MAX_ITERATIONS)
            {
                iteration++;
                max = 0;
                secondMax = -1;
                for (int i = 1; i < d.Length; i++)
                {
                    if (evals[i] > evals[max])
                    {
                        secondMax = max;
                        max = i;
                    }
                    else if ((secondMax == -1 || evals[i] > evals[secondMax]) && i != max)
                    {
                        secondMax = i;
                    }
                }
                double[] xr = d[max];
                for (int k = 0; k < xc.Length; k++)
                {
                    xc[k] = 0;
                    for (int j = 0; j < d.Length; j++)
                    {
                        if (j == max) { continue; }
                        xc[k] += d[j][k];
                    }
                    xc[k] /= d.Length - 1;
                    xr[k] = (1 + ALPHA) * xc[k] - ALPHA * xr[k];
                }

                foreach (var constraint in _explicitConstraints)
                {
                    int i = constraint.Key;
                    double[] t = constraint.Value;
                    xr[i] = Math.Min(Math.Max(xr[i], t[0]), t[1]);
                }

                while (!ImplicitValid(xr))
                {
                    for (int i = 0; i < xr.Length; i++)
                    {
                        xr[i] = (xr[i] + xc[i]) / 2;
                    }
                }
                evals[max] = function.Value(xr);
                if (evals[max] > evals[secondMax])
                {
                    for (int i = 0; i < xr.Length; i++)
                    {
                        xr[i] = (xr[i] + xc[i]) / 2;
                    }
                    evals[max] = function.Value(xr);
                }
            }

            int min = 0;
            for (int i = 0; i < d.Length; i++)
            {
                if (evals[min] < evals[i]) { min = i; }
            }

            double[] xmin = d[min];

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

        private bool IsEnd(double xcV, double[][] points, Function f)
        {
            double t = 0;
            for (int i = 0; i < points.Length; i++)
            {
                double temp = (f.Value(points[i]) - xcV);
                t += temp * temp;
            }
            return Math.Sqrt(t / points.Length) <= EPSILON;
        }

        private bool ImplicitValid(double[] p)
        {
            foreach (Constraint c in _implicitConstraints)
            {
                if (!c.IsValid(p))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}