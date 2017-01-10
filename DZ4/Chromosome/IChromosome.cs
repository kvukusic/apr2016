using System;

namespace APR.DZ4
{
    public interface IChromosome
    {
        /// <summary>
        /// Gets the fitness of this chromosome.
        /// </summary>
        double Fitness { get; }

        /// <summary>
        /// Gets the number of dimensions this chromosome consists of.
        /// A dimension can be called a gene.
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// Copies this chromosome.
        /// </summary>
        IChromosome Copy();
    }
}