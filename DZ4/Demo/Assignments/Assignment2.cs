using System;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment2 : IAssignment
    {
        public void Run()
        {
            // string fKey = null;

            // Console.WriteLine("Enter function number [6, 7]:");
            // try
            // {
            //     switch (Int32.Parse(Console.ReadLine()))
            //     {
            //         case 6:
            //             fKey = "F6";
            //             break;
            //         case 7:
            //             fKey = "F7";
            //             break;
            //         default:
            //             Console.WriteLine("Invalid input. Try again with numbar from 6, 7.");
            //             break;
            //     }
            // }
            // catch (Exception)
            // {
            //     Console.WriteLine("Invalid input. Try again.");
            //     return;
            // }

            // Console.WriteLine("Enter dimension > 0:");
            // GAFunction f = null;
            // int dimension = 0;
            // try
            // {
            //     var dim = Int32.Parse(Console.ReadLine());
            //     if (dim <= 0)
            //     {
            //         Console.WriteLine("Invalid input. Try again.");
            //         return;
            //     }

            //     dimension = dim;
            // }
            // catch (Exception)
            // {
            //     Console.WriteLine("Invalid input. Try again.");
            //     return;
            // }

            // if (fKey.Equals("F6"))
            // {
            //     f = new GAFunction(new F6(), dimension, new GAFunction.Constraint(-150, 50));
            // }
            // else
            // {
            //     f = new GAFunction(new F7(), dimension, new GAFunction.Constraint(-150, 50));
            // }

            // if (fKey != null)
            // {
            //     Console.WriteLine("Optimization of function " + fKey + ":");
            //     var ga = new GeneticAlgorithm(f);
            //     ga.Encoding = ChromosomeEncoding.FloatingPoint;
            //     ga.CrossoverOperator = CrossoverOperator.Arithmetic;
            //     ga.StopValue = 10e-6;
            //     ga.StopEval = 100000;
            //     ga.Precision = 6;
            //     ga.PopulationSize = 1000;
            //     ga.MutationRate = 5;
            //     ga.Initialize();
            //     ga.Run();

            //     Console.WriteLine();
            // }
        }
    }
}