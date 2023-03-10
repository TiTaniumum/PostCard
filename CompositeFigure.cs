using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PostCard
{
    public struct Motion
    {
        private uint Current { get; set; }
        public uint Length { get; set; }

        private bool isRight;

        public bool isRightMotion()
        {
            switch (isRight)
            {
                case true:
                    if (Current >= Length) isRight = false;
                    else ++Current;
                    break;
                case false:
                    if (Current <= 0) isRight = true;
                    else --Current;
                    break;
            }
            return isRight;
        }
    }
    public class CompositeFigure
    {
        protected Vector2 Position;
        protected List<Shape> compositeFigure;
        public Motion LRMotion;
        public Motion UDMotion;

        public CompositeFigure()
        {
            compositeFigure = new List<Shape>();
            LRMotion = new Motion();
            UDMotion = new Motion();
            LRMotion.Length = 50;
            UDMotion.Length = 50;
        }

        public void Draw()
        {
            foreach (Shape shape in compositeFigure)
            {
                shape.Draw((int)Position.X, (int)Position.Y);
            }
        }
        public void MoveX(float x) => Position.X += x;
        public void MoveY(float y) => Position.Y += y;
        public void Move(float x, float y) => Position += new Vector2(x, y);
        public void Move(Vector2 position) => Position += position;
        public void AddShape(Shape shape) => compositeFigure.Add(shape);
        public void SetScaleX(float scale)
        {
            foreach (var item in compositeFigure)
            {
                item.SetScaleX(scale);
            }
        }
        public void SetScaleY(float scale)
        {
            foreach (var item in compositeFigure)
            {
                item.SetScaleY(scale);
            }
        }
        public void SetScale(float scale)
        {
            foreach (var item in compositeFigure)
            {
                item.SetScale(scale);
            }
        }
        public void SetScale(float scaleX,float scaleY)
        {
            foreach (var item in compositeFigure)
            {
                item.SetXYScale(scaleX, scaleY);
            }
        }
        public void LeftRighMotion(float offesetLength = 1)
        {
            if (LRMotion.isRightMotion())
            {
                MoveX(offesetLength);
            }
            else{
                MoveX(-offesetLength);
            }
        }
    }
    public class TriangleRow : CompositeFigure
    {
        public TriangleRow(ConsoleColor color = ConsoleColor.Yellow, ShapeType shapeType = ShapeType.Bottom) : base()
        {
            for (int i = 0; i < 10; i++)
            {
                compositeFigure.Add(new Triangle(color, shapeType));
            }
            int offsetX = 0;
            foreach (var item in compositeFigure)
            {
                item.SetScale(10);
                item.MoveX(offsetX);
                offsetX += 25;
            }
        }
    }
}
