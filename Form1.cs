using PoolGame.Shapes;
using PoolGame.utils;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PoolGame
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(float dt, float frameRate); // Delegate for safe cross-thread calls
        private Bitmap bitmap; // Bitmap for drawing on PictureBox

        // Thread Variables
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource for graceful thread termination
        private readonly Task updateTask; // Task for game update loop
        //Game Object
        Square outBound;
        Circle mainBall;
        private Circle otherBall;
        private Circle otherBall2;
        private Line line;
        // Game Variables
        private Point cPoint;
        private int speed; // Speed of movement

        private float horizontalDirection = 0.0f; // Horizontal direction of movement
        private float verticalDirection = 0.0f; // Vertical direction of movement

        // Declare variables to track mouse coordinates
        private bool isDragging = false;
        private Vector2 dragStartPoint;
        private Vector2 dragEndPoint;

        private readonly object directionLock = new object(); // Lock object for thread safety

        public Form1()
        {
            InitializeComponent();

            // Configure form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Initialize bitmap for drawing
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;

            // Initialize game loop task
            updateTask = Task.Run(() => UpdateLoop(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        // Update Loop with Threading
        // Update Loop with Threading
        private void UpdateLoop(CancellationToken cancellationToken)
        {
            DateTime lastFrameTime = DateTime.Now;

            while (!cancellationToken.IsCancellationRequested)
            {
                // Calculate delta time
                DateTime currentFrameTime = DateTime.Now;
                float dt = (float)(currentFrameTime - lastFrameTime).TotalSeconds;
                lastFrameTime = currentFrameTime;

                // Update game objects
                InitializeObjects();

                // Calculate frame time
                float frameTime = dt;

                // Invoke UI update on PictureBox and FPS counter
                if (pictureBox1.InvokeRequired)
                {
                    var d = new SafeCallDelegate(UpdatePictureMap);
                    pictureBox1.Invoke(d, new object[] { dt, frameTime });
                }
                else
                {
                    UpdatePictureMap(dt, frameTime);
                }

                //Thread.Sleep(12); // Limit frame rate
            }
        }


        // Update the PictureBox with the game state
        // Update the PictureBox with the game state
        private void UpdatePictureMap(float dt, float frameTime)
        {
            float deltaX, deltaY;
            lock (directionLock)
            {
                // Calculate movement based on direction and speed
                deltaX = speed * horizontalDirection;
                deltaY = speed * verticalDirection;
            }

            // Clear and redraw bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.LightGray); // Clear the bitmap
                ballsCollsion(dt);
            }

            // Calculate FPS
            float fps = 1.0f / frameTime;

            // Update FPS label
            fps_Lable.Text = $"FPS: {fps:F2}";

            pictureBox1.Refresh(); // Refresh PictureBox to display changes
        }

        // Initialize game objects (not implemented)
        private void ballsCollsion(float dt)
        {

            if (mainBall.collide(otherBall))
            {
                mainBall.ResolveCollision(otherBall);

            }
            if (mainBall.collide(otherBall2))
            {
                mainBall.ResolveCollision(otherBall2);

            }
            if (otherBall.collide(otherBall2))
            {
                otherBall.ResolveCollision(otherBall2);

            }
            int sx, sy;
            if (mainBall.collide(outBound, out sx, out sy))
            {
                mainBall.Velocity.X = mainBall.Velocity.X * sx;
                mainBall.Velocity.Y = mainBall.Velocity.Y * sy;

            }
            if (otherBall.collide(outBound, out sx, out sy))
            {
                otherBall.Velocity.X = otherBall.Velocity.X * sx;
                otherBall.Velocity.Y = otherBall.Velocity.Y * sy;

            }

            if (otherBall2.collide(outBound, out sx, out sy))
            {
                otherBall2.Velocity.X = otherBall2.Velocity.X * sx;
                otherBall2.Velocity.Y = otherBall2.Velocity.Y * sy;

            }

            mainBall.advanceBallPosition(dt);
            otherBall.advanceBallPosition(dt);
            otherBall2.advanceBallPosition(dt);
            outBound.Draw(bitmap);
            mainBall.DrawFill(bitmap);
            otherBall.DrawFill(bitmap);
            otherBall2.DrawFill(bitmap);
        }
        private void InitializeObjects()
        {
            if (bitmap == null)
            {
                bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Create a new bitmap
            }
            if (cPoint.IsEmpty)
            {
                cPoint = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            }

            if (mainBall == null)
            {
                mainBall = new Circle(new Vector2(cPoint.X - 200, cPoint.Y), Vector2.zero, 0.2f, Color.White, 20);
            }
            if (otherBall == null)
            {
                otherBall = new Circle(new Vector2(cPoint.X + 100, cPoint.Y),Vector2.zero, 0.2f, Color.Lime, 20);
            }                     
            if (otherBall2 == null)
            {
                otherBall2 = new Circle(new Vector2(cPoint.X, cPoint.Y), Vector2.zero, 0.2f, Color.Blue, 20);
            }
            if (outBound == null)
            {
                outBound = new Square(Color.Red, new Point(cPoint.X - 10, cPoint.Y - 10), cPoint, pictureBox1.Width - 40, pictureBox1.Height - 40);
            }

        }

        // Event handler for key press
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Update movement direction based on key press
            switch (e.KeyCode)
            {
                case Keys.W:
                    lock (directionLock)
                    {
                        verticalDirection = -1.0f;
                    }
                    break;
                case Keys.S:
                    lock (directionLock)
                    {
                        verticalDirection = 1.0f;
                    }
                    break;
                case Keys.D:
                    lock (directionLock)
                    {
                        horizontalDirection = 1.0f;
                    }
                    break;
                case Keys.A:
                    lock (directionLock)
                    {
                        horizontalDirection = -1.0f;
                    }
                    break;
            }
        }

        // Event handler for key release
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            // Stop movement when key is released
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    lock (directionLock)
                    {
                        verticalDirection = 0.0f;
                    }
                    break;
                case Keys.D:
                case Keys.A:
                    lock (directionLock)
                    {
                        horizontalDirection = 0.0f;
                    }
                    break;
            }
        }

        // Event handler for form closure
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            cancellationTokenSource.Cancel(); // Request cancellation of the update loop task
            bitmap.Dispose(); // Dispose the bitmap
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mainBall.IsPointInCircle(e.Location.X,e.Location.Y))
            {
                // Start dragging
                isDragging = true;
                dragStartPoint = new Vector2(e.Location.X,e.Location.Y);
            }
        }

        
        

        // Event handler for mouse up
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging && e.Button == MouseButtons.Left && isDragging)
            {
                // End dragging
                isDragging = false;
                dragEndPoint = new Vector2(e.Location.X, e.Location.Y);

                mainBall.Velocity =dragStartPoint - dragEndPoint;
            }
        }
    }
}
