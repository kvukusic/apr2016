using System;

namespace APR.DZ4
{
    public abstract class BoundaryMutationOperator<T> : IMutationOperator<T> where T : IChromosome
    {
        public abstract T Execute(T chromosome);
    }
}