using System;
using APR.DZ1;

namespace APR.DZ5
{
    public interface IDifferentialEquationSolver
    {
        Matrix Solve(Matrix a, Matrix x0, double tmax, double stepSize);
        Matrix Solve(Matrix a, Matrix b, Matrix x0, double tmax, double stepSize);
        Matrix Solve(Matrix a, Matrix b, Matrix x0, double tmin, double tmax, double stepSize);
        string Name { get; }
    }
}