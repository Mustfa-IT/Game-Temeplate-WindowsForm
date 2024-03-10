using PoolGame.Shapes;
using PoolGame.utils;
public class Circle
{   
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; set; }
    public Color color { get; set; }
    public float radius { get; set; }

     public Circle(Vector2 position, Vector2 velocity, float mass, Color color, int radius)
    {
        Position = position;
        Velocity = velocity;
        Mass = mass;
        this.color = color;
        this.radius = radius;
    }

     public void ResolveCollision(Circle other)
        {
            // Step 1: Find unit normal and unit tangent vectors
            Vector2 normalVector = other.Position - Position;
            Vector2 unitNormal = normalVector.Normalize();
            Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);

            // Step 2: Create initial velocity vectors
            Vector2 v1 = Velocity;
            Vector2 v2 = other.Velocity;

            // Step 3: Project velocity vectors onto normal and tangent vectors
            float v1n = Vector2.Dot(v1, unitNormal);
            float v1t = Vector2.Dot(v1, unitTangent);
            float v2n = Vector2.Dot(v2, unitNormal);
            float v2t = Vector2.Dot(v2, unitTangent);

            // Step 4: New tangential velocities remain unchanged
            float v1tAfter = v1t;
            float v2tAfter = v2t;

            // Step 5: Calculate new normal velocities
            float m1 = Mass;
            float m2 = other.Mass;
            float v1nAfter = (v1n * (m1 - m2) + 2 * m2 * v2n) / (m1 + m2);
            float v2nAfter = (v2n * (m2 - m1) + 2 * m1 * v1n) / (m1 + m2);

            // Step 6: Convert scalar velocities into vectors
            Vector2 v1nVector = unitNormal * v1nAfter;
            Vector2 v1tVector = unitTangent * v1tAfter;
            Vector2 v2nVector = unitNormal * v2nAfter;
            Vector2 v2tVector = unitTangent * v2tAfter;

            // Step 7: Update final velocity vectors
            Velocity = v1nVector + v1tVector;
            other.Velocity = v2nVector + v2tVector;
        }

    public void advanceBallPosition(float dt)
    {
        Position.X = Position.X + Velocity.X * dt;
        Position.Y= Position.Y + Velocity.Y * dt;
        
    }
    public void Draw(Graphics graphics)
    {
        Brush brush = new SolidBrush(color);
        for (int i = 0; i < 360; i++)
        {
            int x = (int)Math.Round(Math.Cos(i) * radius + Position.X);
            int y = (int)Math.Round(Math.Sin(i) * radius + Position.Y);
            graphics.FillRectangle(brush, x, y, 1, 1);
            
        }
    }
    public void DrawFill(Graphics graphics)
    {
        Brush brush = new SolidBrush(color);

        for (float x = Position.X - radius; x <= Position.X + radius; x++)
        {
            for (float y = Position.Y - radius; y <= Position.Y + radius; y++)
            {
                if (IsPointInCircle(x, y))
                {
                    graphics.FillRectangle(brush, x, y, 1, 1);
                }
            }
        }
    }
    public bool collide(Circle otherCircle)
    {
        float otherCenterX = otherCircle.Position.X;
        float otherCenterY = otherCircle.Position.Y;

        float dx = Position.X - otherCenterX;
        float dy = Position.Y - otherCenterY;
       
        float squareCenterDistance = dx * dx + dy * dy ;
        float radiiSum = radius + otherCircle.radius;
        return (squareCenterDistance <= (radiiSum * radiiSum));
    }
    public bool collide(Square other,out int sx,out int sy)
    {
       // Check collision with left and right walls
        if (Position.X - radius <= other.Left || Position.X + radius >= other.Right)
        {
            sx = -1;
            sy = 1;
            return true;
        }

        // Check collision with top and bottom walls
        if (Position.Y - radius <= other.Top || Position.Y + radius >= other.Bottom)
        {
            sx = 1;
            sy = -1;
            return true;
        }

            sx = 1;
            sy = 1;
        return false;
    }
    
    private bool IsPointInCircle(float x, float y)
    {
        float dx = x - Position.X;
        float dy = y - Position.Y;
        return dx * dx + dy * dy <= radius * radius;
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
        using (Graphics graphics = Graphics.FromImage(bitmap))
        {
            DrawFill(graphics);
        }
       
    }
}
