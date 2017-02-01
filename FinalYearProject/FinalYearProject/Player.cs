using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FinalYearProject
{
    class Player
    {
        Client clnt;
        private string ID;
        Technique technique = new ClientSidePrediction();

        private int x;
        private int y;
        public static int PREFWIDTH = 45; // Values determined based on personal preference
        public static int PREFHEIGHT = 75; // Values determined based on personal preference
        private Texture2D texture;

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

        public void setID(string ID)
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

        public string getID()
        {
            return ID;
        }

        public void handleInput(World world)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Move Left
                technique.update(clnt, this, world, "1");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Move Right
                technique.update(clnt, this, world, "2");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && state == playerStates.IDLE)
            {
                technique.update(clnt, this, world, "3");
            }
        }

        public void playerUpdate(World world)
        {
            // Get updates from server
            if (clnt != null)
            {
                // Get updated position from server/local
                Tuple<int, int> pos = technique.process(clnt, world);
                setX(pos.Item1);
                setY(pos.Item2);
            }
            // Handle player input
            handleInput(world);
        }

        public void connectClient(string ip, int port)
        {
            clnt = new Client(ip, port);
        }

        public void drawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, PREFWIDTH, PREFHEIGHT), Color.GhostWhite);
        }
    }
}
