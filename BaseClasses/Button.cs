using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anti_Latency
{
    class Button
    {
        private int x;
        private int y;
        private int width;
        private int HEIGHT = 24;
        private string text;
        private bool clicked = false;

        public Button(int x, int y, string text)
        {
            this.x = x;
            this.y = y;
            setText(text);
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return HEIGHT;
        }

        public void setText(string text)
        {
            this.text = text;
            width = (text.Length * 24); // Multiply by size of Font
        }

        public void buttonClicked(bool result)
        {
            clicked = result;
        }

        public bool getResult()
        {
            return clicked;
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (clicked)
            {
                spriteBatch.DrawString(font, text, new Vector2(x, y), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, text, new Vector2(x, y), Color.White);
            }
        }
    }
}
