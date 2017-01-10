using System;

namespace APR.DZ4
{
    /// <summary>
    /// If some object that is used within a genetic algorithm is aware of its
    /// state, this object' class can implement this interface.
    /// </summary>
    public interface IGeneticAlgorithmStateAware
    {
        IGeneticAlgorithmState CurrentState { get; set; }
    }
}