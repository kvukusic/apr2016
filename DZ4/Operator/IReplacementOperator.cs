using System;
using System.Collections.Generic;

namespace APR.DZ4
{
    public interface IReplacementOperator<T> where T : IChromosome
    {
        IList<T> Execute(IList<T> population, IList<T> offspringPopulation);
    }
}