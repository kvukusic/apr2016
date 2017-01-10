using System;

namespace APR.DZ4
{
    public interface IBinaryProblem : IProblem<BinaryChromosome>
    {
        int GetNumberOfBits(int index);
    }
}