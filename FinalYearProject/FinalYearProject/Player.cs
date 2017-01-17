using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalYearProject
{
    class Player
    {
        Client client;
        //Technique technique;

        private int x;
        private int y;
        private int ID;
        private int speed = 2;
        private double gravity = 1;

        private Texture2D texture;
        private int PREFWIDTH = 45; // Values determined based on personal preference
        private int PREFHEIGHT = 75; // Values determined based on personal preference

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

        public void setID(int ID)
        {
            this.ID = ID;
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

        public int getHeight()
        {
            return PREFHEIGHT;
        }

        public int getWidth()
        {
            return PREFWIDTH;
        }

        public int getID()
        {
            return ID;
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
                if ((y + speed) < (Game1.GAMEHEIGHT - PREFHEIGHT))
                {
                    setY(y + speed);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if ((x + speed) < (Game1.GAMEWIDTH - PREFWIDTH))
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

        public void playerUpdate(World world)
        {
            handleInput();
            int colStatus = world.checkColliding(x, y, PREFHEIGHT, PREFWIDTH);
            if (state != playerStates.JUMPING && colStatus == 0)
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
                    gravity += 0.25; // Increase effect of gravity
                    setY(y + (int)gravity);
                    if (y > (Game1.GAMEHEIGHT - PREFHEIGHT) || colStatus == 1)
                    {
                        setY((y - (int)gravity));
                        state = playerStates.IDLE;
                        gravity = 1; // Reset Gravity
                    }
                    break;
            }
        }

        public void connectClient(string ip, int port)
        {
            client = new Client(ip, port);
        }

        public void drawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, PREFWIDTH, PREFHEIGHT), Color.GhostWhite);
        }
    }
}
