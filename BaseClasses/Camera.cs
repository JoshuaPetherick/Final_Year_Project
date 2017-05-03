using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame 

namespace Anti_Latency
{
    /// Handles on-screen positioning based on Player position
    class Camera
    {
        private Viewport viewport;

        private int x = 0;
        private int window_dist = 200;
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

        /// Moves camera based off player position
        public void update(int px, int py, float deltaTime, World world)
        {
            if (x != px)
            {
                if (x - window_dist < 0)
                {
                    Position = new Vector2(0, 0);
                }
                else if (x - window_dist > (world.getWorldLength() - viewport.Width))
                {
                    Position = new Vector2((world.getWorldLength() - viewport.Width), 0);
                }
                else
                {
                    Position = new Vector2((x - window_dist), 0);
                }
                x = px;
            }
        }

        /// Returns cameras X position
        public int getX(World world)
        {
            if (x - window_dist < 0)
            {
                return 0;
            }
            else if (x - window_dist > (world.getWorldLength() - viewport.Width))
            {
                return (world.getWorldLength() - viewport.Width);
            }
            return (x - window_dist);
        }

        /// Pass across camera matrix, based off camera position and origin point
        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

    }
}
