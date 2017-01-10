using System;
using System.Collections;

namespace APR.DZ4
{
    public class BitFlipMutationOperator : IMutationOperator<BinaryChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public BitFlipMutationOperator(double mutationProbabilitiy)
        {
            if (mutationProbabilitiy < 0 || mutationProbabilitiy > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(mutationProbabilitiy));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _mutationProbability = mutationProbabilitiy;
        }

        public BinaryChromosome Execute(BinaryChromosome chromosome)
        {
            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            BinaryChromosome mutant = (BinaryChromosome) chromosome.Copy();

            for (int i = 0; i < mutant.Dimension; i++)
            {
                BitArray gene = mutant.GetValue(i);
                for (int j = 0; j < gene.Length; j++)
                {
                    if (_random.NextDouble() <= _mutationProbability)
                    {
                        gene[j] = !gene[j];
                    }
                }
            }

            return mutant;
        }
    }
}