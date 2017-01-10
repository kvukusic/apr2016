using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    public class GenerationalGeneticAlgorithm<T> : AbstractGeneticAlgorithm<T> where T : IChromosome
    {
        private double _elitismRate;

        public GenerationalGeneticAlgorithm(
            IProblem<T> problem,
            int populationSize,
            ISelectionOperator<T> selectionOperator, 
            ICrossoverOperator<T> crossoverOperator, 
            IMutationOperator<T> mutationOperator,
            ITerminationCondition terminationCondition,
            double elitismRate) 
            : base(problem, populationSize, selectionOperator, 
                crossoverOperator, mutationOperator, terminationCondition)
        {
            _elitismRate = elitismRate;
        }

        public override void Evolve()
        {
            IList<T> offspringPopulation = new List<T>();

            // Let elites survive to next generation
            int numberOfElites = (int) (_elitismRate * _populationSize);
            IList<T> elites = _population
                                .OrderByDescending(c => c, _problem.FitnessComparer)
                                .Take(numberOfElites).ToList();
            foreach (T individual in elites)
            {
                offspringPopulation.Add(individual);
            }

            while (offspringPopulation.Count < _populationSize)
            {
                IList<T> matingPool = new List<T>(CrossoverOperator.NumberOfParents);
                for (int i = 0; i < CrossoverOperator.NumberOfParents; i++)
                {
                    matingPool.Add(SelectionOperator.Execute(_population));
                }

                IList<T> children = CrossoverOperator.Execute(matingPool);

                for (int i = 0; i < children.Count; i++)
                {
                    MutationOperator.Execute(children[i]);
                    _problem.Evaluate(children[i]);
                    offspringPopulation.Add(children[i]);
                }
            }

            // When there is some excess chromosomes, discard them by taking only the needed ones
            _population = offspringPopulation.Take(_populationSize).ToList();

            UpdateState();
        }

        public override void Run()
        {
            InitState();

            while(!IsTerminationConditionReached())
            {
                Evolve();
            }
        }
    }
}