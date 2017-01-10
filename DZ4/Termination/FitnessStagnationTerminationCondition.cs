using System;

namespace APR.DZ4
{
    /// <summary>
    /// Stopping criteria for a genetic algorithm.
    /// 
    /// The condition has been reached when the best fitness value has not
    /// changed in the specified number of generations.
    /// </summary>
    public class FitnessStagnationTerminationCondition : ITerminationCondition
    {
        public static readonly int DefaultValue = 100;

        private int _stagnatingGenerations;
        private double _bestFitness;

        public FitnessStagnationTerminationCondition() : this(DefaultValue)
        {
        }

        public FitnessStagnationTerminationCondition(int maxStagnatingGenerations)
        {
            MaxStagnatingGenerations = maxStagnatingGenerations;
        }

        public int MaxStagnatingGenerations { get; private set; }

        public string Description
        {
            get { return "Fitness Stagnation"; }
        }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            double bestFitness = geneticAlgorithm.BestIndividual.Fitness;

            if (_bestFitness == bestFitness)
            {
                _stagnatingGenerations++;
            }
            else
            {
                _stagnatingGenerations = 1;
            }

            _bestFitness = bestFitness;

            return _stagnatingGenerations >= MaxStagnatingGenerations;
        }
    }
}