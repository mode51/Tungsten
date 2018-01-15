using System;

namespace W.Demo
{
    public static class ConsoleExtensions
    {
        public static void WriteFullConsoleLine(this string message, int verticalOffset = -1)
        {
            int width = Console.BufferWidth;
            int lineCount = (message.Length / width) + 1;
            int index = 0;
            while (index < message.Length)
            {
                var length = Math.Min(width, message.Length - index);
                var line = message.Substring(index, length);
                line = line.PadRight(width);
                if (verticalOffset >= 0)
                    Console.SetCursorPosition(0, verticalOffset);
                Console.Write(line);
                index += length;
            }
        }
    }
}
