using System;
using System.Linq;
using APR.DZ1.Extensions;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3
{
    public class NewtonRaphsonMethod : IMinimizer
    {
        private static readonly int PRECISION = 6;
        private static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private static readonly double ALPHA = 1.0; // OR adaptive step size

        /// <summary>
        /// Number of allowed diverging iterations before the algorithm will terminate.
        /// </summary>
        private static readonly int MAX_DIV_ITER = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonRaphsonMethod"/> class.
        /// </summary>
        public NewtonRaphsonMethod()
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

        public double[] Minimize(Function f, double[] start)
        {
            f.Clear();

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Newton-Raphson method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("OptimizeStepSize = " + OptimizeStepSize);
                ConsoleEx.WriteLine("e = " + EPSILON.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine();
            }

            var currentPoint = start.Copy();
            var currentGradient = f.Gradient(currentPoint);
            var currentOffset = (f.Hessian(currentPoint).ToInverseMatrix() * currentGradient.ToVector()).GetColumnVector(0).ToArray();
            var currentOffsetNorm = currentOffset.Norm();
            var bestValue = f.Value(currentPoint);
            var bestValuePoint = currentPoint;

            var divIter = 0;
            var iteration = 0;

            while (divIter < MAX_DIV_ITER && currentOffsetNorm > EPSILON)
            {
                iteration++;

                var stepSize = OptimizeStepSize ? CalculateOptimalStepSize(f, currentPoint, currentOffset) : ALPHA;
                currentPoint = currentPoint.Select((d, i) => d - stepSize * currentOffset[i]).ToArray();
                currentGradient = f.Gradient(currentPoint);
                currentOffset = (f.Hessian(currentPoint).ToInverseMatrix() * currentGradient.ToVector()).GetColumnVector(0).ToArray();
                currentOffsetNorm = currentOffset.Norm();

                var newBestValue = f.Value(currentPoint);
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
            }

            if (IsOutputEnabled)
            {
                if (!IsOutputPerIterationEnabled)
                {
                    ConsoleEx.WriteLine();
                }

                if (divIter != MAX_DIV_ITER)
                { // Converging
                    ConsoleEx.WriteLineGreen("Minimum found at " + bestValuePoint.Format(PRECISION) + " => " +
                                      bestValue.ToString("F" + PRECISION));
                    ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);
                    ConsoleEx.WriteLine("Number of function evaluations: " + f.Evaluations);
                    ConsoleEx.WriteLine("Number of gradient evaluations: " + f.GradientEvaluations);
                    ConsoleEx.WriteLine("Number of Hessian evaluations: " + f.HessianEvaluations);
                }
                else
                { // Diverging
                    ConsoleEx.WriteLineRed("The algorithm has diverged from the minimum.");
                    ConsoleEx.WriteLine("The best value is at " + bestValuePoint.Format(PRECISION) + " => " +
                                      bestValue.ToString("F" + PRECISION));
                    ConsoleEx.WriteLine("Number of algorithm iterations: " + iteration);
                    ConsoleEx.WriteLine("Number of function evaluations: " + f.Evaluations);
                    ConsoleEx.WriteLine("Number of gradient evaluations: " + f.GradientEvaluations);
                    ConsoleEx.WriteLine("Number of Hessian evaluations: " + f.HessianEvaluations);
                }

                ConsoleEx.WriteLine();
            }

            return bestValuePoint;
        }

        private double CalculateOptimalStepSize(Function f, double[] currentPoint, double[] currentOffset)
        {
            var g = new DelegateFunction(alpha =>
            {
                return f.Value(currentPoint.Select((d, i) => d - alpha[0] * currentOffset[i]).ToArray());
            });

            var minimizer = new GoldenSectionSearch();
            minimizer.IsOutputEnabled = false;
            return minimizer.Minimize(g, 0);
        }
    }
}