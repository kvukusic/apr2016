using System;

namespace APR.DZ4
{
    public class TwoPointCrossoverOperator : MultiPointCrossoverOperator
    {
        public TwoPointCrossoverOperator(double crossoverProbability) 
            : base(crossoverProbability, 2)
        {
        }
    }
}