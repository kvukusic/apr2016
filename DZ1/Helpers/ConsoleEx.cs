using System;

namespace System
{
    public static class ConsoleEx
    {
        public static void Write(String message)
        {
            Console.Write(message);
        }

        public static void WriteLine(String message = null)
        {
            Console.WriteLine(message);
        }

        public static void WriteLineRed(String message = null)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        public static void WriteLineGreen(String message = null)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        public static void WriteLineYellow(String message = null)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}