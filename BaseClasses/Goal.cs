using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anti_Latency
{
    /// Basic goal object 
    class Goal
    {
        public int x;
        public int y;
        public static int HEIGHT = 400;
        public static int WIDTH = 20;
        private Texture2D texture;
        private Texture2D flagTexture;

        public Goal(int x, int y, Texture2D texture, Texture2D flagTexture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
            this.flagTexture = flagTexture;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, WIDTH, HEIGHT), Color.GhostWhite);
            spriteBatch.Draw(flagTexture, new Rectangle((x+5), (y+5), 60, 40), Color.GhostWhite);
        }

    }
}
