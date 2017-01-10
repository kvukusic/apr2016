using System;

namespace APR.DZ4
{
    public class EvolutionTimeTerminationCondition : ITerminationCondition
    {
        public static readonly TimeSpan DefaultValue = TimeSpan.FromMinutes(1);

        public EvolutionTimeTerminationCondition() : this(DefaultValue)
        {
        }

        public EvolutionTimeTerminationCondition(TimeSpan maxTime)
        {
            if (maxTime == null)
            {
                throw new ArgumentNullException(nameof(maxTime));
            }

            MaxTime = maxTime;
        }

        public string Description
        {
            get { return "Elapsed Time"; }
        }

        public TimeSpan MaxTime { get; private set; }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            return geneticAlgorithm.EvolutionTime > MaxTime;
        }
    }
}