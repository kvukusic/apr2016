using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            // var ga = new GeneticAlgorithm(new GAFunction(new F6(), 2, new GAFunction.Constraint(-150, 50)));
            // ga.Encoding = ChromosomeEncoding.FloatingPoint;
            // ga.CrossoverOperator = CrossoverOperator.Arithmetic;
            // ga.StopEval = null;
            // ga.StopValue = 10e-6;
            // ga.StopNew = 1000;
            // ga.Precision = 6;
            // ga.PopulationSize = 30;
            // ga.MutationRate = 2;
            // ga.Initialize();
            // ga.Run();

            var problem = new FunctionMinimizationBinaryProblem(new F1(), 2, 4, -50, 150);
            var chromosomeFactory = new RandomBinaryChromosomeFactory(problem);
            var populationSize = 10000;
            var selectionOperator = new TournamentSelectionOperator<BinaryChromosome>(5, problem);
            var crossoverOperator = new UniformCrossoverOperator(0.75);
            var mutationOperator = new SimpleMutationOperator(0.7);
            var terminationCondition = new CompositeTerminationCondition(
                new MaxGenerationsTerminationCondition(1000),
                new FitnessValueTerminationCondition(fitness => fitness < 10e-6),
                new FitnessStagnationTerminationCondition(100)
            );
            var elitismRate = 1.0/populationSize;

            var ga = new SteadyStateGeneticAlgorithm<BinaryChromosome>(
                problem,
                populationSize,
                chromosomeFactory,
                selectionOperator,
                crossoverOperator,
                mutationOperator,
                terminationCondition,
                0.4,
                new WorstFitnessReplacementOperator<BinaryChromosome>(problem)
            );

            ga.Run();

            Console.WriteLine(ga.BestIndividual + " " + ga.BestIndividual.Fitness);
        }
    }
}