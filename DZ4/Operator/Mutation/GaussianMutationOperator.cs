using System;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class GaussianMutationOperator : IMutationOperator<FloatingPointChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public GaussianMutationOperator(double mutationProbabilitiy)
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

            FloatingPointChromosome mutant = (FloatingPointChromosome) chromosome.Copy();
            IFloatingPointProblem problem = mutant.Problem;

            for (int i = 0; i < problem.Dimension; i++)
            {
                if (_random.NextDouble() <= _mutationProbability)
                {
                    double lowerBound = problem.GetLowerBound(i);
                    double upperBound = problem.GetUpperBound(i);

                    double mu = (upperBound - lowerBound)/2;
                    double sigma = upperBound - mu;
                    mu /= 3;

                    mutant.SetValue(i, Math.Min(Math.Max(_random.NextGaussian(mu, sigma), lowerBound), upperBound));
                }
            }

            return mutant;
        }
    }
}