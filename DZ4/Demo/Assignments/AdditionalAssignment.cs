using System;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class AdditionalAssignment : IAssignment
    {
        public void Run()
        {
            Function f = new F1();
            int dimension = 2;

            new GeneticAlgorithmRunner<GAParametersChromosome>(new GeneticAlgorithmBuilder<GAParametersChromosome>(
                new GAParameterOptimizationProblem(f, dimension),
                new GAParameterCrossoverOperator(0.75),
                new GAParameterMutationOperator(0.1)
            ).SetElitismRate(0.01).SetPopulationSize(100).SetFitnessValueTreshold(0).Build()).Run();
        }
    }
}