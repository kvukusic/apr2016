using System;
using APR.DZ2;
using APR.DZ2.Functions;

namespace APR.DZ3.Demo.Assignments
{
    public class Assignment5 : IAssignment
    {
        public void Run()
        {
            var method = new UnconstrainedMixedMethod();
            method.Minimizer = new NelderMeadMinimizer();

            method.AddImplicitConstraint(new Constraint(x => 3 - x[0] - x[1], d => d >= 0));
            method.AddImplicitConstraint(new Constraint(x => 3 + 1.5 * x[0] - x[1], d => d >= 0));
            method.AddImplicitConstraint(new Constraint(x => x[1] - 1, d => d == 0, true));

            method.Minimize(new F5(3, 0), new[] { 0.0, 0.0 });
            method.Minimize(new F5(3,0), new[] { 6.0, 7.0 });
        }
    }
}