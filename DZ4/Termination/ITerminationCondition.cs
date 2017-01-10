using System;

namespace APR.DZ4
{
    public interface ITerminationCondition
    {
        string Description { get; }
        
        bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome;
    }
}