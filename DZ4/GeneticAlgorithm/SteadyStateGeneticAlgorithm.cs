using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    public class SteadyStateGeneticAlgorithm<T> : AbstractGeneticAlgorithm<T> where T : IChromosome
    {
        /// <summary>
        /// The percentage of the population that is being replaced
        /// in every generation of this algorithm.
        /// </summary>
        /// <remarks>The value needs to be between 0.0 and 1.0</remarks>
        protected double _generationGap;
        protected IReplacementOperator<T> _replacementOperator;

        public SteadyStateGeneticAlgorithm(
            IProblem<T> problem,
            int populationSize,
            ISelectionOperator<T> selectionOperator, 
            ICrossoverOperator<T> crossoverOperator, 
            IMutationOperator<T> mutationOperator,
            ITerminationCondition terminationCondition,
            double generationGap,
            IReplacementOperator<T> replacementOperator) : 
            base(problem, populationSize, selectionOperator,
                crossoverOperator, mutationOperator, terminationCondition)
        {
            _generationGap = generationGap;
            _replacementOperator = replacementOperator;
        }

        public IReplacementOperator<T> ReplacementOperator { get { return _replacementOperator; } }

        public override void Evolve()
        {
            int individualsToEliminate = (int)(_generationGap * _populationSize);

            // A single generation is when GenerationGap % of population is eliminated and replaced
            IList<T> offspringPopulation = new List<T>();
            while (offspringPopulation.Count < individualsToEliminate)
            {
                IList<T> matingPool = new List<T>(CrossoverOperator.NumberOfParents);
                for (int j = 0; j < CrossoverOperator.NumberOfParents; j++)
                {
                    T mate = SelectionOperator.Execute(_population);
                    matingPool.Add(mate);
                }

                IList<T> children = CrossoverOperator.Execute(matingPool);

                for (int j = 0; j < children.Count; j++)
                {
                    MutationOperator.Execute(children[j]);
                    _problem.Evaluate(children[j]);
                    offspringPopulation.Add(children[j]);
                }
            }

            offspringPopulation = offspringPopulation.Take(individualsToEliminate).ToList();

            _population = ReplacementOperator.Execute(_population, offspringPopulation);

            UpdateState();
        }

        public override void Run()
        {
            InitState();

            while (!IsTerminationConditionReached())
            {
                Evolve();
            }
        }
    }
}