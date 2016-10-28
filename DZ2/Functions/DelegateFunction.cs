using System;

namespace APR.DZ2.Functions
{
    public class DelegateFunction : Function
    {
        private readonly Func<double[], double> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateFunction"/> class.
        /// </summary>
        public DelegateFunction(Func<double[], double> function)
        {
            _function = function;
        }

        protected override double ValueEx(params double[] x)
        {
            return _function(x);
        }
    }
}
