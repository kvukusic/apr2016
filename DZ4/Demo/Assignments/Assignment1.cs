using System;
using System.Collections.Generic;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            var funcDict = new Dictionary<string, Tuple<Function, int>>()
            {
                {"F1", new Tuple<Function, int>(new F1(), 2) },
                {"F3", new Tuple<Function, int>(new F3(), 5) },
                {"F6", new Tuple<Function, int>(new F6(), 2) },
                {"F7", new Tuple<Function, int>(new F7(), 2) },
            };

            string fKey = null;

            Console.WriteLine("Enter function number [1, 3, 6, 7]:");
            try
            {
                switch (Int32.Parse(Console.ReadLine()))
                {
                    case 1:
                        fKey = "F1";
                        break;
                    case 3:
                        fKey = "F3";
                        break;
                    case 6:
                        fKey = "F6";
                        break;
                    case 7:
                        fKey = "F7";
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again with number from 1, 3, 6, 7.");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                return;
            }

            if (fKey != null)
            {
                Console.WriteLine("Optimization of function " + fKey + ":");

                var function = funcDict[fKey].Item1;
                var dimension = funcDict[fKey].Item2;

                new GeneticAlgorithmRunner<FloatingPointChromosome>(
                    new GeneticAlgorithmBuilder<FloatingPointChromosome>(
                        new FunctionMinimizationFloatingPointProblem(function, dimension, -50, 150),
                        new HeuristicCrossoverOperator(0.75),
                        new GaussianMutationOperator(0.1)
                )
                .SetGeneticAlgorithmVariant(GeneticAlgorithmVariant.SteadyState)
                .SetFitnessValueTreshold(10e-6)
                .Build()).Run();

                Console.WriteLine();
            }
        }
    }
}