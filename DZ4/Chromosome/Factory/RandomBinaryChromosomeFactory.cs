using System;
using System.Collections;

namespace APR.DZ4
{
    public class RandomBinaryChromosomeFactory : IChromosomeFactory<BinaryChromosome>
    {
        private Random _random;
        private IBinaryProblem _problem;

        public RandomBinaryChromosomeFactory(IBinaryProblem problem)
        {
            _random = RandomNumberGeneratorProvider.Instance;
            _problem = problem;
        }

        public BinaryChromosome Create()
        {
            BinaryChromosome result = new BinaryChromosome(_problem);

            for (int i = 0; i < result.Dimension; i++)
            {
                BitArray gene = BitArrayHelper.CreateRandom(_problem.GetNumberOfBits(i));
                result.SetValue(i, gene);
            }

            return result;
        }
    }
}