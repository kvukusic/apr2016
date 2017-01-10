using System;

namespace APR.DZ4
{
    public class FloatingPointBoundaryMutationOperator : BoundaryMutationOperator<FloatingPointChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public FloatingPointBoundaryMutationOperator(double mutationProbabilitiy)
        {
            if (mutationProbabilitiy < 0 || mutationProbabilitiy > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(mutationProbabilitiy));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _mutationProbability = mutationProbabilitiy;
        }

        public override FloatingPointChromosome Execute(FloatingPointChromosome chromosome)
        {
            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            FloatingPointChromosome mutant = (FloatingPointChromosome) chromosome.Copy();
            IFloatingPointProblem problem = mutant.Problem;

            for (int i = 0; i < problem.Dimension; i++)
            {
                double lowerBound = problem.GetLowerBound(i);
                double upperBound = problem.GetUpperBound(i);

                if (_random.NextDouble() <= _mutationProbability)
                {
                    mutant.SetValue(i, _random.NextDouble() > 0.5 ? lowerBound : upperBound);
                }
            }

            return mutant;
        }
    }
}