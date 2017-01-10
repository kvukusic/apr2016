using System;
using System.Collections;
using System.Collections.Generic;

namespace APR.DZ4
{
    public class UniformCrossoverOperator : ICrossoverOperator<BinaryChromosome>
    {
        private Random _random;
        private double _crossoverProbability;

        public UniformCrossoverOperator(double crossoverProbability)
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

        public IList<BinaryChromosome> Execute(IList<BinaryChromosome> parents)
        {
            if (parents == null)
            {
                throw new ArgumentNullException(nameof(parents));
            }

            if (parents.Count != NumberOfParents)
            {
                throw new ArgumentException($"There must be {NumberOfParents} parents instead of {parents.Count}");
            }

            BinaryChromosome parent1 = (BinaryChromosome)parents[0].Copy();
            BinaryChromosome parent2 = (BinaryChromosome)parents[1].Copy();

            IList<BinaryChromosome> result = new List<BinaryChromosome>();

            if (_random.NextDouble() > _crossoverProbability)
            {
                result.Add(parent1);
                result.Add(parent2);
                return result;
            }

            IBinaryProblem problem = (IBinaryProblem)parent1.Problem;

            BinaryChromosome child = (BinaryChromosome)parent1.Copy();

            for (int i = 0; i < problem.Dimension; i++)
            {
                int numberOfBits = problem.GetNumberOfBits(i);

                BitArray parent1Value = parent1.GetValue(i);
                BitArray parent2Value = parent2.GetValue(i);
                BitArray randomValue = BitArrayHelper.CreateRandom(numberOfBits);

                BitArray childValue = new BitArray(numberOfBits);
                for (int j = 0; j < numberOfBits; j++)
                {
                    bool a = parent1Value[j];
                    bool b = parent2Value[j];
                    bool r = randomValue[j];

                    childValue[j] = (a & b) | (r & (a ^ b));
                }

                child.SetValue(i, childValue);
            }

            result.Add(child);
            return result;
        }
    }
}