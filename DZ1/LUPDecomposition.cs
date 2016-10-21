using System;

namespace DZ1
{
    public class LUPDecomposition : IDecomposition
    {
        public bool Decompose(Matrix matrix)
        {
            ConsoleEx.WriteLine();
            ConsoleEx.WriteLineGreen("Starting LUP decomposition of matrix:");
            ConsoleEx.WriteLine(matrix.ToString());

            if (!matrix.IsSquareMatrix())
            {
                ConsoleEx.WriteLineRed("LUP decomposition not possible on non-square matrix.");
                return false;
            }

            /*za i = 1 do n
                P[i] = i;
            za i = 1 do n - 1
                pivot = i;
                za j = i + 1 do n
                    ako(abs(A[P[j], i]) > abs(A[P[pivot], i))
                        pivot = j;
                zamijeni(P[i], P[pivot]);
                za j = i + 1 do n
                    A[P[j], i] /= A[P[i], i];
                    za k = i + 1 do n
                        A[P[j], k] -= A[P[j], i] * A[P[i], k];*/

            int[] p = new int[matrix.Rows];
            for (int i = 0; i < matrix.Rows; i++)
            {
                p[i] = i;
            }

            int pivot;
            for (int i = 0; i < matrix.Rows - 1; i++)
            {
                pivot = i;
                for (int j = i + 1; j < matrix.Rows; j++)
                {
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[pivot][i]))
                    {
                        pivot = j;
                    }
                }

                int temp = p[i];
                p[i] = p[pivot];
                p[pivot] = temp;

                Matrix tempRow = matrix.GetRow(i);
                matrix.SetRow(i, matrix.GetRow(pivot));
                matrix.SetRow(pivot, tempRow);

                for (int j = i + 1; j < matrix.Rows; j++)
                {
                    if (Math.Abs(matrix[i][i]) < Matrix.EPSILON)
                    {
                        ConsoleEx.WriteLineRed("LUP decomposition not possible because matrix is singular.");
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
            P = new Matrix(matrix.Rows, matrix.Columns);
            for (int i = 0; i < matrix.Rows; i++)
            {
                P[i][p[i]] = 1.0;
            }

            ConsoleEx.WriteLineGreen();
            ConsoleEx.WriteLineGreen("LUP decomposition successfull:");
            ConsoleEx.WriteLine("L:");
            ConsoleEx.WriteLine(L.ToString());
            ConsoleEx.WriteLine("U:");
            ConsoleEx.WriteLine(U.ToString());
            ConsoleEx.WriteLine("P:");
            ConsoleEx.WriteLine(P.ToString());

            return true;
        }

        public Matrix L { get; private set; }

        public Matrix U { get; private set; }

        public Matrix P { get; private set; }
    }
}