using System;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            string fKey = null;

            Console.WriteLine("Enter function number [6, 7]:");
            try
            {
                switch (Int32.Parse(Console.ReadLine()))
                {
                    case 6:
                        fKey = "F6";
                        break;
                    case 7:
                        fKey = "F7";
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again with numbar from 6, 7.");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                return;
            }

            Console.WriteLine("Enter dimension > 0:");
            int dimension = 0;
            try
            {
                var dim = Int32.Parse(Console.ReadLine());
                if (dim <= 0)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    return;
                }

                dimension = dim;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                return;
            }

            Function f = fKey.Equals("F6") ? (Function) new F6() : new F7();
            if (fKey != null)
            {

                new GeneticAlgorithmRunner<FloatingPointChromosome>(
                    new GeneticAlgorithmBuilder<FloatingPointChromosome>(
                        new FunctionMinimizationFloatingPointProblem(f, dimension, -50, 150),
                        new HeuristicCrossoverOperator(0.75),
                        new UniformMutationOperator(0.1)
                )
                .SetGeneticAlgorithmVariant(GeneticAlgorithmVariant.SteadyState)
                .SetFitnessValueTreshold(10e-6)
                .Build()).Run();

                Console.WriteLine();
            }
        }
    }
}