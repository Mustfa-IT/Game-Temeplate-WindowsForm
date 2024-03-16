using PoolGame.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PoolGame.Shapes
{
    public class Plane2D : IDrawable
    {
        public Vector2 pos { set; get; }
        public Vector2 Normal { get; private set; }
        public float Distance { get; private set; }

        public Plane2D(Vector2 pos,Vector2 normal, float distance)
        {
            this.pos = pos;
            Normal = normal.Normalize();
            Distance = distance;
        }

        public void Draw(Bitmap bitmap)
        {
            PixelDraw.DrawMidPointLine(Graphics.FromImage(bitmap),new Point((int)(Normal.X + pos.X),(int)(Normal.Y + pos.Y)),new Point(( (int) (Normal.X * -Distance + pos.X) ), (int) ( Normal.Y * - Distance + pos.Y)),Color.Black);
        }
    }
}
