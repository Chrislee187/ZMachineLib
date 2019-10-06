using System;
using System.Collections.Generic;

namespace ZTest
{
    public static class ConsoleX
    {
        private static readonly Stack<(ConsoleColor background, ConsoleColor foreground)> ColoursStack 
            = new Stack<(ConsoleColor background, ConsoleColor foreground)>();

        private static void PopColors()
        {
            var colours = ColoursStack.Pop();

            Console.BackgroundColor = colours.background;
            Console.ForegroundColor = colours.foreground;
        }

        private static void PushColors(ConsoleColor background, ConsoleColor foreground)
        {
            ColoursStack.Push((Console.BackgroundColor, Console.ForegroundColor));

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

        }

        public static void ColouredWriteLine(string text, ConsoleColor background, ConsoleColor foreground)
        {
            PushColors(background, foreground);
            Console.Write(text);
            PopColors();
            Console.WriteLine();
        }

    }
}