using System;
using System.Collections.Generic;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            var funcDict = new Dictionary<string, GAFunction>()
            {
                {"F1",  new GAFunction(new F1(), 2, new GAFunction.Constraint(-50, 150))},
                {"F3",  new GAFunction(new F3(), 5, new GAFunction.Constraint(-50, 150))},
                {"F6",  new GAFunction(new F6(), 2, new GAFunction.Constraint(-50, 150))},
                {"F7",  new GAFunction(new F7(), 2, new GAFunction.Constraint(-50, 150))},
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
                var ga = new GeneticAlgorithm(funcDict[fKey]);
                ga.Encoding = ChromosomeEncoding.Binary;
                ga.CrossoverOperator = CrossoverOperator.OnePoint;
                ga.StopValue = 10e-6;
                ga.Precision = 6;
                ga.PopulationSize = 1000;
                ga.MutationRate = 5;
                ga.Initialize();
                ga.Run();

                Console.WriteLine();
            }
        }
    }
}