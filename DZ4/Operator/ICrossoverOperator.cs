using System;
using System.Collections.Generic;

namespace APR.DZ4
{
    public interface ICrossoverOperator<T> where T : IChromosome
    {
        int NumberOfParents { get; }

        IList<T> Execute(IList<T> parents);
    }
}