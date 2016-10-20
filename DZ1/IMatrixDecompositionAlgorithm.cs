using System;

namespace DZ1
{
    public interface IMatrixDecompositionAlgorithm
    {
        void DecomposeMatrix(Matrix matrix, out IMatrixDecompositionResult result);
    }
}