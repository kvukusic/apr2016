using System;

namespace APR.DZ4
{
    public class MaxEvaluationsTerminationCondition : ITerminationCondition
    {
        public static readonly int DefaultValue = 10000;

        public MaxEvaluationsTerminationCondition() : this(DefaultValue)
        {
        }

        public MaxEvaluationsTerminationCondition(int maxEvaluations)
        {
            MaxEvaluations = maxEvaluations;
        }

        public int MaxEvaluations { get; private set; }

        public string Description
        {
            get { return "Max Evaluations"; }
        }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            return geneticAlgorithm.FitnessEvaluations >= MaxEvaluations;
        }
    }
}