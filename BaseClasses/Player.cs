using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalYearProject
{
    class Player
    {
        Client clnt;
        private bool input = true;
        Technique technique = new ServerReconcilliation();

        private int x;
        private int y;
        private Texture2D texture;
        public static int PREFWIDTH = 45; // Values determined based on personal preference
        public static int PREFHEIGHT = 75; // Values determined based on personal preference
        private SpriteEffects effect = SpriteEffects.None;

        public playerStates state = playerStates.IDLE; // Made public for Unit Test
        public enum playerStates { IDLE, JUMPING, FALLING};
        public int jumpPoint;

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

        public int getHeight()
        {
            return PREFHEIGHT;
        }

        public int getWidth()
        {
            return PREFWIDTH;
        }

        public void handleInput(World world)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Move Left
                technique.update(clnt, this, world, "1");
                effect = SpriteEffects.FlipHorizontally;
                input = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Move Right
                technique.update(clnt, this, world, "2");
                effect = SpriteEffects.None;
                input = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && state == playerStates.IDLE)
            {
                technique.update(clnt, this, world, "3");
                input = true;
            }
        }

        public void playerUpdate(World world)
        {
             // Handle player input
            handleInput(world);
            if (!input)
            {
                technique.update(clnt, this, world, "0");
            }
            // Get updates from server
            if (clnt != null)
            {
                // Get updated position from server/local
                Tuple<int, int> pos = technique.process(clnt, world);
                if (pos != null)
                {
                    setX(pos.Item1);
                    setY(pos.Item2);
                }
            }
        }

        /*
            Connect to a server
        */
        public void connectClient(string ip, int port)
        {
            clnt = new Client(ip, port);
        }

        /*
            Connect to a server
        */
        public void connectClient(bool local)
        {
            clnt = new Client(local);
        }

        public void drawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, PREFWIDTH, PREFHEIGHT), null,
                new Color(255, 255, 255, 1.0f), 0.0f, new Vector2(0, 0), effect, 0.0f);
            input = false; // Reset input
        }

        public void drawClient(SpriteBatch spriteBatch)
        {
            ServerPlayer cPlayer = clnt.getPlayer();
            spriteBatch.Draw(texture, new Rectangle(cPlayer.getX(), cPlayer.getY(), PREFWIDTH, PREFHEIGHT), null,
                new Color(255, 255, 255, 0.5f), 0.0f, new Vector2(0, 0), effect, 0.0f);
        }
    }
}
