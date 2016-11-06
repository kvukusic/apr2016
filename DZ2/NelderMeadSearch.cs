using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ1.Extensions;

namespace APR.DZ2
{
    public class NelderMeadSearch : IMinimizer
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);
        private readonly int MAX_IT = 100000;
        private readonly double ALPHA = 1.0;
        private readonly double BETA = 0.5;
        private readonly double GAMMA = 2.0;
        private readonly double SIGMA = 0.5;
        
        private double SIMPLEX_OFFSET = 1.0;

        public NelderMeadSearch()
        {
            IsOutputPerIterationEnabled = true;
            IsOutputEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whetehr values of every iteration
        /// should be output.
        /// </summary>
        public bool IsOutputPerIterationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating this algorithm produces any output.
        /// </summary>
        public bool IsOutputEnabled { get; set; }

        /// <summary>
        /// Gets or sets the offset which is used in generating the initial simplex.
        /// </summary>
        public double SimplexOffset
        {
            get { return SIMPLEX_OFFSET; }
            set { SIMPLEX_OFFSET = value; }
        }

        public double[] Minimize(Function function, double[] start)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (start.Length == 0)
            {
                throw new ArgumentException("Dimension of given starting point is invalid");
            }

            // Clear function
            function.Clear();

            if (IsOutputEnabled)
            {

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLineGreen("****************************************************************");
                ConsoleEx.WriteLineGreen("Starting minimization with Nelder Mead Simplex Search method...");

                ConsoleEx.WriteLine();
                ConsoleEx.WriteLine("Parameters: ");
                ConsoleEx.WriteLine("x0 = " + start.Format(PRECISION));
                ConsoleEx.WriteLine("alpha = " + ALPHA.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("beta = " + BETA.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("gamma = " + GAMMA.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("sigma = " + SIGMA.ToString("F" + PRECISION));
                ConsoleEx.WriteLine("t = " + SIMPLEX_OFFSET.ToString("F" + PRECISION));
                ConsoleEx.WriteLine();
            }

            int vs; // Vertex with smallest value
            int vh; // Vertex with next smallest value
            int vg; // Vertex with largest value

            int i, j, m, row;
            int itr; // Track the number of iterations

            double[][] v; // Holds vertices of simplex
            double pn, qn; // Values used to create initial simplex
            double[] f; // Value of function at each vertex
            double fr; // Value of function at reflection point
            double fe; // Value of function at expansion point
            double fc; // Value of function at contraction point
            double[] vr; // Reflection - coordinates
            double[] ve; // Expansion - coordinates
            double[] vc; // Contraction - coordinates
            double[] vm; // Centroid - coordinates

            double fsum, favg, s, cent;

            // Dimension
            int n = start.Length;

            // Allocate the rows of the arrays
            v = new double[n+1][];
            f = new double[n+1];
            vr = new double[n];
            ve = new double[n];
            vc = new double[n];
            vm = new double[n];

            // Allocate the columns of the arrays
            for (i = 0; i <= n; i++)
            {
                v[i] = new double[n];
            }

            // Create the initial simplex
            // Assume one of the vertices is 0,0
            pn = SIMPLEX_OFFSET*(Math.Sqrt(n + 1) - 1 + n)/(n*Math.Sqrt(2));
            qn = SIMPLEX_OFFSET*(Math.Sqrt(n + 1) - 1)/(n*Math.Sqrt(2));

            for (i = 0; i < n; i++)
            {
                v[0][i] = start[i];
            }

            for (i = 1; i <= n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (i - 1 == j)
                    {
                        v[i][j] = pn + start[j];
                    }
                    else
                    {
                        v[i][j] = qn + start[j];
                    }
                }
            }

            // Find the initial function values
            for (j = 0; j <= n; j++)
            {
                f[j] = function.Value(v[j]);
            }

            if (IsOutputEnabled)
            {
                // Print out the initial values
                ConsoleEx.WriteLine("Initial Values:");
                for (j = 0; j <= n; j++)
                {
                    for (i = 0; i < n; i++)
                    {
                        ConsoleEx.WriteLine(v[j][i].ToString("F" + PRECISION) + " " +
                                          f[j].ToString("F" + PRECISION));
                    }
                }

                ConsoleEx.WriteLine();
            }

            // Begin the main loop of the minimization
            for (itr = 1; itr <= MAX_IT; itr++)
            {
                // Find the index of the largest value
                vg = 0;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] > f[vg])
                    {
                        vg = j;
                    }
                }

                // Find the index of the smallest value
                vs = 0;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] < f[vs])
                    {
                        vs = j;
                    }
                }

                // Find the index of the second largest value
                vh = vs;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] > f[vh] && f[j] < f[vg])
                    {
                        vh = j;
                    }
                }

                // Calculate the centroid
                for (j = 0; j <= n - 1; j++)
                {
                    cent = 0.0;
                    for (m = 0; m <= n; m++)
                    {
                        if (m != vg)
                        {
                            cent += v[m][j];
                        }
                    }
                    vm[j] = cent / n;
                }

                // Reflect vg to new vertex vr
                for (j = 0; j <= n - 1; j++)
                {
                    // vr[j] = (1+ALPHA)*vm[j] - ALPHA*v[vg][j];
                    vr[j] = vm[j] + ALPHA * (vm[j] - v[vg][j]);
                }
                
                fr = function.Value(vr);

                if (fr < f[vh] && fr >= f[vs])
                {
                    for (j = 0; j <= n - 1; j++)
                    {
                        v[vg][j] = vr[j];
                    }
                    f[vg] = fr;
                }

                // Investigate a step further in this direction
                if (fr < f[vs])
                {
                    for (j = 0; j <= n - 1; j++)
                    {
                        // ve[j] = GAMMA*vr[j] + (1-GAMMA)*vm[j];
                        ve[j] = vm[j] + GAMMA * (vr[j] - vm[j]);
                    }
                    
                    fe = function.Value(ve);

                    // By making fe < fr as opposed to fe < f[vs],
                    // Rosenbrocks function takes 63 iterations as opposed 
                    // to 64 when using double variables.
                    if (fe < fr)
                    {
                        for (j = 0; j <= n - 1; j++)
                        {
                            v[vg][j] = ve[j];
                        }
                        f[vg] = fe;
                    }
                    else
                    {
                        for (j = 0; j <= n - 1; j++)
                        {
                            v[vg][j] = vr[j];
                        }

                        f[vg] = fr;
                    }
                }

                // Check to see if a contraction is necessary
                if (fr >= f[vh])
                {
                    if (fr < f[vg] && fr >= f[vh])
                    {
                        // Perform outside contraction
                        for (j = 0; j <= n - 1; j++)
                        {
                            // vc[j] = BETA*v[vg][j] + (1-BETA)*vm[j];
                            vc[j] = vm[j] + BETA * (vr[j] - vm[j]);
                        }
                        
                        fc = function.Value(vc);
                    }
                    else {
                        // Perform inside contraction
                        for (j = 0; j <= n - 1; j++)
                        {
                            // vc[j] = BETA*v[vg][j] + (1-BETA)*vm[j];
                            vc[j] = vm[j] - BETA * (vm[j] - v[vg][j]);
                        }
                        
                        fc = function.Value(vc);
                    }


                    if (fc < f[vg])
                    {
                        for (j = 0; j <= n - 1; j++)
                        {
                            v[vg][j] = vc[j];
                        }

                        f[vg] = fc;
                    }

                    // At this point the contraction is not successful,
                    // we must halve the distance from vs to all the 
                    // vertices of the simplex and then continue.
                    else
                    {
                        for (row = 0; row <= n; row++)
                        {
                            if (row != vs)
                            {
                                for (j = 0; j <= n - 1; j++)
                                {
                                    v[row][j] = v[vs][j] + (v[row][j] - v[vs][j]) * SIGMA;
                                }
                            }
                        }
                        
                        f[vg] = function.Value(v[vg]);
                        f[vh] = function.Value(v[vh]);
                    }
                }

                // Print out the value at each iteration
                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    Console.WriteLine("Iteration {0}:", itr);
                    //for (j = 0; j <= n; j++)
                    //{
                    //    for (i = 0; i < n; i++)
                    //    {
                    //        ConsoleEx.WriteLine(v[j][i].ToString("F" + OutputPrecision) + " " +
                    //                          f[j].ToString("F" + OutputPrecision));
                    //    }
                    //}

                    ConsoleEx.WriteLine("Xc: " + vm.Format(PRECISION) + ", f(Xc): " + function.Value(vm).ToString("F" + PRECISION));
                }

                // Test for convergence
                fsum = 0.0;
                for (j = 0; j <= n; j++)
                {
                    fsum += f[j];
                }
                favg = fsum / (n + 1);
                s = 0.0;
                for (j = 0; j <= n; j++)
                {
                    s += Math.Pow((f[j] - favg), 2.0) / n;
                }
                s = Math.Sqrt(s);
                if (s < EPSILON) break;
            }
            // End main loop of the minimization

            // Find the index of the smallest value
            vs = 0;
            for (j = 0; j <= n; j++)
            {
                if (f[j] < f[vs])
                {
                    vs = j;
                }
            }

            if (IsOutputPerIterationEnabled && IsOutputEnabled)
            {
                ConsoleEx.WriteLine();
            }

            if (IsOutputEnabled)
            {
                var evaluations = function.Evaluations;
                var cachedCalls = function.CachedCalls;
                ConsoleEx.WriteLineGreen("Final position found. Returning value: x = " + v[vs].Format(PRECISION));
                ConsoleEx.WriteLineGreen("Function value of final position is: " + function.Value(v[vs]).ToString("F" + PRECISION));
                ConsoleEx.WriteLine("Number of algorithm iterations: " + itr);
                ConsoleEx.WriteLine("Number of function cached calls: " + cachedCalls);
                ConsoleEx.WriteLine("Number of function evaluations: " + evaluations);
                ConsoleEx.WriteLine();
            }

            return v[vs];
        }
    }
}
