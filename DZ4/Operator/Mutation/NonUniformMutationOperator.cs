using System;

namespace APR.DZ4
{
    public class NonUniformMutationOperator : IMutationOperator<FloatingPointChromosome>, IGeneticAlgorithmStateAware
    {
        public IGeneticAlgorithmState CurrentState { get; set; }

        public FloatingPointChromosome Execute(FloatingPointChromosome chromosome)
        {
            throw new NotImplementedException();
        }
    }
}