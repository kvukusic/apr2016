using System;
using APR.DZ2.Functions;

namespace APR.DZ3.Demo.Assignments
{
    public class Assignment3 : IAssignment
    {
        public void Run()
        {
            var method = new BoxMinimizer();
            for (int i = 0; i < 2 /*Use the dimension of the function instead*/; i++)
            {
                method.AddExplicitConstraint(i, -100, 100);
            }

            method.AddImplicitConstraint(new Constraint(x => x[0] - x[1], d => d <= 0));
            method.AddImplicitConstraint(new Constraint(x => x[0] - 2, d => d <= 0));

            method.Minimize(new F1(), new double[] { -1.9, 2.0 });
            method.Minimize(new F2(), new double[] { 0.1, 0.3 });
        }
    }
}