using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ4.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            var ga = new GeneticAlgorithm(new GAFunction(new F6(), 2, new GAFunction.Constraint(-150, 50)));
            ga.Encoding = ChromosomeEncoding.FloatingPoint;
            ga.CrossoverOperator = CrossoverOperator.Arithmetic;
            ga.StopEval = null;
            ga.StopValue = 10e-6;
            ga.StopNew = 1000;
            ga.Precision = 6;
            ga.PopulationSize = 30;
            ga.MutationRate = 2;
            ga.Initialize();
            ga.Run();
        }
    }
}