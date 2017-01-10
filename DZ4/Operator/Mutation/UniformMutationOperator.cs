using System;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class UniformMutationOperator : IMutationOperator<FloatingPointChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public UniformMutationOperator(double mutationProbabilitiy)
        {
            if (mutationProbabilitiy < 0 || mutationProbabilitiy > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(mutationProbabilitiy));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _mutationProbability = mutationProbabilitiy;
        }

        public FloatingPointChromosome Execute(FloatingPointChromosome chromosome)
        {
            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            FloatingPointChromosome mutant = (FloatingPointChromosome)chromosome.Copy();

            if (_random.NextDouble() > _mutationProbability)
            {
                return mutant;
            }

            IFloatingPointProblem problem = chromosome.Problem;
            for (int i = 0; i < problem.Dimension; i++)
            {
                double lowerBound = problem.GetLowerBound(i);
                double upperBound = problem.GetUpperBound(i);
                double mutantValue = _random.NextDouble(lowerBound, upperBound);
                mutant.SetValue(i, mutantValue);
            }

            return mutant;
        }
    }
}