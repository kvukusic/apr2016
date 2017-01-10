using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmState : IGeneticAlgorithmState
    {
        public int Generation { get; set; }
        public int Evaluations { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }

    public class GeneticAlgorithmState<T> : GeneticAlgorithmState, IGeneticAlgorithmState<T>
    {
        public T BestIndividual { get; set; }
    }
}