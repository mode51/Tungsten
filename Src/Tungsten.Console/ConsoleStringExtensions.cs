using System;

namespace W
{
    /// <summary>
    /// Extension methods related to the Console
    /// </summary>
    public static class ConsoleStringExtensions
    {
        /// <summary>
        /// Writes text to the console at the specified location (x,y)
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="x">The column on which to start writing text</param>
        /// <param name="y">The row on which to start writing text</param>
        /// <remarks>The Console origin (0,0) is top-left</remarks>
        public static void WriteToConsole(this string message, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(message);
        }
        /// <summary>
        /// Writes text to the console.  Columns which the text doesn't overwrite are filled with the specified padding character.
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="verticalOffset">The line (or row) on which to write the text.  0 is the top of the Console.</param>
        /// <param name="paddingChar">The padding character used to fill the unused portion of the line</param>
        public static void WriteFullConsoleLine(this string message, int verticalOffset = -1, char paddingChar = ' ')
        {
            int width = Console.BufferWidth;
            int lineCount = (message.Length / width) + 1;
            int index = 0;
            while (index < message.Length)
            {
                var length = Math.Min(width, message.Length - index);
                var line = message.Substring(index, length);
                line = line.PadRight(width, paddingChar);
                if (verticalOffset >= 0)
                    Console.SetCursorPosition(0, verticalOffset);
                Console.Write(line);
                index += length;
            }
        }
    }
}
