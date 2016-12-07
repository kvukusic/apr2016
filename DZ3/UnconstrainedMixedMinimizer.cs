using System;
using System.Linq;
using System.Collections.Generic;
using APR.DZ1.Extensions;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3
{
    public class UnconstrainedMixedMinimizer : IMinimizer
    {
        private const int PRECISION = 6;

        private readonly int MAX_ITERATIONS = 1000;

        private readonly double EPSILON = Math.Pow(10, -PRECISION);
        private readonly double INITIAL_T = 1.0;

        private IDictionary<int, double[]> _explicitConstraints = new Dictionary<int, double[]>();
        private List<Constraint> _implicitConstraints = new List<Constraint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnconstrainedMixedMethod"/> class.
        /// </summary>
        public UnconstrainedMixedMinimizer()
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

        /// <summary>
        /// Gets or sets te minimizer which is used.
        /// </summary>
        public IMinimizer Minimizer { get; set; }

        public void AddImplicitConstraint(Constraint c)
        {
            _implicitConstraints.Add(c);
        }

        public double[] Minimize(Function f, double[] start)
        {
            f.Clear();

            if (IsOutputEnabled)
            {

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Unconstrained-Mixed optimization method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("initial_t = " + INITIAL_T.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("epsilon = " + EPSILON.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            Minimizer.IsOutputEnabled = false;

            var t = INITIAL_T;
            var x = start.Copy();

            while(!IsImplicitValid(x))
            {
                ConsoleEx.WriteLineRed("Starting point is infeasable. Searching for inner starting point...");

                x = Minimizer.Minimize(CreateFunctionForInnerStartingPoint(t), x);
                ConsoleEx.WriteLineRed("Found starting point is " + "x0 = " + x.Format(PRECISION));
                ConsoleEx.WriteLine();

                t = t*10;
            }

            var iteration = 0;
            t = INITIAL_T;

            while (iteration < MAX_ITERATIONS)
            {
                iteration++;

                var unconstrainedFunction = CreateFunctionToMinimize(f, t);
                var newX = Minimizer.Minimize(unconstrainedFunction, x);

                var found = true;
                for (int i = 0; i < x.Length; i++)
                {
                    if (Math.Abs(x[i] - newX[i]) > EPSILON)
                    {
                        found = false;
                    }
                }

                x = newX;

                if (found)
                {
                    break;
                }

                t *= 10;
            }

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            if (IsOutputEnabled)
            {
                var evaluations = f.Evaluations;
                var cachedCalls = f.CachedCalls;
                ConsoleEx.WriteLineGreen("Final position found. Returning value: x = " + x.Format(PRECISION));
                if(iteration == MAX_ITERATIONS) ConsoleEx.WriteLineRed($"Maximum number of iterations were hit [{MAX_ITERATIONS}]");
                ConsoleEx.WriteLineGreen("Function value of final position is: " + f.Value(x).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine();
            }

            return x;
        }

        private bool IsImplicitValid(double[] x)
        {
            foreach (Constraint c in _implicitConstraints.Where(constraint => !constraint.IsEqualityConstraint))
            {
                if (!c.IsValid(x))
                {
                    return false;
                }
            }
            
            return true;
        }

        private Function CreateFunctionForInnerStartingPoint(double t)
        {
            return new DelegateFunction(x =>
            {
                return -_implicitConstraints.Where(constraint => !constraint.IsEqualityConstraint)
                           .Sum(constraint => constraint.Value(x) < 0 ? t*constraint.Value(x) : 0);
            });
        }

        private Function CreateFunctionToMinimize(Function function, double t)
        {
            return new DelegateFunction(x =>
            {
                return function.Value(x) -
                       1/t*
                       _implicitConstraints.Where(constraint => !constraint.IsValid(x)).Where(constraint => !constraint.IsEqualityConstraint)
                           .Sum(constraint => constraint.Value(x) <= 0 ? Double.NegativeInfinity : Math.Log(constraint.Value(x))) +
                       t*
                       _implicitConstraints.Where(constraint => constraint.IsEqualityConstraint)
                           .Sum(constraint => Math.Pow(constraint.Value(x), 2));
            });
        }
    }
}
