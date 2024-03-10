using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PoolGame.Shapes
{
    public class Square : IDrawable
    {

        private Point _centerPoint;
        private Point _pivot;
        private int _width;
        private int _height;
        private Color _color;
        private int _angle;
        private Point[] _corners;
        // Define properties with events for each field
        public float Left => CenterPoint.X - (Width / 2);
        public float Right => CenterPoint.X + (Width / 2);
        public float Top => CenterPoint.Y - (Height / 2);
        public float Bottom => CenterPoint.Y + (Height / 2);

        public Vector2 LeftVector;
        public Vector2 RightVector;
        public Vector2 TopVector;
        public Vector2 BottomVector;
        public Point CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                if (_centerPoint != value)
                {
                    _centerPoint = value;
                    OnPropertyChange(nameof(CenterPoint));
                }
            }
        }

        public Point Pivot
        {
            get { return _pivot; }
            set
            {
                if (_pivot != value)
                {
                    _pivot = value;
                    OnPropertyChange(nameof(Pivot));
                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChange(nameof(Width));
                }
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChange(nameof(Height));
                }
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChange(nameof(Color));
                }
            }
        }

        public int Angle
        {
            get { return _angle; }
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    OnPropertyChange(nameof(Angle));
                }
            }
        }
       

        public Point[] corners = new Point[4];
      

        public Square(Color color, Point centerPoint , Point pivot, int width, int height)
        {
            this._color = color;
            this._centerPoint = centerPoint;
            this._pivot = pivot;
            this._width = width;
            this._height = height;
            corners[0] = new Point(_centerPoint.X - (_width / 2), _centerPoint.Y - (_height / 2));//bottom left
            corners[1] = new Point(_centerPoint.X + (_width / 2), _centerPoint.Y - (_height / 2));//bottom right
            corners[2] = new Point(_centerPoint.X - (_width / 2), _centerPoint.Y + (_height / 2));//top left
            corners[3] = new Point(_centerPoint.X + (_width / 2), _centerPoint.Y + (_height / 2));//top right

            LeftVector = Vector2.Normalize(new Vector2(Left, CenterPoint.Y));
            RightVector = Vector2.Normalize(new Vector2(Right, CenterPoint.Y));
            TopVector = Vector2.Normalize(new Vector2(CenterPoint.X, Top));
            BottomVector = Vector2.Normalize(new Vector2(CenterPoint.X, Bottom));

        }

        public void OnPropertyChange(String propertyName) 
        {
            switch (propertyName) 
            {
                case nameof(CenterPoint):
                    // Calculate corner points based on the center point, width, and height
                    _pivot.X = _centerPoint.X;
                    _pivot.Y = _centerPoint.Y;

                    corners[0] = new Point(_centerPoint.X - (_width / 2), _centerPoint.Y - (_height / 2));
                    corners[1] = new Point(_centerPoint.X + (_width / 2), _centerPoint.Y - (_height / 2));
                    corners[2] = new Point(_centerPoint.X - (_width / 2), _centerPoint.Y + (_height / 2));
                    corners[3] = new Point(_centerPoint.X + (_width / 2), _centerPoint.Y + (_height / 2));
                    break;
            }
        }
        public bool collision(Square traget) 
        {
            int i = 0;
            foreach (Point p in traget.corners) 
            {
                if (corners[i].X == p.X) 
                {
                    return true;
                }
                if (corners[i].Y == p.Y)
                    return true;
                i++;
            }
            return false;
        }
        private void BoundaryFill(Bitmap bitmap, Point startPoint, Color fillColor, int tolerance)
        {
            Draw(bitmap);
            // Check if starting point is within bitmap bounds
            if (startPoint.X < 0 || startPoint.X >= bitmap.Width ||
                startPoint.Y < 0 || startPoint.Y >= bitmap.Height)
                return;

            // Get the target color at starting point
            Color targetColor = bitmap.GetPixel(startPoint.X, startPoint.Y);

            // If the target color is the same as the fill color, return
            if (targetColor == fillColor)
                return;

            Stack<Point> stack = new Stack<Point>();
            stack.Push(startPoint);

            while (stack.Count > 0)
            {
                Point point = stack.Pop();

                // Check if current point is within bitmap bounds
                if (point.X >= 0 && point.X < bitmap.Width &&
                    point.Y >= 0 && point.Y < bitmap.Height)
                {
                    // Get the color of the current point
                    Color currentColor = bitmap.GetPixel(point.X, point.Y);

                    // If the current color is similar to the target color
                    if (IsSimilar(currentColor, targetColor, tolerance))
                    {
                        // Fill the current pixel with the fill color
                        bitmap.SetPixel(point.X, point.Y, fillColor);

                        // Push neighboring pixels onto the stack
                        stack.Push(new Point(point.X + 1, point.Y));
                        stack.Push(new Point(point.X - 1, point.Y));
                        stack.Push(new Point(point.X, point.Y + 1));
                        stack.Push(new Point(point.X, point.Y - 1));
                    }
                }
            }
        }
        private static bool IsSimilar(Color source, Color target, int tolerance)
        {
            return Math.Abs(source.R - target.R) <= tolerance &&
                   Math.Abs(source.G - target.G) <= tolerance &&
                   Math.Abs(source.B - target.B) <= tolerance;
        }
       
       


        public void Draw(Graphics graphics)
        {
            PixelDraw.DrawMidPointLine(graphics, corners[0], corners[1], _color);
            PixelDraw.DrawMidPointLine(graphics, corners[1], corners[3], _color);
            PixelDraw.DrawMidPointLine(graphics, corners[3], corners[2], _color);
            PixelDraw.DrawMidPointLine(graphics, corners[2], corners[0], _color);
        }

        public void Draw(Bitmap bitmap)
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Draw(graphics);
            }

        }
        public void DrawFill(Bitmap bitmap)
        {
            BoundaryFill(bitmap,CenterPoint, Color, 10);
        }
        public void RotateAndDraw(Bitmap bitmap, int angle)
        {
            if (this._angle == angle)
                return;
            this._angle = angle;

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                RotateAndDraw(graphics,angle);
            }
        }
        
        public void RotateAndDraw(Graphics graphics, int angle)
        {
            // Clear the previous drawing
            graphics.Clear(Color.White);
            // Rotate the corner points of the square
            Rotate(angle);
            // Draw the rotated square
            Draw(graphics);
        }
        public void RotateAndDrawFill(Bitmap bitmap, int angle)
        {
            if (this._angle == angle)
                return;
            this._angle = angle;

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Clear the previous drawing
                graphics.Clear(Color.White);
                // Rotate the corner points of the square
                Rotate(angle);
                // Draw the rotated square
                Draw(bitmap);
                DrawFill(bitmap);
            }
        }
        
        public void Rotate(int angle) 
        {
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = RotatePoint(corners[i], _pivot, angle);
            }
            _centerPoint = RotatePoint(_centerPoint, _pivot, angle);
        }



        private Point RotatePoint(Point location, Point pivot, int angle)
        {
            // Shifting the pivot point to the origin 
            // and the given point accordingly 
            int x_shifted = location.X - pivot.X;
            int y_shifted = location.Y - pivot.Y;

            // Convert angle to radians
            double angleRadians = angle * Math.PI / 180.0;

            // Calculating the rotated point co-ordinates 
            // and shifting it back 
            int rotatedX = (int)Math.Round(x_shifted * Math.Cos(angleRadians) - y_shifted * Math.Sin(angleRadians)) + pivot.X;
            int rotatedY = (int)Math.Round(x_shifted * Math.Sin(angleRadians) + y_shifted * Math.Cos(angleRadians)) + pivot.Y;

            return new Point(rotatedX, rotatedY);
        }


    }
}
