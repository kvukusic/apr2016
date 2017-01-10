using System;

namespace APR.DZ5.Demo
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
                default:
                    return null;
            }
        }
    }
}