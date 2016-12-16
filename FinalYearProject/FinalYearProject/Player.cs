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
        private double gravity = 1;

        private Texture2D texture;
        private int PREFWIDTH = 60; // Values determined based on personal preference
        private int PREFHEIGHT = 100; // Values determined based on personal preference

        public playerStates state = playerStates.IDLE; // Made public for Unit Test
        public enum playerStates { IDLE, JUMPING, FALLING};
        private int jumpPoint;

        public Player(int x, int y)
        {
            setX(x);
            setY(y);
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
                if ((y - speed) > 0)
                {
                    setY(y - speed);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if ((x - speed) > 0)
                {
                    setX(x - speed);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if ((y + speed) < (600 - PREFHEIGHT)) // Need to get GAMEHEIGHT
                {
                    setY(y + speed);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if ((x + speed) < (800 - PREFWIDTH)) // Need to get GAMEWIDTH
                {
                    setX(x + speed);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && state == playerStates.IDLE)
            {
                jumpPoint = y - 40;
                state = playerStates.JUMPING;
            }
        }

        public void playerUpdate()
        {
            handleInput();
            
            if (y != (600 - PREFHEIGHT) && state != playerStates.JUMPING)
            {
                state = playerStates.FALLING;
            }
            switch (state)
            {
                case playerStates.JUMPING:
                    setY(y - speed);
                    if (y <= jumpPoint)
                    {
                        state = playerStates.FALLING;
                    }
                    break;

                case playerStates.FALLING:
                    gravity += 0.5; // Increase effect of gravity
                    setY(y + (int)gravity);
                    if (y > (600 - PREFHEIGHT))  // Need to get GAMEHEIGHT
                    {
                        setY((600 - PREFHEIGHT)); // Need to get GAMEHEIGHT
                        state = playerStates.IDLE;
                        gravity = 1; // Reset Gravity
                    }
                    break;
            }
        }

        public void drawPlayer(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, new Rectangle(x, y, texture.Width, texture.Height), Color.GhostWhite);
            spriteBatch.Draw(texture, new Rectangle(x, y, PREFWIDTH, PREFHEIGHT), Color.GhostWhite);
        }
    }
}
