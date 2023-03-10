using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Открытка;

namespace PostCard
{
    public class MainClass
    {
        public static int height, width;
        public static ColorBuffer buffer;
        static MainClass()
        {
            SetConsoleFullScreen();
            //height = 50;
            //width = 200;
            height = Console.WindowHeight-1;
            width = Console.WindowWidth;
            buffer = Singleton.GetColorBuffer();
        }
        public static void Act1()
        {
            Random random = new Random();
            List<Shape> act1 = new List<Shape>();
            List<Particle> particles = new List<Particle>();
            for (int i = 0; i < 100; i++)
            {
                particles.Add(Particle.GetRanodmParticle(new Vector2(random.Next(-100, -50), random.Next(0, Console.WindowHeight-1))));
            }
            for (int i = 0; i < 3; i++)
            {
                act1.Add(new Heart(ConsoleColor.Red));
                act1[i].SetScale(random.Next(6, 10));
                int x = random.Next(-100, -50);
                int y = random.Next(0, 50);
                act1[i].SetPosition(x+random.Next(0, 200), y);
                act1[i].SpownPoint = new Vector2(x, y);
            }
            int length1 = act1.Count;
            for (int i = length1; i < length1+5; i++)
            {
                act1.Add(new Star());
                act1[i].SetScale(random.Next(3, 6));
                int x = random.Next(-100, -50);
                int y = random.Next(0, 50);
                act1[i].SetPosition(x+random.Next(0, 200), y);
                act1[i].SpownPoint = new Vector2(x, y);
            }
            TriangleRow row1 = new TriangleRow(ConsoleColor.Yellow, ShapeType.Top);
            TriangleRow row2 = new TriangleRow(ConsoleColor.Yellow, ShapeType.Bottom);
            row1.MoveX(-100);
            row2.MoveX(-100);
            row1.SetScale(13, 3);
            row2.SetScale(18, 8);
            row2.MoveY(41);

            int iteration = 0;
            int endIteration = 450;
            while (iteration < endIteration)
            {
                buffer.Display();
                buffer.UpdateBuffer();

                for (int i = 0; i < act1.Count; i++)
                {
                    act1[i].Draw();
                    if (act1[i].Position.X >200) act1[i].Respown();
                    act1[i].MoveX(1);
                    act1[i].RotateFigure(0.05);
                }
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Draw();
                    if (particles[i].position.X>150) particles[i].Respown();
                    particles[i].Move(new Vector2(1, random.Next(-1, 2)));
                }
                row1.Draw();
                row2.Draw();
                row1.LeftRighMotion(1);
                row2.LeftRighMotion(1);
                ++iteration;
            }
        }
        public static void Act2()
        {
            //Console.Clear();
            int centerY = height/ 2;
            int centerX = width/ 2;
            List<Shape> act2 = new List<Shape>();
            int colorLenght = Enum.GetNames(typeof(ConsoleColor)).Length;
            int currentColor = 0;
            for (int i = 0; i < 54; i++)
            {
                act2.Add(new Star((ConsoleColor)currentColor));
                act2[i].SetScale(54-i);
                act2[i].SetPosition(centerX-act2[i].GetHalfLengthX()-50, centerY - act2[i].getHalfHeightY());
                act2[i].FigureColor = (ConsoleColor)(currentColor%colorLenght);
                foreach (var item in act2)
                {
                    item.Draw();
                }
                buffer.Display();
                buffer.UpdateBuffer();
                ++currentColor;
            }

            int iteration = 0;
            int endIteration = 15;
            while (iteration <endIteration)
            {
                ConsoleColor tempColor = act2[0].FigureColor;
                for (int i = 0, j = i+1; j < act2.Count; ++j, ++i)
                {
                    act2[i].FigureColor = act2[j].FigureColor;
                    act2[i].Draw();
                }
                act2[act2.Count-1].FigureColor = tempColor;
                act2[act2.Count-1].Draw();

                buffer.Display(); buffer.UpdateBuffer();
                ++iteration;
            }
            while (act2.Count > 0)
            {
                act2.Remove(act2[0]);
                foreach (var item in act2)
                {
                    item.Draw();
                }
                buffer.Display(); buffer.UpdateBuffer();
            }
        }

        public static void Act3()
        {
            int centerY = height/ 2;
            int centerX = width/ 2;
            List<Shape> act3 = new List<Shape>();
            int currentColor = 0;
            int colorLenght = Enum.GetNames(typeof(ConsoleColor)).Length;
            for (int i = 0; i < 54; i++)
            {
                act3.Add(new Heart((ConsoleColor)currentColor));
                act3[i].SetScale(1+i);
                act3[i].SetPosition(centerX-act3[i].GetHalfLengthX()-50, centerY - act3[i].getHalfHeightY());
                act3[i].FigureColor = (ConsoleColor)(currentColor%colorLenght);
                for (int j = act3.Count - 1; j >= 0; --j)
                {
                    act3[j].Draw();
                }
                buffer.Display();
                buffer.UpdateBuffer();
                ++currentColor;
            }
            int iteration = 0;
            int endIteration = 15;
            while (iteration <endIteration)
            {
                ConsoleColor tempColor = act3[act3.Count-1].FigureColor;
                for (int i = act3.Count-1, j = i-1; j >= 0; --j, --i)
                {
                    act3[i].FigureColor = act3[j].FigureColor;
                    act3[i].Draw();
                }
                act3[0].FigureColor = tempColor;
                act3[0].Draw();

                buffer.Display(); buffer.UpdateBuffer();
                ++iteration;
            }
            while (act3.Count > 0)
            {
                act3.Remove(act3[act3.Count-1]);
                for (int i = act3.Count - 1; i >= 0; i--)
                {
                    act3[i].Draw();
                }
                buffer.Display(); buffer.UpdateBuffer();
            }
        }
        public static void Act4()
        {
            //Console.Clear();
            Random random = new Random();
            List<Particle> particles = new List<Particle>();
            for (int i = 0; i < 200; i++)
            {
                particles.Add(Particle.GetRanodmParticle(new Vector2(random.Next(0, Console.WindowWidth), random.Next(-40, -5))));
            }
            List<Shape> act4 = new List<Shape>();
            int centerX = width/2;
            int centerY = height/2;
            for (int i = 0; i < 4; i++)
            {
                if (i%2==0) act4.Add(new Heart(ConsoleColor.DarkRed));
                else act4.Add(new Heart(ConsoleColor.Red));
                act4[i].SetScale(0.5f+(i+1)%2);
                if (i<2) act4[i].SetPosition(centerX-act4[i].GetHalfLengthX()-25, centerY - act4[i].getHalfHeightY());
                else act4[i].SetPosition(centerX-act4[i].GetHalfLengthX()-85, centerY - act4[i].getHalfHeightY());
            }
            act4.Add(new Square(ConsoleColor.DarkGray));
            act4[act4.Count-1].SetXYScale(10f, 3f);
            act4[act4.Count-1].SetPosition(centerX-act4[act4.Count-1].GetHalfLengthX()-53, centerY - act4[act4.Count-1].getHalfHeightY());
            act4.Add(new Square(ConsoleColor.Gray));
            act4[act4.Count-1].SetXYScale(9.1f, 2.3f);
            act4[act4.Count-1].SetPosition(centerX-act4[act4.Count-1].GetHalfLengthX()-53, centerY - act4[act4.Count-1].getHalfHeightY());

            int iteration = 0;
            int endIteration = 500;
            ConsoleColor oldColor = Console.ForegroundColor;
            ConsoleColor oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            while (iteration <endIteration)
            {
                foreach (var item in particles)
                {
                    item.Draw();
                    if (item.position.Y >70) item.Respown();
                    item.Move(new Vector2(random.Next(-1, 2), 1));
                }
                foreach (var item in act4)
                {
                    item.Draw();
                    if (item == act4[act4.Count-2]) continue;
                    if (item == act4[act4.Count-1]) break;
                    item.Pulsation(0.2f);
                }
                Console.SetCursorPosition((int)(centerX-act4[act4.Count-2].GetHalfLengthX())+4, (int)(centerY - act4[act4.Count-2].getHalfHeightY()+3));
                Console.Write("Some Text");
                Console.SetCursorPosition((int)(centerX-act4[act4.Count-2].GetHalfLengthX())+4, (int)(centerY - act4[act4.Count-2].getHalfHeightY()+5));
                Console.Write("Some Text");
                buffer.Display(); buffer.UpdateBuffer();
                ++iteration;
            }
            Console.ForegroundColor = oldColor;
            Console.BackgroundColor = oldBackColor;
        }

        public static void Act5()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.Clear();
            buffer.UpdateBuffer();
            List<Shape> act5 = new List<Shape>();
            act5.Add(new Square(ConsoleColor.DarkGray));
            act5.Add(new Square(ConsoleColor.Gray));
            act5[0].SetPosition(2, 2);
            act5[1].SetPosition(3, 3);
            act5[0].SetXYScale(30,15);
            act5[1].SetXYScale(29,14);
            act5[0].Draw();
            act5[1].Draw();

            buffer.Display(); buffer.UpdateBuffer();
            List<string> strings = new List<string>();
            //add text here
            strings.Add("Some Text");
            

            ConsoleColor oldColor = Console.ForegroundColor;
            ConsoleColor oldBackgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            for (int i = 0; i < strings.Count; i++)
            {
                Console.SetCursorPosition(10, 4+i);
                for (int j = 0; j < strings[i].Length; j++)
                {
                    Console.Write(strings[i][j]);
                    Thread.Sleep(50);
                }
            }
            Console.ForegroundColor= oldColor;
            Console.BackgroundColor= oldBackgroundColor;
        }
        public static void Main(string[] args)
        {
            buffer.UpdateBuffer();
            Console.CursorVisible = false;
            Singleton.GetColorBuffer().UpdateBufferSize();
            Act1();
            Act2();
            Act3();
            Act4();
            Act5();
            Console.ReadLine();
        }

        // Define the SetConsoleFullScreen function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void SetConsoleFullScreen()
        {
            // Get the handle to the console window
            IntPtr hWndConsole = GetConsoleWindow();

            // Get the size of the desktop
            RECT desktopRect;
            GetWindowRect(GetDesktopWindow(), out desktopRect);

            // Set the console window to fullscreen
            SetWindowPos(hWndConsole, IntPtr.Zero, desktopRect.Left, desktopRect.Top, desktopRect.Right, desktopRect.Bottom, 0x0040);

        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
    }
}