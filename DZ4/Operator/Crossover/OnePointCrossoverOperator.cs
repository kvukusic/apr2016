using System;

namespace APR.DZ4
{
    public class OnePointCrossoverOperator : MultiPointCrossoverOperator
    {
        public OnePointCrossoverOperator(double crossoverProbability)
            : base(crossoverProbability, 1)
        {
        }
    }
}