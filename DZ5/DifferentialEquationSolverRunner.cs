using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ1;

namespace APR.DZ5
{
    public class DifferentialEquationSolverRunner
    {
        private IDifferentialEquationSolver[] _solvers;
        private const int Precision = 10;

        public DifferentialEquationSolverRunner(params IDifferentialEquationSolver[] solvers)
        {
            StepOutput = 1;
            _solvers = solvers;
        }

        public int StepOutput { get; set; }

        public IList<Vector[]> StateVariablesThroughTime { get; private set; }
        public IList<double> Time { get; private set; }

        public Matrix[] Solve(Matrix a, Matrix x0, double tmax, double stepSize)
        {
            return Solve(a, Matrix.Zero(x0.Rows, x0.Columns), x0, tmax, stepSize);
        }

        public Matrix[] Solve(Matrix a, Matrix b, Matrix x0, double tmax, double stepSize)
        {
            int n_solvers = _solvers.Length;
            int n_variables = x0.Rows;

            int iterations = (int)Math.Ceiling(tmax / stepSize);
            int k = 0;

            Matrix[] results = new Matrix[n_solvers];
            Matrix xk = x0.Copy();
            double t = 0.0;

            StateVariablesThroughTime = new List<Vector[]>();
            Time = new List<double>();
            Vector[] stateVariables = new Vector[n_solvers];

            ConsoleEx.WriteLineGreen("\nk: " + k);
            
            for (int i = 0; i < n_solvers; i++)
            {
                ConsoleEx.WriteLineYellow(_solvers[i].Name + ":");
                results[i] = _solvers[i].Solve(a, b, x0, t, stepSize);
                results[i].WriteToConsole(Precision);
                stateVariables[i] = results[i].GetColumnVector(0);
            }
            StateVariablesThroughTime.Add(stateVariables);
            Time.Add(t);

            while (k++ < iterations)
            {
                var tmin = t;
                t += stepSize;

                var printIteration = k % StepOutput == 0 ? true : false;

                stateVariables = new Vector[n_solvers];

                if (printIteration) ConsoleEx.WriteLineGreen("\nk: " + k);
                for (int i = 0; i < n_solvers; i++)
                {
                    if (printIteration) ConsoleEx.WriteLineYellow(_solvers[i].Name + ":");
                    results[i] = _solvers[i].Solve(a, b, results[i], tmin, t, stepSize);
                    if (printIteration) results[i].WriteToConsole(Precision);
                    stateVariables[i] = results[i].GetColumnVector(0);
                }
                StateVariablesThroughTime.Add(stateVariables);
                Time.Add(t);
            }

            // Transform data for graph drawing
            double[][][] variableValues = new double[n_variables][][];
            for (int i = 0; i < n_variables; i++)
            {
                variableValues[i] = StateVariablesThroughTime.Select(x => {
                    double[] res = new double[n_solvers];
                    for (int j = 0; j < n_solvers; j++)
                    {
                        res[j] = x[j][i];
                    }
                    return res;
                }).ToArray();
            }

            new GraphGenerator().GenerateLineChart(_solvers.Select(s => s.Name).ToArray(), Time.ToArray(), variableValues);

            ConsoleEx.WriteLineGreen("\nGraph generated.");

            return results;
        }
    }
}