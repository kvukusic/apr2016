using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmBuilder<T> where T : IChromosome
    {
        private IProblem<IChromosome> _problem;
        private IChromosomeFactory<IChromosome> _chromosomeFactory;

        public GeneticAlgorithmBuilder(
            IProblem<IChromosome> problem,
            ICrossoverOperator<IChromosome> crossoverOperator,
            IMutationOperator<IChromosome> mutationOperator)
        {

        }

        public IGeneticAlgorithm<T> Build()
        {
            return null;
        }
    }
}