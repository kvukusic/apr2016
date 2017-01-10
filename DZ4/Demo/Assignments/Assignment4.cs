using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            // var populations = new int[]{30,50,100,200,500,1000,3000};

            // Population and tournament size box plot data made with following alogorithm

            var populationSizes = new int[]{30,50,100,200,500,1000,3000};

            for (int j = 0; j < 10; j++)
            {
                string result = "";
                for (int i = 0; i < populationSizes.Length; i++)
                {
                    var populationSize = populationSizes[i];
                    var problem = new FunctionMinimizationFloatingPointProblem(new F6(), 2, -50, 150);
                    // var populationSize = 500;
                    var tournamentSize = 3;
                    var selectionOperator = new TournamentSelectionOperator<FloatingPointChromosome>(tournamentSize, problem);
                    var crossoverOperator = new HeuristicCrossoverOperator(0.8);
                    var mutationOperator = new GaussianMutationOperator(0.05);
                    var elitismRate = 1.0 / populationSize;

                    var ga = new GeneticAlgorithmBuilder<FloatingPointChromosome>(
                            problem,
                            crossoverOperator,
                            mutationOperator
                    )
                    .SetGeneticAlgorithmVariant(GeneticAlgorithmVariant.Generational)
                    .SetPopulationSize(populationSize)
                    .SetElitismRate(1.0 / populationSize)
                    .Build();

                    // new GeneticAlgorithmRunner<FloatingPointChromosome>(ga).Run();
                    ga.Run();

                    result += ga.BestIndividual.Fitness;
                    if (i != populationSizes.Length - 1)  result += ",";
                }

                Console.WriteLine(result);
            }
        }
    }
}