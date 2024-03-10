namespace PoolGame.utils
{
    public class Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public static Vector2 zero {get{return new Vector2(0,0);}}
    public static Vector2 one {get{return new Vector2(1,1);}}
     
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float Length()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }

    public Vector2 Normalize()
    {
        float magnitude = Length();
        return new Vector2(X / magnitude, Y / magnitude);
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }
    public static Vector2 operator +(float f1, Vector2 v1)
    {
        return new Vector2(v1.X + f1, v1.Y + f1);
    }
    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector2 operator *(Vector2 v, float scalar)
    {
        return new Vector2(v.X * scalar, v.Y * scalar);
    }
   
    public static Vector2 operator /(Vector2 v, float scalar)
    {
        if (scalar == 0)
            throw new ArgumentException("Cannot divide by zero");
        
        return new Vector2(v.X / scalar, v.Y / scalar);
    }
    public static Vector2 operator *(float scalar,Vector2 v)
    {
        return new Vector2(v.X * scalar, v.Y * scalar);
    }

    public static Vector2 operator /(float scalar,Vector2 v)
    {
        if (scalar == 0)
            throw new ArgumentException("Cannot divide by zero");
        
        return new Vector2(v.X / scalar, v.Y / scalar);
    }
    public static float Dot(Vector2 v1, Vector2 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    public static float Angle(Vector2 from, Vector2 to)
    {
        from = from.Normalize();
        to = to.Normalize();
        return (float)Math.Acos(Dot(from, to));
    }
}
}