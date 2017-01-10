using System;

namespace APR.DZ4
{
    public class FitnessValueTerminationCondition : ITerminationCondition
    {
        private Func<double, bool> _predicate;

        public FitnessValueTerminationCondition(Func<double, bool> predicate)
        {
            _predicate = predicate;
        }

        public string Description
        {
            get { return "Fitness Value"; }
        }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            return _predicate(geneticAlgorithm.BestIndividual.Fitness);
        }
    }
}