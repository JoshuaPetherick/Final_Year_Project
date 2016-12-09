using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalYearProject
{
    class Player
    {
        private int x;
        private int y;
        private int speed = 2;
        private Texture2D texture;   

        public Player(int x, int y)
        {
            setX(x);
            setY(x);
        }

        public void setX(int x)
        {
            this.x = x;
        }

        public void setY(int y)
        {
            this.y = y;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public void handleInput()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                y -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                x -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                y += speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                x += speed;
            }
        }

        public void drawPlayer(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, new Rectangle(x, y, texture.Width, texture.Height), Color.GhostWhite);
            spriteBatch.Draw(texture, new Rectangle(x, y, 90, 100), Color.GhostWhite);
        }
    }
}
