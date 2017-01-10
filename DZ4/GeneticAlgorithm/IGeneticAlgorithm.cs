using System;

namespace APR.DZ4
{
    public interface IGeneticAlgorithm<TResult>
    {
        void Run();

        TResult BestIndividual { get; }

        int CurrentGeneration { get; }

        int FitnessEvaluations { get; }

        TimeSpan EvolutionTime { get; }

        event EventHandler<GeneticAlgorithmStateChangedEventArgs<TResult>> StateChanged;

        event EventHandler<GeneticAlgorithmTerminatedEventArgs> Terminated;
    }
}