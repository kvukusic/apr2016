using System;
using APR.DZ1.Extensions;
using APR.DZ2.Functions;

namespace APR.DZ4
{
    public class GAFunction
    {
        public struct Constraint
        {
            public double Min { get; }
            public double Max { get; }

            public Constraint(double min, double max)
            {
                Min = min;
                Max = max;
            }
        }

        private readonly Function _function;
        private readonly int _dimension;
        private readonly Constraint[] _constraints;

        /// <summary>
        /// Initializes a new instance of the <see cref="GAFunction"/> class.
        /// </summary>
        public GAFunction(Function function, int dimension)
        {
            CheckArguments(function, dimension);
            var constraints = new Constraint[dimension];
            for (int i = 0; i < dimension; i++)
            {
                constraints[i] = new Constraint(double.MinValue, double.MaxValue);
            }

            _function = function;
            _dimension = dimension;
            _constraints = constraints;

            // Clear function
            _function.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GAFunction"/> class.
        /// </summary>
        public GAFunction(Function function, int dimension, Constraint uniformConstraint)
        {
            CheckArguments(function, dimension);
            
            var constraints = new Constraint[dimension];
            for (int i = 0; i < dimension; i++)
            {
                constraints[i] = uniformConstraint;
            }

            _function = function;
            _dimension = dimension;
            _constraints = constraints;

            // Clear function
            _function.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GAFunction"/> class.
        /// </summary>
        public GAFunction(Function function, int dimension, Constraint[] constraints)
        {
            CheckArguments(function, dimension);

            if (constraints.Length != dimension)
            {
                throw new ArgumentException("Constraints not specified for every variable.");
            }

            _function = function;
            _dimension = dimension;
            _constraints = constraints.Copy();

            // Clear function
            _function.Clear();
        }

        private void CheckArguments(Function function, int dimension)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (dimension <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }

        }

        /// <summary>
        /// Gets the dimension of the constraint function.
        /// </summary>
        public int Dimension { get { return _dimension; } }

        /// <summary>
        /// Gets the current number of function evaluations.
        /// </summary>
        public int Evaluations { get { return _function.Evaluations; } }

        /// <summary>
        /// Calculates the fitness value with the given parameters of the
        /// function in this constraint.
        /// </summary>
        /// <param name="x">The function arguments.</param>
        /// <returns>The function fitness value.</returns>
        public double Fitness(params double[] x)
        {
            return _function.Value(x.Copy());
        }

        /// <summary>
        /// Returns the constraints of the given dimension index.
        /// </summary>
        /// <param name="index">The dimension.</param>
        /// <returns>The constraint.</returns>
        public Constraint ConstraintAt(int index)
        {
            if (index < 0 || index >= _dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _constraints[index];
        }

        public Constraint this[int index] => ConstraintAt(index);
    }
}