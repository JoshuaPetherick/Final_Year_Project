using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalYearProject
{
    class Floor
    {
        public int x;
        public int y;
        public int height = 10;
        public int width = 10;
        private Texture2D texture;

        public Floor(int x, int y, Texture2D texture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.GhostWhite);
        }
    }
}
