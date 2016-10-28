using System;
using APR.DZ2.Functions;

namespace APR.DZ2
{
    public class CoordinateDescent : IMinimizer
    {
        public bool IsOutputEnabled { get; set; }

        public double[] Minimize(Function f, double[] start)
        {
            throw new NotImplementedException();
        }
    }
}