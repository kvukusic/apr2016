using System;

namespace APR.DZ4
{
    public interface IMutationOperator<T> where T : IChromosome
    {
        T Execute(T chromosome);
    }
}