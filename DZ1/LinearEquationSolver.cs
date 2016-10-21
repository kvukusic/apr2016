using System;

namespace DZ1
{
    public class LinearEquationSolver
    {
        private static Matrix Solve(Matrix l, Matrix u, Matrix b)
        {
            Matrix y = SubstituteForward(l, b);
            Matrix x = SubstituteBackward(l, y);
            return x;
        }

        public static Matrix SolveLU(Matrix a, Matrix b)
        {
            ConsoleEx.WriteLineGreen();
            ConsoleEx.WriteLineGreen("Solving a system of linear equations Ax=b where:");
            ConsoleEx.WriteLine("A:");
            ConsoleEx.WriteLine(a.ToString());
            ConsoleEx.WriteLine("b:");
            ConsoleEx.WriteLine(b.ToString());

            var aCopy = a.Copy();
            var decomposition = new LUDecomposition();
            if(decomposition.Decompose(aCopy))
            {
                var result = Solve(decomposition.L, decomposition.U, b);
                
                Console.WriteLine();
                ConsoleEx.WriteLineGreen("The given system has the following solution:");
                ConsoleEx.WriteLine("x:");
                ConsoleEx.WriteLine(result.ToString());

                return result;
            }

            ConsoleEx.WriteLineRed("The given system has no solutions.");

            return null;
        }

        public static Matrix SolveLUP(Matrix a, Matrix b)
        {
            ConsoleEx.WriteLineGreen();
            ConsoleEx.WriteLineGreen("Solving a system of linear equations Ax=b where:");
            ConsoleEx.WriteLine("A:");
            ConsoleEx.WriteLine(a.ToString());
            ConsoleEx.WriteLine("b:");
            ConsoleEx.WriteLine(b.ToString());

            var aCopy = a.Copy();
            var decomposition = new LUPDecomposition();
            if(decomposition.Decompose(aCopy))
            {
                var result = Solve(decomposition.L, decomposition.U, decomposition.P*b);

                Console.WriteLine();
                ConsoleEx.WriteLineGreen("The given system has the following solution:");
                ConsoleEx.WriteLine("x:");
                ConsoleEx.WriteLine(result.ToString());

                return result;
            }

            ConsoleEx.WriteLineRed("The given system has no solutions.");

            return null;
        }

        public static Matrix SubstituteForward(Matrix a, Matrix b)
        {
            Matrix result = b.Copy();
            for (int i = 0; i < a.Rows - 1; i++)
            {
                for (int j = i + 1; j < a.Rows; j++)
                {
                    result[j][0] -= a[j][i] * result[i][0];
                }
            }
            return result;
        }

        public static Matrix SubstituteBackward(Matrix a, Matrix b)
        {
            Matrix result = b.Copy();
            for (int i = a.Rows - 1; i >= 0 && i < a.Rows; i--)
            {
                if(Math.Abs(a[i][i]) < Matrix.EPSILON)
                {
                    // TODO
                    ConsoleEx.WriteLineRed("Possible zero division");
                }

                result[i][0] /= a[i][i];
                for (int j = 0; j < i; j++)
                {
                    result[j][0] -= a[j][i] * result[i][0];
                }
            }
            return result;
        }
    }
}