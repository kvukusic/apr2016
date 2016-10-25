using System;

namespace APR.DZ1.Demo
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
                case 6:
                    return new Assignments.Assignment6();
                default:
                    return null;
            }
        }
    }
}