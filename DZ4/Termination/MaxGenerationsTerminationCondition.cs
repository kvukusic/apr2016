using System;

namespace APR.DZ4
{
    public class MaxGenerationsTerminationCondition : ITerminationCondition
    {
        public static readonly int DefaultValue = 10000;

        public MaxGenerationsTerminationCondition() : this(DefaultValue)
        {
        }

        public MaxGenerationsTerminationCondition(int maxGenerations)
        {
            MaxGenerations = maxGenerations;
        }

        public int MaxGenerations { get; private set; }

        public string Description
        {
            get { return "Max Generations"; }
        }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            return geneticAlgorithm.CurrentGeneration >= MaxGenerations;
        }
    }
}