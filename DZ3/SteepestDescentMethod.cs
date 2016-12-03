using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3
{
    public class SteepestDescentMethod : IMinimizer
    {
        public bool IsOutputEnabled { get; set; }

        public double[] Minimize(Function f, double[] start)
        {
            throw new NotImplementedException();
        }
    }
}