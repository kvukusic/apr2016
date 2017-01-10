using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    /// <summary>
    /// The arithmetic crossover operator (local crx).
    /// Two children are generated using a new random number for every variable.
    /// </summary>
    public class ArithmeticCrossoverOperator : ICrossoverOperator<FloatingPointChromosome>
    {
        private Random _random;
        private double _crossoverProbability;

        public ArithmeticCrossoverOperator(double crossoverProbability)
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

            FloatingPointChromosome child1 = parent1;
            FloatingPointChromosome child2 = parent2;

            for (int i = 0; i < parent1.Dimension; i++)
            {
                double parent1Value = parent1.GetValue(i);
                double parent2Value = parent2.GetValue(i);

                double a = _random.NextDouble();

                child1.SetValue(i, a * parent1Value + (1 - a) * parent2Value);
                child2.SetValue(i, (1 - a) * parent1Value + a * parent2Value);
            }

            result.Add(child1);
            result.Add(child2);

            return result;
        }
    }
}