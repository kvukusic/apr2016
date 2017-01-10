using System;

namespace APR.DZ4
{
    public interface IProblem
    {
        /// <summary>
        /// Gets the dimension (number of variables) of this problem.
        /// </summary>
        int Dimension { get; }
    }
}