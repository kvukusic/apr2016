using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class TournamentSelectionOperator<T> : ISelectionOperator<T> where T : IChromosome
    {
        private int _tournamentSize;
        private IProblem<T> _problem;
        private Random _random;

        public TournamentSelectionOperator(int tournamentSize, IProblem<T> problem)
        {
            _random = RandomNumberGeneratorProvider.Instance;
            _tournamentSize = tournamentSize;
            _problem = problem;
        }

        public T Execute(IList<T> population)
        {
            IList<T> candidates = new List<T>(_tournamentSize);

            for (int i = 0; i < _tournamentSize; i++)
            {
                candidates.Add(population[_random.NextInt(0, population.Count)]);
            }

            return candidates.OrderByDescending(c => c, _problem.FitnessComparer).First();
        }
    }
}