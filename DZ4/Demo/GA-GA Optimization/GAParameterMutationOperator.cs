using System;

namespace APR.DZ4.Demo
{
    public class GAParameterMutationOperator : IMutationOperator<GAParametersChromosome>
    {
        private double _mutationProbability;

        public GAParameterMutationOperator(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
        }

        public GAParametersChromosome Execute(GAParametersChromosome chromosome)
        {
            GAParametersChromosome child = (GAParametersChromosome)chromosome.Copy();
            child.IsEvaluated = true;

            if (RandomNumberGeneratorProvider.Instance.NextDouble() <= _mutationProbability)
            {
                child.SetValue(0, GAParameters.Mutate(child.GetValue(0)));
                child.IsEvaluated = false;
            }

            return child;
        }
    }
}