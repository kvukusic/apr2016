using System;
using System.Collections;

namespace APR.DZ4
{
    public class BinaryBoundaryMutationOperator : BoundaryMutationOperator<BinaryChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public BinaryBoundaryMutationOperator(double mutationProbabilitiy)
        {
            if (mutationProbabilitiy < 0 || mutationProbabilitiy > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(mutationProbabilitiy));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _mutationProbability = mutationProbabilitiy;
        }

        public override BinaryChromosome Execute(BinaryChromosome chromosome)
        {
            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            BinaryChromosome mutant = (BinaryChromosome) chromosome.Copy();

            for (int i = 0; i < mutant.Dimension; i++)
            {
                if (_random.NextDouble() <= _mutationProbability)
                {
                    BitArray gene = mutant.GetValue(i);
                    mutant.SetValue(i, new BitArray(gene.Length, _random.NextDouble() > 0.5));                    
                }
            }

            return mutant;
        }
    }
}