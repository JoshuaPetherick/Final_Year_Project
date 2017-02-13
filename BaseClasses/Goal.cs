using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalYearProject
{
    class Goal
    {
        public int x;
        public int y;
        public int height = 400;
        public int width = 20;
        private Texture2D texture;

        public Goal(int x, int y, Texture2D texture)
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
