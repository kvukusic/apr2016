using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3.Demo.Assignments
{
    public class Assignment4 : IAssignment
    {
        public void Run()
        {
            var method = new UnconstrainedMixedMinimizer();
            method.Minimizer = new HookeJeevesMinimizer();

            method.AddImplicitConstraint(new Constraint(x => x[1] - x[0], d => d >= 0));
            method.AddImplicitConstraint(new Constraint(x => 2 - x[0], d => d >= 0));

            method.Minimize(new F1(), new double[] {-1.9, 2.0});
            method.Minimize(new F2(), new double[] {0.1, 0.3});
        }
    }
}