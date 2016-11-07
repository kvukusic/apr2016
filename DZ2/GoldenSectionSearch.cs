using System;
using APR.DZ2.Functions;

namespace APR.DZ2
{
    public class GoldenSectionSearch
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private static readonly double UNIMODAL_OFFSET = 1.0;
        private static readonly double GOLDEN_RATIO = (Math.Sqrt(5.0) - 1) / 2.0;

        public GoldenSectionSearch()
        {
            IsOutputEnabled = true;
            IsOutputPerIterationEnabled = true;
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

        public double Minimize(Function f, double start)
        {
            var startingInterval = FindUnimodal(f, UNIMODAL_OFFSET, start);
            return Minimize(f, startingInterval, EPSILON);
        }

        public double Minimize(Function f, double start, double eps)
        {
            var startingInterval = FindUnimodal(f, UNIMODAL_OFFSET, start);
            return Minimize(f, startingInterval, eps);
        }

        public double Minimize(Function f, Interval startingInterval)
        {
            return Minimize(f, startingInterval, EPSILON);
        }

        public double Minimize(Function f, Interval startingInterval, double eps)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            if(eps < 0)
            {
                throw new ArgumentException("eps is less than zero");
            }

            // Clear f
            f.Clear();

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("**********************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Golden Section Search method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("EPSILON = " + eps);
                ConsoleEx.WriteLine("K = " + GOLDEN_RATIO);
                ConsoleEx.WriteLine("Start interval = " + startingInterval.ToString(6));
                ConsoleEx.WriteLine();
            }

            var lowerBound = startingInterval.Lower;
            var upperBound = startingInterval.Upper;

            int iterations = 1;

            double c = upperBound - GOLDEN_RATIO * (upperBound - lowerBound);
            double d = lowerBound + GOLDEN_RATIO * (upperBound - lowerBound);
            double fc = f.Value(c);
            double fd = f.Value(d);

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                LogIteration(iterations, lowerBound, upperBound, c, d, f.Value(lowerBound), f.Value(upperBound), fc, fd);
            }

            while(upperBound - lowerBound > eps)
            {
                if(fc < fd)
                {
                    upperBound = d;
                    d = c;
                    c = upperBound - GOLDEN_RATIO * (upperBound - lowerBound);
                    fd = fc;
                    fc = f.Value(c);
                }
                else {
                    lowerBound = c;
                    c = d;
                    d = lowerBound + GOLDEN_RATIO * (upperBound - lowerBound);
                    fc = fd;
                    fd = f.Value(d);
                }

                iterations++;
                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    f.DisableStatistics();
                    LogIteration(iterations, lowerBound, upperBound, c, d, f.Value(lowerBound), f.Value(upperBound), fc, fd);
                    f.EnableStatistcs();
                }
            }

            var finalPosition = FinalPosition(lowerBound, upperBound);
            var cachedCalls = f.CachedCalls;
            var evaluations = f.Evaluations;

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            if (IsOutputEnabled)
            {
                ConsoleEx.WriteLineGreen("Final position found. Returning value: " + finalPosition.ToString("F" + PRECISION));
                ConsoleEx.WriteLineGreen("Function value of final position is: " + f.Value(finalPosition).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + iterations);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine(); 
            }

            return finalPosition;
        }

        private double FinalPosition(double lower, double upper)
        {
            return (upper + lower) / 2.0;
        }

        private void LogIteration(int iteration, double a, double b, double c, double d, double fa, double fb, double fc, double fd)
        {
            var precision = "F" + PRECISION;
            Console.Write("[{0,3:D3}]", iteration);
            ConsoleEx.Write(" a = " + a.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" c = " + c.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" d = " + d.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" b = " + b.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(a) = " + fa.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(c) = " + fc.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(d) = " + fd.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.Write(" f(b) = " + fb.ToString(precision).PadRight(PRECISION + 4));
            ConsoleEx.WriteLine();
        }

        private Interval FindUnimodal(Function f, double h, double x)
        {
            double l = x - h, r = x + h;
            double m = x;
            uint step = 1;

            double fm = f.Value(x);
            double fl = f.Value(l);
            double fr = f.Value(r);

            if (fm < fr && fm < fl)
            {
                return new Interval(l, r);
            }

            if (fm > fr)
            {
                do
                {
                    l = m;
                    m = r;
                    fm = fr;
                    r = x + h * (step *= 2);
                    fr = f.Value(r);
                } while (fm > fr);
            }
            else
            {
                do
                {
                    r = m;
                    m = l;
                    fm = fl;
                    l = x - h * (step *= 2);
                    fl = f.Value(l);
                } while (fm > fl);
            }

            return new Interval(l, r);
        }

        public sealed class Interval
        {
            public double Lower { get; }
            public double Upper { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Interval"/> class.
            /// </summary>
            public Interval(double lower, double upper)
            {
                if (upper < lower)
                {
                    throw new ArgumentOutOfRangeException(nameof(upper));
                }

                Lower = lower;
                Upper = upper;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return "[" + Lower + ", " + Upper + "]";
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public string ToString(int precision)
            {
                return "[" + Lower.ToString("F" + precision) + ", " + Upper.ToString("F" + precision) + "]";
            }
        }
    }
}
