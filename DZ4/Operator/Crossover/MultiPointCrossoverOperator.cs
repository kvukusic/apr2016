using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class MultiPointCrossoverOperator : ICrossoverOperator<BinaryChromosome>
    {
        private Random _random;
        private double _crossoverProbability;
        private int _numberOfCrossPoints;

        public MultiPointCrossoverOperator(double crossoverProbability, int numberOfCrossPoints)
        {
            if (crossoverProbability < 0 || crossoverProbability > 1)
            {
                throw new ArgumentOutOfRangeException("Invalid probability. Expected value in [0, 1]", nameof(crossoverProbability));
            }

            _random = RandomNumberGeneratorProvider.Instance;
            _crossoverProbability = crossoverProbability;
            _numberOfCrossPoints = numberOfCrossPoints;
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

            BinaryChromosome child1 = parent1;
            BinaryChromosome child2 = parent2;

            for (int i = 0; i < problem.Dimension; i++)
            {
                BitArray parent1Value = parent1.GetValue(i);
                BitArray parent2Value = parent2.GetValue(i);

                int numberOfBits = problem.GetNumberOfBits(i);

                // If the number of specified cross points is larger than the number of bits
                // then use cross point for every bit
                var crossPointsCount = Math.Min(numberOfBits-1, _numberOfCrossPoints);
                var crossPoints = _random.NextInts(0, numberOfBits, crossPointsCount);
                crossPoints = crossPoints.OrderBy(p => p).ToArray();
                int previousCrossPoint = 0;
                for (int j = 0; j < crossPoints.Length; j++)
                {
                    for (int k = previousCrossPoint; k < crossPoints[j]; k++)
                    {
                        child1[i][k] = i%2==0 ? parent2Value[k] : parent1Value[k];
                        child2[i][k] = i%2==0 ? parent1Value[k] : parent2Value[k];
                    }

                    previousCrossPoint = crossPoints[j];
                }
            }

            result.Add(child1);
            result.Add(child2);
            return result;
        }
    }
}