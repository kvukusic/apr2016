using System;
using APR.DZ1.Extensions;
using APR.DZ2;
using APR.DZ2.Functions;
using System.Linq;

namespace APR.DZ3
{
    public class SteepestDescentMinimizer : IMinimizer
    {
        private static readonly int PRECISION = 6;
        private static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private static readonly double ALPHA = 1; // OR adaptive step size

        /// <summary>
        /// Number of allowed diverging iterations before the algorithm will terminate.
        /// </summary>
        private static readonly int MAX_DIV_ITER = 100;

        /// <summary>
        /// Maximum number of algorithm iterations.
        /// </summary>
        private static readonly int MAX_ITER = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="SteepestDescentMinimizer"/> class.
        /// </summary>
        public SteepestDescentMinimizer()
        {
            IsOutputEnabled = true;
            IsOutputPerIterationEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the step size (alpha) should be optimized using
        /// the <see cref="GoldenSectionSearch"/> algorithm.
        /// </summary>
        public bool OptimizeStepSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether values of every iteration
        /// should be output.
        /// </summary>
        public bool IsOutputPerIterationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating this algorithm produces any output.
        /// </summary>
        public bool IsOutputEnabled { get; set; }

        public double[] Minimize(Function function, double[] start)
        {
            function.Clear();

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Steepest Descent method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("OptimizeStepSize = " + OptimizeStepSize);
                ConsoleEx.WriteLine("e = " + EPSILON.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            var currentPoint = start.Copy();
            var currentGradient = function.Gradient(currentPoint);
            var bestValue = function.Value(currentPoint);
            var bestValuePoint = currentPoint;

            var divIter = 0;
            var iteration = 0;

            while (divIter < MAX_DIV_ITER && currentGradient.Norm() > EPSILON)
            {
                iteration++;

                var stepSize = OptimizeStepSize ? CalculateOptimalStepSize(function, currentPoint, currentGradient): ALPHA;
                currentPoint = currentPoint.Select((d, i) => d - stepSize*currentGradient[i]).ToArray();

                var newBestValue = function.Value(currentPoint);
                if (newBestValue < bestValue)
                {
                    divIter = 0;
                    bestValue = newBestValue;
                    bestValuePoint = currentPoint;
                }
                else
                {
                    divIter++;
                }

                if(IsOutputPerIterationEnabled)
                {
                    LogIteration(iteration, currentPoint, currentGradient, newBestValue);
                }

                currentGradient = function.Gradient(currentPoint);
            }

            if (IsOutputEnabled)
            {
                if (IsOutputPerIterationEnabled)
                {
                    ConsoleEx.WriteLine();
                }

                if (divIter != MAX_DIV_ITER)
                { // Converging
                    ConsoleEx.WriteLineGreen("Minimum found at " + bestValuePoint.Format(PRECISION) + " => " +
                                      bestValue.ToString("F" + PRECISION));
                    ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);                                      
                    ConsoleEx.WriteLine("Number of function evaluations: " + function.Evaluations);
                    ConsoleEx.WriteLine("Number of gradient evaluations: " + function.GradientEvaluations);
                }
                else
                { // Diverging
                    ConsoleEx.WriteLineRed("The algorithm has diverged.");
                    ConsoleEx.WriteLine("The best value is at " + bestValuePoint.Format(PRECISION) + " => " +
                                      bestValue.ToString("F" + PRECISION));
                    ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);                                      
                    ConsoleEx.WriteLine("Number of function evaluations: " + function.Evaluations);
                    ConsoleEx.WriteLine("Number of gradient evaluations: " + function.GradientEvaluations);
                }

                ConsoleEx.WriteLine();
            }

            return bestValuePoint;
        }

        private void LogIteration(int iteration, double[] currentPoint, double[] currentGradient, double fx)
        {
            Console.Write("[{0,3:D3}]", iteration);
            ConsoleEx.Write(" x = " + currentPoint.Format(PRECISION));
            ConsoleEx.Write(" dF = " + currentGradient.Format(PRECISION));
            ConsoleEx.Write(" f(x) = " + fx.ToString("F" + PRECISION));
            ConsoleEx.WriteLine();
        }

        private double CalculateOptimalStepSize(Function f, double[] currentPoint, double[] currentGradient)
        {
            var g = new DelegateFunction(alpha =>
            {
                return f.Value(currentPoint.Select((d, i) => d - alpha[0]*currentGradient[i]).ToArray());
            });

            var minimizer = new GoldenSectionMinimizer();
            minimizer.IsOutputEnabled = false;
            return minimizer.Minimize(g, 0);
        }
    }
}