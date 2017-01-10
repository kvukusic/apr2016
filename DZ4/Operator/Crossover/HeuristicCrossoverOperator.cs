using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    public class HeuristicCrossoverOperator : ICrossoverOperator<FloatingPointChromosome>
    {
        private Random _random;
        private double _crossoverProbability;

        public HeuristicCrossoverOperator(double crossoverProbability)
        {
            if (crossoverProbability < 0 || crossoverProbability > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(crossoverProbability));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _crossoverProbability = crossoverProbability;
        }

        public int NumberOfParents
        {
            get { return 2; }
        }

        public IList<FloatingPointChromosome> Execute(IList<FloatingPointChromosome> parents)
        {
            if (parents == null)
            {
                throw new ArgumentNullException(nameof(parents));
            }

            if (parents.Count != NumberOfParents)
            {
                throw new ArgumentException($"There must be {NumberOfParents} parents instead of {parents.Count}");
            }

            FloatingPointChromosome parent1 = (FloatingPointChromosome)parents[0].Copy();
            FloatingPointChromosome parent2 = (FloatingPointChromosome)parents[1].Copy();

            IList<FloatingPointChromosome> result = new List<FloatingPointChromosome>();

            if (_random.NextDouble() > _crossoverProbability)
            {
                result.Add(parent1);
                result.Add(parent2);
                return result;
            }

            IFloatingPointProblem problem = parent1.Problem;

            // Order parents so that the first has the worst Fitness
            IList<FloatingPointChromosome> orderedParents = parents.OrderBy(c => c, problem.FitnessComparer).ToList();
            parent1 = (FloatingPointChromosome)orderedParents[0].Copy();
            parent2 = (FloatingPointChromosome)orderedParents[1].Copy();

            FloatingPointChromosome child = parent1;
            for (int i = 0; i < parent1.Dimension; i++)
            {
                double parent1Value = parent1.GetValue(i);
                double parent2Value = parent2.GetValue(i);

                double a = _random.NextDouble();

                var childValue = a * (parent2Value - parent1Value) + parent2Value;
                while (childValue < problem.GetLowerBound(i) || childValue > problem.GetUpperBound(i))
                {
                    a = _random.NextDouble();
                    childValue = a * (parent2Value - parent1Value) + parent2Value;
                }

                child.SetValue(i, childValue);
            }

            result.Add(child);
            return result;
        }
    }
}