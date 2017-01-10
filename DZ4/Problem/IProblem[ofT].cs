using System;
using System.Collections.Generic;

namespace APR.DZ4
{
    public interface IProblem<T> : IProblem where T : IChromosome
    {
        /// <summary>
        /// Evaluates the fitness of the given chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome whose fitness is the be evaluated.</param>
        void Evaluate(T chromosome);

        /// <summary>
        /// Gets the number of fitness function evaluations.
        /// </summary>
        int FitnessFunctionEvaluations { get; }

        /// <summary>
        /// Gets the fitness value comparer for this problem.
        /// </summary>
        IComparer<T> FitnessComparer { get; }

        /// <summary>
        /// Creates a chromosome which can be used in this problem.
        /// </summary>
        T CreateChromosome();
    }
}