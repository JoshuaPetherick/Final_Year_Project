using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Anti_Latency
{
    /// World object designed to manage game world (Floor & Goal)
    class World
    {
        private int WORLDLENGTH;

        private Tuple<int, int> playerPos;
        private Goal goal;
        private List<Floor> floors = new List<Floor>();

        private Texture2D floorTexture;
        private Texture2D goalTexture;
        private Texture2D flagTexture;

        /// Pass across images for objects - Optional for testing reasons
        public void loadTextures(Texture2D floorTexture, Texture2D goalTexture, Texture2D flagTexture)
        {
            this.floorTexture = floorTexture;
            this.goalTexture = goalTexture;
            this.flagTexture = flagTexture;
        }

        /// Load in level from text file
        public void loadLevel()
        {
            buildFromFile(File.ReadAllLines("Content/level.txt"));
        }

        /// Create objects based off position in text file
        private void buildFromFile(string[] lines)
        {
            int x, y = 0;
            foreach (string line in lines)
            {
                x = 0;
                foreach (char type in line)
                {
                    newObject(type, x, y);
                    x += Floor.WIDTH;
                    WORLDLENGTH = x;
                }
                y += Floor.HEIGHT;
            }
        }

        /// New object found, create based on character type (e.g. P = Player Start Position)
        private void newObject(char type, int x, int y)
        {
            switch (type)
            {
                case 'G':
                    floors.Add(new Floor(x, y, floorTexture));
                    break;
                case 'E':
                    goal = new Goal(x, ((y+Floor.HEIGHT)-Goal.HEIGHT), goalTexture, flagTexture);
                    break;
                case 'P':
                    playerPos = new Tuple<int, int>(x, (y-Player.PREFHEIGHT));
                    break;
            }
        }

        /// Set player start point - created for test purposes
        public void setPlayerPos(Tuple<int, int>newPos)
        {
            playerPos = newPos;
        }

        /// Return player start position 
        public Tuple<int, int> getPlayerPos()
        {
            return playerPos;
        }

        /// Empty world arrays
        public void unloadLevel()
        {
            floors.Clear();
            goal = null;
        }

        /// Check if player colliding with floor objects or goal
        public int checkColliding(int px, int py, int ph, int pw)
        {
            // Check this for testing purposes
            if (goal != null)
            {
                if (Logic.axisAlignedBoundingBox(px, py, ph, pw, goal.x, goal.y, Goal.HEIGHT, Goal.WIDTH) > 0)
                {
                    return 5;
                }
            }
            foreach (Floor floor in floors)
            {
                // Only want to check floors which are NEAR the player
               if (floor.x >= px - 50 && floor.x <= px + (pw + 50))
               {
                    int tempResult = Logic.axisAlignedBoundingBox(px, py, ph, pw, floor.x, floor.y, Floor.HEIGHT, Floor.WIDTH);
                    if (tempResult > 0)
                    {
                        return tempResult;
                    }
                }
            }
            return 0;
        }

        public int getWorldLength()
        {
            return WORLDLENGTH;
        }

        /// Draw all floor and goal object
        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Floor floor in floors)
            {
                floor.draw(spriteBatch);
            }
            goal.draw(spriteBatch);
        }
    }
}
