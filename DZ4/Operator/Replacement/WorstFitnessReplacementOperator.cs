using System;
using System.Linq;
using System.Collections.Generic;

namespace APR.DZ4
{
    /// <summary>
    /// Replaces the worst individuals from a population with new individuals.
    /// </summary>
    public class WorstFitnessReplacementOperator<T> : IReplacementOperator<T> where T : IChromosome
    {
        private IProblem<T> _problem;

        public WorstFitnessReplacementOperator(IProblem<T> problem)
        {
            _problem = problem;
        }

        public IList<T> Execute(IList<T> population, IList<T> offspringPopulation)
        {
            IList<T> orderedPopulation = population.OrderBy(c => c, _problem.FitnessComparer).ToList();

            for (int i = 0; i < offspringPopulation.Count; i++)
            {
                T individualToReplace = orderedPopulation[i];
                population[population.IndexOf(individualToReplace)] = offspringPopulation[i];
            }

            return population;
        }
    }
}