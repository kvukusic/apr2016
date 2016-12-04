using System;

namespace APR.DZ3
{
    public class Constraint
    {
        private bool _isEqualityConstraint;

        private readonly Func<double[], double> _constraintFunction;
        private readonly Func<double, bool> _validationFunction;

        public Constraint(
            Func<double[], double> constraintFunction,
            Func<double, bool> validationFunction,
            bool isEqualityConstraint = false)
        {
            _isEqualityConstraint = isEqualityConstraint;
            _constraintFunction = constraintFunction;
            _validationFunction = validationFunction;
        }

        public bool IsEqualityConstraint { get { return _isEqualityConstraint; } }

        public double Value(params double[] x)
        {
            return _constraintFunction(x);
        }

        public bool IsValid(params double[] x)
        {
            return _validationFunction(_constraintFunction(x));
        }
    }
}