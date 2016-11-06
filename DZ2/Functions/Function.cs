using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ1;
using APR.DZ1.Extensions;

namespace APR.DZ2.Functions
{
    public abstract class Function
    {
        private const double EPSILON = 1e-4;

        private Dictionary<double[], double> _cache =
            new Dictionary<double[], double>(new DoubleArrayStructuralEqualityComparer());

        private int _evaluations;
        private int _cachedCalls;
        private int _gradientEvaluations;
        private int _hessianEvaluations;
        private bool _statisticsEnabled = true;

        /// <summary>
        /// Gets the number of calls to this function that returned results that were not already
        /// evaluated.
        /// </summary>
        public int Evaluations { get { return _evaluations; } }

        /// <summary>
        /// Gets or sets the number of cached function calls.
        /// </summary>
        public int CachedCalls { get { return _cachedCalls; } }

        /// <summary>
        /// Gets the number of calls to the gradient of this function.
        /// </summary>
        public int GradientEvaluations { get { return _gradientEvaluations; } }

        /// <summary>
        /// Gets the number of calls to the hessian of this function.
        /// </summary>
        public int HessianEvaluations { get { return _hessianEvaluations; } }

        /// <summary>
        /// Clears the evaluations counter and the cache.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
            _evaluations = 0;
            _cachedCalls = 0;
            _gradientEvaluations = 0;
            _hessianEvaluations = 0;
        }

        /// <summary>
        /// Evaluates this function with the given parameters and returns the value.
        /// </summary>
        /// <param name="x">Function arguments.</param>
        /// <returns>The function value.</returns>
        public double Value(params double[] x)
        {
            var xCopy = x.Copy();
            if (_cache.ContainsKey(xCopy))
            {
                if(_statisticsEnabled) _cachedCalls++;
                return _cache[x];
            }

            var value = ValueEx(xCopy);
            _cache.Add(xCopy, value);
            if(_statisticsEnabled) _evaluations++;
            return value;
        }

        /// <summary>
        /// Evaluates the gradient of this function with the given parameters and returns the value.
        /// </summary>
        /// <param name="x">Function arguments.</param>
        /// <returns>The function value.</returns>
        public double[] Gradient(params double[] x)
        {
            if(_statisticsEnabled) _gradientEvaluations++;

            var xCopy = x.Copy();

            var grad = GradientEx(x);
            if(grad != null)
            {
                return grad;
            }

            return xCopy.Select((d, i) => PartialDerivative(i, xCopy)).ToArray();
        }

        /// <summary>
        /// Calculates the partial derivative of the <param name="index"></param> variable at the
        /// given point.
        /// </summary>
        /// <param name="index">The index of the variable whose partial derivation is calculated.</param>
        /// <param name="x">The partial derivative function parameters.</param>
        /// <returns>The partial derivation at <param name="x"></param>.</returns>
        public double PartialDerivative(int index, params double[] x)
        {
            var xCopy1 = x.Copy();
            var xCopy2 = x.Copy();

            xCopy1[index] += EPSILON;
            var f_plus = Value(xCopy1);

            xCopy2[index] -= EPSILON;
            var f_minus = Value(xCopy2);

            return (f_plus - f_minus)/(2*EPSILON);
        }

        /// <summary>
        /// Calculates the 2nd-partial derivative of the <param name="index1"></param> and <param name="index2"></param>
        /// variables at the given point.
        /// <para>
        /// Order of calculation is <c>partial(partial(index2), index1)</c>.
        /// </para>
        /// </summary>
        /// <param name="index1">The index of the first variable.</param>
        /// <param name="index2">The index of the second variable.</param>
        /// <param name="x">The function arguments.</param>
        /// <returns>The second partial derivative of this function at the given <param name="x"></param>.</returns>
        public double SecondPartialDerivative(int index1, int index2, params double[] x)
        {
            var xCopy1 = x.Copy();
            var xCopy2 = x.Copy();

            xCopy1[index1] += EPSILON;
            var f_plus = PartialDerivative(index2, xCopy1);

            xCopy2[index1] -= EPSILON;
            var f_minus = PartialDerivative(index2, xCopy2);

            return (f_plus - f_minus)/(2*EPSILON);
        }

        /// <summary>
        /// Returns the <c>Hessian</c> matrix of this function.
        /// </summary>
        /// <param name="x">The function arguments. Length of this array defines the dimension of the matrix.</param>
        /// <returns>The <c>Hessian</c> matrix.</returns>
        public Matrix Hessian(params double[] x)
        {
            if(_statisticsEnabled) _hessianEvaluations++;

            var hessian = HessianEx(x);
            if(hessian != null)
            {
                return hessian;
            }

            var result = new Matrix(x.Length, x.Length);

            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    result[i][j] = SecondPartialDerivative(i, j, x);
                }
            }

            return result;
        }

        protected abstract double ValueEx(params double[] x);

        protected double[] GradientEx(params double[] x)
        {
            return null;
        }

        protected Matrix HessianEx(params double[] x)
        {
            return null;
        }

        public void EnableStatistcs()
        {
            _statisticsEnabled = true;
        }

        public void DisableStatistics()
        {
            _statisticsEnabled = false;
        }

        class DoubleArrayStructuralEqualityComparer : EqualityComparer<double[]>
        {
            public override bool Equals(double[] x, double[] y)
            {
                return System.Collections.StructuralComparisons.StructuralEqualityComparer
                    .Equals(x, y);
            }

            public override int GetHashCode(double[] obj)
            {
                return System.Collections.StructuralComparisons.StructuralEqualityComparer
                    .GetHashCode(obj);
            }
        }
    }
}