using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalYearProject
{
    class World
    {
        private int WORLDLENGTH;

        private Tuple<int, int> playerPos;
        private Goal goal;
        private List<Floor> floors = new List<Floor>();

        private Texture2D floorTexture;
        private Texture2D goalTexture;

        // Optional for testing reasons
        public void loadTextures(Texture2D floorTexture, Texture2D goalTexture)
        {
            this.floorTexture = floorTexture;
            this.goalTexture = goalTexture;
        }

        public void loadLevel()
        {
            buildFromFile(File.ReadAllLines("Final_Year_Project/Content/level.txt"));
        }

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
            Console.WriteLine(WORLDLENGTH);
        }

        private void newObject(char type, int x, int y)
        {
            switch (type)
            {
                case 'G':
                    floors.Add(new Floor(x, y, floorTexture));
                    break;
                case 'E':
                    goal = new Goal(x, ((y+Floor.HEIGHT)-Goal.HEIGHT), goalTexture);
                    break;
                case 'P':
                    playerPos = new Tuple<int, int>(x, (y-Player.PREFHEIGHT));
                    break;
            }
        }

        public Tuple<int, int> getPlayerPos()
        {
            return playerPos;
        }

        public void unloadLevel()
        {
            floors.Clear();
            goal = null;
        }

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
                        Console.WriteLine(tempResult);
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
