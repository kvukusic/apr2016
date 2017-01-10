using System;
using System.Collections;
using System.Linq;

namespace APR.DZ4
{
    public class SimpleMutationOperator : IMutationOperator<BinaryChromosome>
    {
        private Random _random;
        private double _mutationProbability;

        public SimpleMutationOperator(double mutationProbabilitiy)
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
            IBinaryProblem problem = (IBinaryProblem)mutant.Problem;

            int totalBits = mutant.GetValues().Sum(gene => gene.Length);

            if (_random.NextDouble() <= 1 - Math.Pow(1 - _mutationProbability, totalBits))
            {
                for (int i = 0; i < mutant.Dimension; i++)
                {
                    BitArray gene = mutant.GetValue(i);
                    int chosenIndex = _random.Next(0, gene.Length);
                    gene[chosenIndex] = !gene[chosenIndex];
                    mutant.SetValue(i, gene);
                }
            }

            return mutant;
        }
    }
}