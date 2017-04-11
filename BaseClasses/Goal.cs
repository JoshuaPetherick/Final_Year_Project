using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalYearProject
{
    class Goal
    {
        public int x;
        public int y;
        public static int HEIGHT = 400;
        public static int WIDTH = 20;
        private Texture2D texture;

        public Goal(int x, int y, Texture2D texture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, WIDTH, HEIGHT), Color.GhostWhite);
        }

    }
}
