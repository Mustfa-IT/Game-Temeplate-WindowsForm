using PoolGame.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PoolGame.Shapes
{
    public class Line : IDrawable
    {
        public Vector2 start { get; set; }
        public Vector2 end { get; set; }

        public Line(Vector2 point1, Vector2 point2)
        {
            start = point1;
            end = point2;
        }

        public static Vector2 ClosestPointOnLineSegment(float lx1, float ly1,float lx2, float ly2, float x0, float y0)
        {
            float A1 = ly2 - ly1;
            float B1 = lx1 - lx2;
            float C1 = (ly2 - ly1) * lx1 + (lx1 - lx2) * ly1;
            float C2 = -B1 * x0 + A1 * y0;
            float det = A1 * A1 - -B1 * B1;
            float cx = 0;
            float cy = 0;

            if (det != 0)
            {
                cx = (float)((A1 * C1 - B1 * C2) / det);
                cy = (float)((A1 * C2 - -B1 * C1) / det);
            }
            else
            {
                cx = x0;
                cy = y0;
            }
            return new Vector2(cx, cy);
        }

        public void Draw(Bitmap bitmap)
        {
            PixelDraw.DrawMidPointLine(bitmap, new Point((int)start.X, (int)start.Y), new Point((int)end.X, (int)end.Y), Color.Black);
        }
    }
}
