using PostCard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PostCard
{
    // класс отвечает за отображение частиц
    public class Particle
    {
        public ColorBuffer buffer = Singleton.GetColorBuffer();
        public Vector2 spownPoint;
        public Vector2 position;
        public ConsoleColor color;
        public Particle(Vector2 position = new Vector2(), ConsoleColor color = ConsoleColor.White)
        {
            this.spownPoint = position;
            this.position=position;
            this.color=color;
        }
        public Particle(Vector2 spownPoint, Vector2 position, ConsoleColor color)
        {
            this.spownPoint=spownPoint;
            this.position=position;
            this.color=color;
        }

        public void Respown() => position = spownPoint;
        public void Move(Vector2 position) => this.position += position;
        public void MoveX(float x) => position.X = x;
        public void MoveY(float y) => position.Y = y;
        // Factory Methid pattern отдает рандомный Particle
        public static Particle GetRanodmParticle(Vector2 spownPoint = new Vector2())
        {
            Random random= new Random();
            ConsoleColor tempColor = (ConsoleColor)(random.Next((int)ConsoleColor.DarkBlue, (int)ConsoleColor.White));
            Vector2 tempPos = new Vector2(random.Next(-100,200), random.Next(0,Console.BufferHeight-1));
            return new Particle(spownPoint, tempPos, tempColor);
        }
        // занесение в буффер
        public void Draw()
        {
            if (position.X*2<0 || position.Y < 0 || position.Y>=buffer.consoleColors.Length || position.X*2+2>= buffer.consoleColors[0].Length) return;
            buffer.consoleColors[(int)position.Y][(int)position.X*2] = color;
            buffer.consoleColors[(int)position.Y][(int)position.X*2+1] = color;
        }
    }
}
