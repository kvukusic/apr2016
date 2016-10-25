using System;

namespace APR.DZ1
{
    public class LUDecomposition : IDecomposition
    {
        public bool Decompose(Matrix matrix)
        {
            ConsoleEx.WriteLine();
            ConsoleEx.WriteLineGreen("Starting LU decomposition of matrix:");
            ConsoleEx.WriteLine(matrix.ToString());

            if (!matrix.IsSquareMatrix())
            {
                ConsoleEx.WriteLineRed("LU decomposition not possible on non-square matrix.");
                return false;
            }

            /*za i = 1 do n - 1
                za j = i + 1 do n
                    A[j, i] /= A[i, i];
                    za k = i + 1 do n
                        A[j, k] -= A[j, i] * A[i, k];*/

            for (int i = 0; i < matrix.Rows - 1; i++)
            {
                for (int j = i + 1; j < matrix.Rows; j++)
                {
                    if (Math.Abs(matrix[i][i]) < Matrix.EPSILON)
                    {
                        ConsoleEx.WriteLineRed("LU decomposition not possible because of division by zero.");
                        return false;
                    }

                    matrix[j][i] /= matrix[i][i];
                    for (int k = i + 1; k < matrix.Rows; k++)
                    {
                        matrix[j][k] -= matrix[j][i] * matrix[i][k];
                    }
                }
            }

            L = matrix.ToLowerTriangularMatrix();
            U = matrix.ToUpperTriangularMatrix();

            ConsoleEx.WriteLineGreen();
            ConsoleEx.WriteLineGreen("LU decomposition successfull:");
            ConsoleEx.WriteLine("L:");
            ConsoleEx.WriteLine(L.ToString());
            ConsoleEx.WriteLine("U:");
            ConsoleEx.WriteLine(U.ToString());

            return true;
        }

        public Matrix L { get; private set; }

        public Matrix U { get; private set; }
    }
}