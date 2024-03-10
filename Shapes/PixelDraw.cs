using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame.Shapes
{
    public static class PixelDraw
    {
        public static void DrawMidPointLine(Graphics g, Point p1, Point p2, Color color)
        {
            /*
             Δx = AbsoluteValue(p2.X - p1.X)
             Δy = AbsoluteValue(p2.Y - p1.Y)
    
             sx = If p1.X < p2.X Then 1 Else -1
             sy = If p1.Y < p2.Y Then 1 Else -1
    
            interchange = 0
            newPoint = Point(p1.X, p1.Y)

            If Δx < Δy Then
                Swap Δx and Δy
                interchange = 1
            ELse
                interchange = 0
            End if
            d = 2Δy - Δx
            deltaD = 2(Δy - Δx)
            SetPixel(newPoint)
    
             For i = 1 To Δx
                If d < 0 Then
                    If interchange Then
                        newPoint.Y += sy
                    Else
                        newPoint.X += sx
                    End If
                d += 2Δy
                Else
                    newPoint.X += sx
                    newPoint.Y += sy
                    d += deltaD
                End If
                SetPixel(newPoint)
             End For
             */
            int dx = Math.Abs(p2.X - p1.X);
            int dy = Math.Abs(p2.Y - p1.Y);

            int sx = p1.X < p2.X ? 1 : -1;
            int sy = p1.Y < p2.Y ? 1 : -1;
            bool interchange;
            Point newPoint = new Point(p1.X, p1.Y);

            if (dx < dy)
            {
                (dy, dx) = (dx, dy);
                interchange = true;
            }
            else
            {
                interchange = false;
            }
            int d = (2 * dy) - dx;
            int deltaD = 2 * (dy - dx);
            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, newPoint.X, newPoint.Y, 1, 1);

            for (int i = 1; i <= dx; i++)
            {

                if (d < 0)
                {
                    if (!interchange)
                    {
                        newPoint.X += sx;

                    }
                    else
                    {
                        newPoint.Y += sy;
                    }
                    d += 2 * dy;
                }
                else
                {
                    newPoint.X += sx;
                    newPoint.Y += sy;
                    d += deltaD;
                }
                g.FillRectangle(brush, newPoint.X , newPoint.Y,1,1);
            }
        }
        public static void DrawMidPointLine(Bitmap bitmap, Point p1, Point p2, Color color)
        {
            /*
             Δx = AbsoluteValue(p2.X - p1.X)
             Δy = AbsoluteValue(p2.Y - p1.Y)
    
             sx = If p1.X < p2.X Then 1 Else -1
             sy = If p1.Y < p2.Y Then 1 Else -1
    
            interchange = 0
            newPoint = Point(p1.X, p1.Y)

            If Δx < Δy Then
                Swap Δx and Δy
                interchange = 1
            ELse
                interchange = 0
            End if
            d = 2Δy - Δx
            deltaD = 2(Δy - Δx)
            SetPixel(newPoint)
    
             For i = 1 To Δx
                If d < 0 Then
                    If interchange Then
                        newPoint.Y += sy
                    Else
                        newPoint.X += sx
                    End If
                d += 2Δy
                Else
                    newPoint.X += sx
                    newPoint.Y += sy
                    d += deltaD
                End If
                SetPixel(newPoint)
             End For
             */
            int dx = Math.Abs(p2.X - p1.X);
            int dy = Math.Abs(p2.Y - p1.Y);

            int sx = p1.X < p2.X ? 1 : -1;
            int sy = p1.Y < p2.Y ? 1 : -1;
            bool interchange;
            Point newPoint = new Point(p1.X, p1.Y);

            if (dx < dy)
            {
                (dy, dx) = (dx, dy);
                interchange = true;
            }
            else
            {
                interchange = false;
            }
            int d = (2 * dy) - dx;
            int deltaD = 2 * (dy - dx);
            bitmap.SetPixel(newPoint.X,newPoint.Y,color);

            for (int i = 1; i <= dx; i++)
            {

                if (d < 0)
                {
                    if (!interchange)
                    {
                        newPoint.X += sx;

                    }
                    else
                    {
                        newPoint.Y += sy;
                    }
                    d += 2 * dy;
                }
                else
                {
                    newPoint.X += sx;
                    newPoint.Y += sy;
                    d += deltaD;
                }
                bitmap.SetPixel(newPoint.X, newPoint.Y, color);
            }
        }

    }
}
