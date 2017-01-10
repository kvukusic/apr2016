using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    /// <summary>
    /// Represents the Roulette Wheel Selection operator.
    /// <remarks>
    /// Selects a single individual from a given population based on proportoins
    /// to the fitness values of these individuals.
    /// Selective pressure is used because we don't know the nature of the fitness
    /// values (minimization, maximization etc.), but we need probabilities
    /// which are proportional to fitness values.
    /// </remarks>
    /// </summary>
    public class RouletteWheelSelectionOperator<T> : ISelectionOperator<T> where T : IChromosome
    {
        private Random _random;
        private double _selectivePressure;
        private IProblem<T> _problem;

        public RouletteWheelSelectionOperator(double selectivePressure, IProblem<T> problem)
        {
            if (selectivePressure < 1.0 || selectivePressure > 2.0)
            {
                throw new ArgumentOutOfRangeException("Selective pressure not between 1 and 2", selectivePressure.ToString());
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _selectivePressure = selectivePressure;
            _problem = problem;
        }

        public T Execute(IList<T> population)
        {
            int n = population.Count;

            // Recreate fitnesses with selective pressure (2 - SP + 2*(SP - 1)*(i-1)/(n-1))
            IList<T> orderedPopulation = population.OrderBy(c => c, _problem.FitnessComparer).ToList();
            IList<double> orderedPopulationFitnesses = new List<double>(n);
            for (int i = 0; i < n; i++)
            {
                double fitness =  2 - _selectivePressure + 2 * (_selectivePressure - 1) * i / (n - 1);
                orderedPopulationFitnesses.Add(fitness);
            }

            double totalFitness = orderedPopulationFitnesses.Sum(fitness => fitness);
            double cumulativeProbability = 0.0;
            IList<double> rouletteWheel = new List<double>(n);
            for (int i = 0; i < n; i++)
            {
                cumulativeProbability += orderedPopulationFitnesses[i] / totalFitness;
                rouletteWheel.Add(cumulativeProbability);
            }

            double randomProbability = _random.NextDouble();
            for (int i = 0; i < n; i++)
            {
                if (rouletteWheel[i] >= randomProbability)
                {
                    return orderedPopulation[i];
                }
            }

            // This should never happen, but in case it happens, return the best individual
            return orderedPopulation.Last();
        }
    }
}