using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;
using APR.DZ1.Extensions;

namespace APR.DZ2
{
    public class NelderMeadSearch : IMinimizer
    {
        private const int OutputPrecision = 6;

        private readonly int MAX_IT = 1000;

        private readonly double EPSILON = 10e-6;
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

                Console.WriteLine();
                Console.WriteLine("****************************************************************");
                Console.WriteLine("Starting minimization with Nelder Mead Simplex Search method...");

                Console.WriteLine();

            }

            int vs;         /* vertex with smallest value */
            int vh;         /* vertex with next smallest value */
            int vg;         /* vertex with largest value */

            int i, j, m, row;
            int itr;          /* track the number of iterations */

            double[][] v;     /* holds vertices of simplex */
            double pn, qn;   /* values used to create initial simplex */
            double[] f;      /* value of function at each vertex */
            double fr;      /* value of function at reflection point */
            double fe;      /* value of function at expansion point */
            double fc;      /* value of function at contraction point */
            double[] vr;     /* reflection - coordinates */
            double[] ve;     /* expansion - coordinates */
            double[] vc;     /* contraction - coordinates */
            double[] vm;     /* centroid - coordinates */
            double min;

            double fsum, favg, s, cent;

            // Dimension
            int n = start.Length;

            /* dynamically allocate arrays */

            /* allocate the rows of the arrays */
            v = new double[n+1][];
            f = new double[n+1];
            vr = new double[n];
            ve = new double[n];
            vc = new double[n];
            vm = new double[n];

            /* allocate the columns of the arrays */
            for (i = 0; i <= n; i++)
            {
                v[i] = new double[n];
            }

            /* create the initial simplex */
            /* assume one of the vertices is 0,0 */
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

            /* find the initial function values */
            for (j = 0; j <= n; j++)
            {
                f[j] = function.Value(v[j]);
            }

            if (IsOutputEnabled)
            {
                /* print out the initial values */
                Console.WriteLine("Initial Values:");
                for (j = 0; j <= n; j++)
                {
                    for (i = 0; i < n; i++)
                    {
                        Console.WriteLine(v[j][i].ToString("F" + OutputPrecision) + " " +
                                          f[j].ToString("F" + OutputPrecision));
                    }
                }

                Console.WriteLine();
            }

            /* begin the main loop of the minimization */
            for (itr = 1; itr <= MAX_IT; itr++)
            {
                /* find the index of the largest value */
                vg = 0;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] > f[vg])
                    {
                        vg = j;
                    }
                }

                /* find the index of the smallest value */
                vs = 0;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] < f[vs])
                    {
                        vs = j;
                    }
                }

                /* find the index of the second largest value */
                vh = vs;
                for (j = 0; j <= n; j++)
                {
                    if (f[j] > f[vh] && f[j] < f[vg])
                    {
                        vh = j;
                    }
                }

                /* calculate the centroid */
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

                /* reflect vg to new vertex vr */
                for (j = 0; j <= n - 1; j++)
                {
                    /*vr[j] = (1+ALPHA)*vm[j] - ALPHA*v[vg][j];*/
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

                /* investigate a step further in this direction */
                if (fr < f[vs])
                {
                    for (j = 0; j <= n - 1; j++)
                    {
                        /*ve[j] = GAMMA*vr[j] + (1-GAMMA)*vm[j];*/
                        ve[j] = vm[j] + GAMMA * (vr[j] - vm[j]);
                    }
                    
                    fe = function.Value(ve);

                    /* by making fe < fr as opposed to fe < f[vs], 			   
                       Rosenbrocks function takes 63 iterations as opposed 
                       to 64 when using double variables. */

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

                /* check to see if a contraction is necessary */
                if (fr >= f[vh])
                {
                    if (fr < f[vg] && fr >= f[vh])
                    {
                        /* perform outside contraction */
                        for (j = 0; j <= n - 1; j++)
                        {
                            /*vc[j] = BETA*v[vg][j] + (1-BETA)*vm[j];*/
                            vc[j] = vm[j] + BETA * (vr[j] - vm[j]);
                        }
                        
                        fc = function.Value(vc);
                    }
                    else {
                        /* perform inside contraction */
                        for (j = 0; j <= n - 1; j++)
                        {
                            /*vc[j] = BETA*v[vg][j] + (1-BETA)*vm[j];*/
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
                    /* at this point the contraction is not successful,
                       we must halve the distance from vs to all the 
                       vertices of the simplex and then continue.
                       10/31/97 - modified to account for ALL vertices. 
                    */
                    else
                    {
                        for (row = 0; row <= n; row++)
                        {
                            if (row != vs)
                            {
                                for (j = 0; j <= n - 1; j++)
                                {
                                    v[row][j] = v[vs][j] + (v[row][j] - v[vs][j]) / 2.0;
                                }
                            }
                        }
                        
                        f[vg] = function.Value(v[vg]);
                        f[vh] = function.Value(v[vh]);
                    }
                }

                /* print out the value at each iteration */
                if (IsOutputPerIterationEnabled && IsOutputEnabled)
                {
                    Console.WriteLine("Iteration {0}:", itr);
                    //for (j = 0; j <= n; j++)
                    //{
                    //    for (i = 0; i < n; i++)
                    //    {
                    //        Console.WriteLine(v[j][i].ToString("F" + OutputPrecision) + " " +
                    //                          f[j].ToString("F" + OutputPrecision));
                    //    }
                    //}

                    Console.WriteLine("Xc: " + vm.Format(OutputPrecision) + ", f(Xc): " + function.Value(vm).ToString("F" + OutputPrecision));
                }

                /* test for convergence */
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
            /* end main loop of the minimization */

            /* find the index of the smallest value */
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
                Console.WriteLine();
            }

            if (IsOutputEnabled)
            {
                var evaluations = function.Evaluations;
                var cachedCalls = function.CachedCalls;
                Console.WriteLine("Final position found. Returning value: x = " + v[vs].Format(OutputPrecision));
                Console.WriteLine("Function value of final position is: " + function.Value(v[vs]).ToString("F" + OutputPrecision));
                Console.WriteLine("Number of algorithm iterations: " + itr);
                Console.WriteLine("Number of function cached calls: " + cachedCalls);
                Console.WriteLine("Number of function evaluations: " + evaluations);
                Console.WriteLine();
            }

            return v[vs];
        }
    }
}
