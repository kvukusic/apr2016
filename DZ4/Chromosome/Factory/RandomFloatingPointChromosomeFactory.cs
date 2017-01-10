using System;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class RandomFloatingPointChromosomeFactory : IChromosomeFactory<FloatingPointChromosome>
    {
        private IFloatingPointProblem _problem;

        public RandomFloatingPointChromosomeFactory(IFloatingPointProblem problem)
        {
            _problem = problem;
        }

        public FloatingPointChromosome Create()
        {
            FloatingPointChromosome result = new FloatingPointChromosome(_problem);
            
            for (int i = 0; i < result.Dimension; i++)
            {
                double lowerBound = _problem.GetLowerBound(i);
                double upperBound = _problem.GetUpperBound(i);

                result.SetValue(i, RandomNumberGeneratorProvider.Instance.NextDouble(lowerBound, upperBound));
            }

            return result;
        }
    }
}