using System;

namespace APR.DZ4
{
    public interface IChromosomeFactory<T> where T : IChromosome
    {
        T Create();
    }
}