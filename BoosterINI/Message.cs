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

        public static void ErrorMessageAdd(string text)
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0,16}", text);
            Console.ForegroundColor = color;
        }

        public static void ExcellentMessageAdd(string text)
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,16}", text);
            Console.ForegroundColor = color;
        }
    }
}