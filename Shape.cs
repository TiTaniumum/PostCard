using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Xml.XPath;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Diagnostics;

namespace PostCard
{
    //поворот фигуры
    public enum ShapeType
    {
        Left, Right, Top, Bottom
    }
    public abstract class Shape
    {
        ColorBuffer colorBuffer;
        public Vector2 SpownPoint { get; set; }
        public Vector2 Position { get; set; }
        protected Vector2 Scale { get; set; }

        //фигура состоит из 2д точек 
        protected List<Vector2> Vertexes { get; set; }
        public ConsoleColor FigureColor { get; set; }
        //длинна и ширина фигуры
        protected Vector2 MinPos, MaxPos;
        private Motion puls;
        public Shape()
        {
            SpownPoint = new Vector2();
            Position = new Vector2();
            Scale = new Vector2(1, 1);
            Vertexes= new List<Vector2>();
            FigureColor = ConsoleColor.White;
            MinPos = new Vector2();
            MaxPos = new Vector2();
            colorBuffer = Singleton.GetColorBuffer();
            puls= new Motion();
            puls.Length = 5;
        }
        public Shape(Vector2 position, Vector2 scale, List<Vector2> vertexes)
        {
            this.Position = position;
            this.Scale = scale;
            this.Vertexes = vertexes;
            FigureColor = ConsoleColor.White;
            MinPos = new Vector2();
            MaxPos = new Vector2();
            colorBuffer = Singleton.GetColorBuffer();
            puls= new Motion();
            puls.Length = 5;
        }
        // алгоритм принадлежности точки к фигуре 
        // ссылка на страницу где описаны подобные алгоритмы 
        // https://ru.wikibooks.org/wiki/%D0%A0%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8_%D0%B0%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC%D0%BE%D0%B2/%D0%97%D0%B0%D0%B4%D0%B0%D1%87%D0%B0_%D0%BE_%D0%BF%D1%80%D0%B8%D0%BD%D0%B0%D0%B4%D0%BB%D0%B5%D0%B6%D0%BD%D0%BE%D1%81%D1%82%D0%B8_%D1%82%D0%BE%D1%87%D0%BA%D0%B8_%D0%BC%D0%BD%D0%BE%D0%B3%D0%BE%D1%83%D0%B3%D0%BE%D0%BB%D1%8C%D0%BD%D0%B8%D0%BA%D1%83
        public bool isPointInFigure(int x, int y)
        {
            bool result = false;

            for (int i = 0, j = Vertexes.Count - 1; i < Vertexes.Count; j = i++)
            {

                Vector2 temp1 = Vertexes[i] * Scale;
                Vector2 temp2 = Vertexes[j] * Scale;

                bool isYInRange1 = (temp1.Y < temp2.Y) && (temp1.Y <= y) && (y < temp2.Y);
                bool isYInRange2 = (temp1.Y > temp2.Y) && (temp2.Y <= y) && (y < temp1.Y);

                float lenghtY = (temp2.Y - temp1.Y);
                float DifferenceByX = x - temp1.X;
                float MultiplingPart1 = DifferenceByX * lenghtY;
                float lengthX = (temp2.X - temp1.X);
                float DifferenceByY = (y - temp1.Y);
                float MultiplingPart2 = lengthX * DifferenceByY;


                if ((isYInRange1 && (MultiplingPart1 >=MultiplingPart2))||(isYInRange2 && (MultiplingPart1 <= MultiplingPart2)))
                    {
                        result = !result;
                    }
            }
            return result;
        }
        //обновление длинны и ширины 
        public void UpdateMinMax()
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                MinPos = Vector2.Min(MinPos, new Vector2(Vertexes[i].X*Scale.X, Vertexes[i].Y*Scale.Y));
                MaxPos = Vector2.Max(MaxPos, new Vector2(Vertexes[i].X*Scale.X, Vertexes[i].Y*Scale.Y));
            }
        }
        // фигура вернется к изначальной точке
        public void Respown()
        {
            Position = SpownPoint;
        }

        public float Length { get { return Math.Abs(MaxPos.X - MinPos.X); } }
        public float GetHalfLengthX()
        {
            return Math.Abs((MaxPos.X - MinPos.X)/2);
        }
        public float getHalfHeightY()
        {
            return Math.Abs((MaxPos.Y - MinPos.Y)/2);
        }
        public void SetScale(float scale)
        {
            Scale = new Vector2(scale, scale);
            UpdateMinMax();
        }
        public void SetScaleX(float scale)
        {
            Scale = new Vector2(scale, Scale.Y);
            UpdateMinMax();
        }
        public void SetScaleY(float scale)
        {
            Scale = new Vector2(Scale.Y, scale);
            UpdateMinMax();
        }
        public void SetXYScale(float x, float y)
        {
            Scale = new Vector2(x, y);
            UpdateMinMax();
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
            UpdateMinMax();
        }
        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
            UpdateMinMax();
        }
        public void MoveX(float x) => Position += new Vector2(x, 0);
        public void MoveY(float y) => Position += new Vector2(0, y);
        public void Move(float x, float y) => Position += new Vector2(x, y);
        public void Move(Vector2 position) => Position += position;
        
        //занесение фигуры в буффер, аналог window.draw() в sfml
        public virtual void Draw(int offsetx = 0, int offsety = 0)
        {
            int xMaxLength = colorBuffer.consoleColors[0].Length;
            for (int i = (int)MinPos.Y; i <MaxPos.Y; i++)
            {
                for (int j = (int)MinPos.X; j < MaxPos.X; j++)
                {
                    int tempi = i+(int)Position.Y+offsety;
                    int tempj = j*2+(int)Position.X*2+offsetx;

                    if (tempi < 0 || tempi >= colorBuffer.consoleColors.Length || tempj <0 || tempj+2>=xMaxLength) continue;
                    if (isPointInFigure(j, i))
                    {
                        if (colorBuffer.consoleColors[tempi][tempj] == FigureColor) continue;
                        colorBuffer.consoleColors[tempi][tempj] = FigureColor;
                        colorBuffer.consoleColors[tempi][tempj+1] = FigureColor;
                    }
                }
            }
        }
        // вращение фигуры взят у chatGPT
        public void RotateFigure(double angle)
        {
            // Calculate sine and cosine of the angle
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            // Define the rotation matrix
            double[,] rotationMatrix = { { cos, -sin }, { sin, cos } };

            // Iterate through each point and apply the rotation matrix
            for (int i = 0; i < Vertexes.Count; i++)
            {
                double x = Vertexes[i].X*Scale.X;
                double y = Vertexes[i].Y*Scale.Y;

                // Apply the rotation matrix
                double newX = x * rotationMatrix[0, 0] + y * rotationMatrix[0, 1];
                double newY = x * rotationMatrix[1, 0] + y * rotationMatrix[1, 1];

                // Update the point's coordinates
                Vertexes[i] = new Vector2((float)newX/Scale.X, (float)newY/Scale.Y);
            }
            UpdateMinMax();
        }
        //метод пульсации фигуры. меняется скейл
        public void Pulsation(float pulsationPower = 0.5f)
        {
            if (puls.isRightMotion())
            {
                Scale+= new Vector2(pulsationPower,pulsationPower);
            }
            else
            {
                Scale-= new Vector2(pulsationPower, pulsationPower);
            }
            UpdateMinMax();
        }
        protected void InitPart()
        {
            MinPos.X = Vertexes[0].X;
            MinPos.Y = Vertexes[0].Y;
            MaxPos.X = Vertexes[0].X;
            MaxPos.Y = Vertexes[0].Y;
            UpdateMinMax();
        }
    }

    public class Square : Shape
    {
        public Square() : base(new Vector2(), new Vector2(), new List<Vector2> { new Vector2(0, 0), new Vector2(3, 0), new Vector2(3, 3), new Vector2(0, 3) })
        {
            InitPart();
        }
        public Square(ConsoleColor color) : this()
        {
            FigureColor=color;
        }
    }
    public class Heart : Shape
    {
        public Heart() : base()
        {

            Vertexes.Add(new Vector2(0, 1));
            Vertexes.Add(new Vector2(1, 0));
            Vertexes.Add(new Vector2(2, 0));
            Vertexes.Add(new Vector2(3, 1));
            Vertexes.Add(new Vector2(4, 0));
            Vertexes.Add(new Vector2(5, 0));
            Vertexes.Add(new Vector2(6, 1));
            Vertexes.Add(new Vector2(6, 2));
            Vertexes.Add(new Vector2(3, 5));
            Vertexes.Add(new Vector2(0, 2));

            InitPart();
        }
        public Heart(ConsoleColor color) : this()
        {
            FigureColor= color;
        }
    }

    public class Pentagon : Shape
    {
        public Pentagon() : base()
        {
            Vertexes.Add(new Vector2(0, 2));
            Vertexes.Add(new Vector2(1, 0));
            Vertexes.Add(new Vector2(3, 0));
            Vertexes.Add(new Vector2(4, 2));
            Vertexes.Add(new Vector2(2, 4));

            InitPart();
        }
        public Pentagon(ConsoleColor color) : this()
        {
            FigureColor= color;
        }
    }

    public class CursedSquare : Shape
    {
        public CursedSquare() : base()
        {
            Vertexes.Add(new Vector2(0, 0));
            Vertexes.Add(new Vector2(1, 1));
            Vertexes.Add(new Vector2(1, 0));
            Vertexes.Add(new Vector2(0, 1));

            InitPart();
        }
        public CursedSquare(ConsoleColor color) : this()
        {
            FigureColor= color;
        }
    }

    public class Triangle : Shape
    {
        public Triangle(ShapeType shapeType = ShapeType.Bottom) : base()
        {
            InitType(shapeType);
            InitPart();
        }
        private void InitType(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Top:
                    Vertexes.Add(new Vector2(0, 0));
                    Vertexes.Add(new Vector2(2, 0));
                    Vertexes.Add(new Vector2(1, 2));
                    break;
                case ShapeType.Bottom:
                    Vertexes.Add(new Vector2(0, 1));
                    Vertexes.Add(new Vector2(1, 0));
                    Vertexes.Add(new Vector2(2, 1));
                    break;
                case ShapeType.Left:
                    Vertexes.Add(new Vector2(0, 0));
                    Vertexes.Add(new Vector2(1, 1));
                    Vertexes.Add(new Vector2(0, 2));
                    break;
                case ShapeType.Right:
                    Vertexes.Add(new Vector2(1, 0));
                    Vertexes.Add(new Vector2(0, 1));
                    Vertexes.Add(new Vector2(1, 2));
                    break;
                default: throw new Exception("something went Wrong");
            }
        }
        public Triangle(ConsoleColor color, ShapeType shapeType = ShapeType.Bottom) : this(shapeType)
        {
            FigureColor = color;
        }
    }
    public class Star : Shape
    {
        public Star(ConsoleColor color = ConsoleColor.Magenta)
        {
            Vertexes.Add(new Vector2(0,2));
            Vertexes.Add(new Vector2(2,2));
            Vertexes.Add(new Vector2(3,0));
            Vertexes.Add(new Vector2(4,2));
            Vertexes.Add(new Vector2(6,2));
            Vertexes.Add(new Vector2(4,3));
            Vertexes.Add(new Vector2(5,5));
            Vertexes.Add(new Vector2(3,4));
            Vertexes.Add(new Vector2(1,5));
            Vertexes.Add(new Vector2(2,3));
            FigureColor  = color;
            InitPart();
        }
    }
}
