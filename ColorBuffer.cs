using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PostCard
{
    public class ColorBuffer
    {
        public ConsoleColor[][] consoleColors;
        public ConsoleColor[][] oldConsoleColor;
        public ColorBuffer()
        {
            UpdateBufferSize();
        }
        public void UpdateBufferSize()
        {
            consoleColors= new ConsoleColor[Console.WindowHeight-1][];
            oldConsoleColor = new ConsoleColor[Console.WindowHeight-1][];

            for (int i = 0; i <  consoleColors.Length; i++)
            {
                consoleColors[i] = new ConsoleColor[Console.WindowWidth];
                oldConsoleColor[i] = new ConsoleColor[Console.WindowWidth];
            }

            for (int i = 0; i < consoleColors.Length; i++)
            {
                for (int j = 0; j < consoleColors[i].Length; j++)
                {
                    consoleColors[i][j] = ConsoleColor.Black;
                    oldConsoleColor[i][j] = consoleColors[i][j];
                }
            }
        }
        public void UpdateBuffer()
        {
            for (int i = 0; i < consoleColors.Length; i++)
            {
                for (int j = 0; j < consoleColors[i].Length; j++)
                {
                    oldConsoleColor[i][j] = consoleColors[i][j];
                    consoleColors[i][j] = ConsoleColor.Black;
                }
            }
        }
        public void DrawAt(int x,int y)
        {
                Console.SetCursorPosition(x, y);
                Console.Write(" ");   
        }
        public void Display()
        {
            ConsoleColor oldColor = Console.BackgroundColor;
            for (int i = 0; i < consoleColors.Length; i++)
            {
                for (int j = 0; j < consoleColors[i].Length; j++)
                {
                    if (consoleColors[i][j] == oldConsoleColor[i][j]) continue;
                    if (Console.BackgroundColor != consoleColors[i][j]) Console.BackgroundColor = consoleColors[i][j];
                    DrawAt(j, i);
                }
            }
            Console.BackgroundColor = oldColor;
        }
    }
}
