using System;
using System.Collections.Generic;
using APR.DZ2.Functions;

namespace APR.DZ2
{
    public class GoldenSectionSearch
    {
        private const int OutputPrecision = 10;

        private readonly double EPSILON = 10e-6;
        private readonly double UNIMODAL_OFFSET = 1.0;
        private readonly double GOLDEN_RATIO = (Math.Sqrt(5.0) - 1) / 2.0;

        public GoldenSectionSearch()
        {
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
            return Minimize(f, startingInterval);
        }

        public double Minimize(Function f, Interval startingInterval)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            // Clear f
            f.Clear();

            if (IsOutputEnabled)
            {
                Console.WriteLine();
                Console.WriteLine("**********************************************************");
                Console.WriteLine("Starting minimization with Golden Section Search method...");

                Console.WriteLine();
                Console.WriteLine("Parameters: ");
                Console.WriteLine("EPSILON = " + EPSILON);
                Console.WriteLine("K = " + GOLDEN_RATIO);
                Console.WriteLine("Start interval = " + startingInterval.ToString(6));
                Console.WriteLine();
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
                LogIteration(iterations, lowerBound, upperBound, c, d, fc, fd);
            }

            while(upperBound - lowerBound > EPSILON)
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
                    LogIteration(iterations, lowerBound, upperBound, c, d, fc, fd);
                }
            }

            var finalPosition = FinalPosition(lowerBound, upperBound);
            var cachedCalls = f.CachedCalls;
            var evaluations = f.Evaluations;

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                Console.WriteLine();
            }

            if (IsOutputEnabled)
            {
                Console.WriteLine("Final position found. Returning value: " + finalPosition.ToString("F" + OutputPrecision));
                Console.WriteLine("Function value of final position is: " + f.Value(finalPosition).ToString("F" + OutputPrecision));
                Console.WriteLine("Number of algorithm iterations: " + iterations);
                Console.WriteLine("Number of function cached calls: " + cachedCalls);
                Console.WriteLine("Number of function evaluations: " + evaluations);
                Console.WriteLine(); 
            }

            return finalPosition;
        }

        private double FinalPosition(double lower, double upper)
        {
            return (upper + lower) / 2.0;
        }

        private void LogIteration(int iteration, double a, double b, double c, double d, double fc, double fd)
        {
            var precision = "F" + OutputPrecision;
            Console.Write("[{0,3:D3}]", iteration);
            Console.Write(" a = " + a.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" b = " + b.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" c = " + c.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" d = " + d.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" f(c) = " + fc.ToString(precision).PadRight(OutputPrecision + 4));
            Console.Write(" f(d) = " + fd.ToString(precision).PadRight(OutputPrecision + 4));
            Console.WriteLine();
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
