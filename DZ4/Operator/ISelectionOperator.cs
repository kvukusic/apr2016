using System;
using System.Collections.Generic;

namespace APR.DZ4
{
    public interface ISelectionOperator<T> where T : IChromosome
    {
        T Execute(IList<T> population);
    }
}