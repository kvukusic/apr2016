using System;
using APR.DZ1;

namespace APR.DZ5.Demo.Assignments
{
    public class Assignment3 : IAssignment
    {
        public void Run()
        {
            Matrix a = new Matrix("Input/M3_A.txt");
            Matrix b = new Matrix("Input/M3_B.txt");
            Matrix x = new Matrix("Input/M3_X.txt");

            DifferentialEquationSolverRunner runner = null;

            choose_method:

            Console.WriteLine("Choose [1] for Trapezodial method, [2] for Runge-Kutta 4th order method or [3] for both:");
            try
            {
                switch (Int32.Parse(Console.ReadLine()))
                {
                    case 1:
                        runner = new DifferentialEquationSolverRunner(new TrapezodialMethod());
                        break;
                    case 2:
                        runner = new DifferentialEquationSolverRunner(new RungeKuttaMethod());
                        break;
                    case 3:
                        runner = new DifferentialEquationSolverRunner(new RungeKuttaMethod(), new TrapezodialMethod());
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again with numbers 1, 2 or 3.");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                goto choose_method;
            }

            specify_tmax:

            int? tmax = null;
            Console.WriteLine("Enter t_max:");
            try
            {
                tmax = Int32.Parse(Console.ReadLine());
                if (tmax < 0)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    goto specify_tmax;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                goto specify_tmax;
            }

            specify_step:

            double? step = null;
            Console.WriteLine("Enter step value:");
            try
            {
                step = Double.Parse(Console.ReadLine());
                if (step < 0 || step > tmax)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    goto specify_step;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                goto specify_step;
            }

            specify_step_output:

            int? stepOutput = null;
            Console.WriteLine("Enter a number n so that every n-th iteration will be printed:");
            try
            {
                stepOutput = Int32.Parse(Console.ReadLine());
                if (stepOutput <= 0 || stepOutput > tmax/step)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    goto specify_step_output;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                goto specify_step_output;
            }

            Console.WriteLine("Solving linear system of differential equations x' = Ax + B where");
            Console.WriteLine("A:");
            a.WriteToConsole();
            Console.WriteLine("B:");
            b.WriteToConsole();
            Console.WriteLine("x(t = 0):");
            x.WriteToConsole();

            Console.WriteLine($"t_max = {tmax}");
            Console.WriteLine($"T = {step}");

            runner.StepOutput = stepOutput.Value;
            runner.Solve(a, b, x, tmax.Value, step.Value);
        }
    }
}