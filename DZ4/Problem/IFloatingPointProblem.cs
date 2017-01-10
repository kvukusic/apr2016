using System;

namespace APR.DZ4
{
    public interface IFloatingPointProblem : IProblem<FloatingPointChromosome>
    {
        double GetLowerBound(int index);
        double GetUpperBound(int index);
    }
}