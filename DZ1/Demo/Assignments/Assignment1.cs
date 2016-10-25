using System;

namespace APR.DZ1.Demo.Assignments
{
    public class Assignment1 : IAssignment
    {
        public void Run()
        {
            Matrix m1 = new Matrix("Input/M1.txt");
            Matrix m2 = m1.Copy();
            m1 /= Math.PI;
            m1 *= Math.PI;

            Console.WriteLine("M1:");
            m1.WriteToConsole();
            Console.WriteLine("M2:");
            m2.WriteToConsole();
            Console.WriteLine("Matrices are same? " + ((m1 == m2) ? "TRUE" : "FALSE"));

            // Matrices will be the same because the precision of Matrix operations (Matrix.PRECISION)
            // is set to a small value (default is 6)
            // Precision of 16 decimals will return FALSE
            // The output will seem as same because of 15 digits precision of double in .NET
        }
    }
}