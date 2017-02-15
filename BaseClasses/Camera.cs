using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame 

namespace FinalYearProject
{
    class Camera
    {
        private Viewport viewport;

        private int x = 0;
        public float Zoom = 1.0f;
        public float Rotation = 0;
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;

            Origin = new Vector2(viewport.Width / 2f, viewport.Width / 2f);
            Position = Vector2.Zero;
        }

        public void update(int px, int py, float deltaTime)
        {
            if (x != px)
            {
                int diffX = x - px;
                Position -= new Vector2((60 * diffX), 0) * deltaTime;
                // Don't move camera off world setting
                if (Position.X < 0 || Position.X >= (World.WORLDLENGTH - viewport.Width))
                { 
                    Position += new Vector2((60 * diffX), 0) * deltaTime;
                }
                x = px;
            }
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

    }
}
