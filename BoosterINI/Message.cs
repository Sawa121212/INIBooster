using System;

namespace BoosterINI
{
    public static class Message
    {
        public static void ErrorMessage(string text)
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = color;
        }

        public static void ExcellentMessage(string text)
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = color;
        }
    }
}