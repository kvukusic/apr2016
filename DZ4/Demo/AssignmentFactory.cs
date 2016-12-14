using System;
using APR.DZ4.Demo;

namespace APR.DZ4.Demo
{
    public static class AssignmentFactory
    {
        public static IAssignment FromNumber(int identifier)
        {
            switch (identifier)
            {
                case 1:
                    return new Assignments.Assignment1();
                case 2:
                    return new Assignments.Assignment2();
                case 3:
                    return new Assignments.Assignment3();
                case 4:
                    return new Assignments.Assignment4();
                case 5:
                    return new Assignments.Assignment5();
                default:
                    return null;
            }
        }
    }
}