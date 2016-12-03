using System;

namespace APR.DZ3.Demo
{
    public class Demo
    {
        public static void Main(String[] args)
        {
            while(true)
            {
                Console.WriteLine("================================");
                Console.WriteLine("Enter assignment number [1 - 5]:");
                int identifier = -1;
                Int32.TryParse(Console.ReadLine(), out identifier);

                IAssignment assignment = AssignmentFactory.FromNumber(identifier);
                if(assignment != null)
                {
                    assignment.Run();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Assignment with specified identifier not found: " + identifier);
                    Console.WriteLine("Please try again.");
                }

                Console.WriteLine();
            }
        }
    }
}