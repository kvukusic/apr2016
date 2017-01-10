using System;

namespace APR.DZ4
{
    public interface IGeneticAlgorithmState
    {
        int Generation { get; }
        int Evaluations { get; }
        TimeSpan ElapsedTime { get; }
    }

    public interface IGeneticAlgorithmState<T> : IGeneticAlgorithmState
    {
        T BestIndividual { get; }
    }
}