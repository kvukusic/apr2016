using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmBuilder<T> where T : IChromosome
    {
        private IProblem<T> _problem;
        private ISelectionOperator<T> _selectionOperator;
        private ICrossoverOperator<T> _crossoverOperator;
        private IMutationOperator<T> _mutationOperator;
        private IReplacementOperator<T> _replacementOperator;
        private int _populationSize;
        private int _maxGenerations;
        private int _maxEvaluations;
        private double _fitnessValue;
        private double _elitismRate;
        private double _generationGap;
        private GeneticAlgorithmVariant _variant;

        public GeneticAlgorithmBuilder(
            IProblem<T> problem,
            ICrossoverOperator<T> crossoverOperator,
            IMutationOperator<T> mutationOperator)
        {
            _problem = problem;
            _crossoverOperator = crossoverOperator;
            _mutationOperator = mutationOperator;
            _selectionOperator = new TournamentSelectionOperator<T>(3, problem);
            _replacementOperator = new WorstFitnessReplacementOperator<T>(problem);
            _populationSize = 1000;
            _maxGenerations = 1000;
            _maxEvaluations = 100000;
            _fitnessValue = 10e-6;
            _elitismRate = 1.0/_populationSize;
            _generationGap = 0.4;
            _variant = GeneticAlgorithmVariant.SteadyState;
        }

        public GeneticAlgorithmBuilder<T> SetSelectionOperator(ISelectionOperator<T> selectionOperator)
        {
            _selectionOperator = selectionOperator;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetReplacementOperator(IReplacementOperator<T> replacementOperator)
        {
            _replacementOperator = replacementOperator;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetMaxGenerations(int maxGenerations)
        {
            _maxGenerations = maxGenerations;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetMaxEvaluations(int maxEvaluations)
        {
            _maxEvaluations = maxEvaluations;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetPopulationSize(int populationSize)
        {
            _populationSize = populationSize;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetFitnessValueTreshold(double fitness)
        {
            _fitnessValue = fitness;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetElitismRate(double elitismRate)
        {
            _elitismRate = elitismRate;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetGenerationGap(double generationGap)
        {
            _generationGap = generationGap;
            return this;
        }

        public GeneticAlgorithmBuilder<T> SetGeneticAlgorithmVariant(GeneticAlgorithmVariant variant)
        {
            _variant = variant;
            return this;
        }

        public IGeneticAlgorithm<T> Build()
        {
            if (_variant == GeneticAlgorithmVariant.SteadyState)
            {
                return new SteadyStateGeneticAlgorithm<T>(
                    _problem,
                    _populationSize,
                    _selectionOperator,
                    _crossoverOperator,
                    _mutationOperator,
                    new CompositeTerminationCondition(
                        new MaxGenerationsTerminationCondition(_maxGenerations),
                        new MaxEvaluationsTerminationCondition(_maxEvaluations),
                        new FitnessValueTerminationCondition(fitness => fitness < _fitnessValue)
                    ),
                    _generationGap,
                    _replacementOperator
                );
            }
            else
            {
                return new GenerationalGeneticAlgorithm<T>(
                    _problem,
                    _populationSize,
                    _selectionOperator,
                    _crossoverOperator,
                    _mutationOperator,
                    new CompositeTerminationCondition(
                        new MaxGenerationsTerminationCondition(_maxGenerations),
                        new MaxEvaluationsTerminationCondition(_maxEvaluations),
                        new FitnessValueTerminationCondition(fitness => fitness < _fitnessValue)
                    ),
                    _elitismRate
                );
            }
        }
    }
}