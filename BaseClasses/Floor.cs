using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalYearProject
{
    class Floor
    {
        public int x;
        public int y;
        public static int HEIGHT = 50;
        public static int WIDTH = 50;
        private Texture2D texture;

        public Floor(int x, int y, Texture2D texture)
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
