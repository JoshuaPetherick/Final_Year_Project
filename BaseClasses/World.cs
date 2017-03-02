using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalYearProject
{
    class World
    {
        public static int WORLDLENGTH = 2400;

        private int level;
        private Goal goal;
        private List<Floor> floors = new List<Floor>();

        private Texture2D floorTexture;
        private Texture2D goalTexture;

        public World(int level)
        {
            this.level = level;
        }

        // Optional for testing reasons
        public void loadTextures(Texture2D floorTexture, Texture2D goalTexture)
        {
            this.floorTexture = floorTexture;
            this.goalTexture = goalTexture;
        }

        public void loadLevel()
        {
            switch (level)
            {
                case 1:
                    for (int i = 0; i < WORLDLENGTH; i+= Floor.WIDTH)
                    { // Height - texture.height = 470
                        for (int j = 0; j < Floor.HEIGHT; j+= Floor.HEIGHT)
                        {
                            if (i >= 200 && i <= 240) { }
                            else
                            {
                                floors.Add(new Floor(i, (Game1.GAMEHEIGHT - (j + Floor.HEIGHT)), floorTexture));
                            }
                        }
                    }
                    goal = new Goal(WORLDLENGTH-50, Game1.GAMEHEIGHT - 400, goalTexture);
                    break;

                case 2:
                    for (int i = 0; i < 400; i++)
                    { // Height - texture.height = 470
                        for (int j = 1; j < 4; j++)
                        {
                            floors.Add(new Floor((i * 10), (Game1.GAMEHEIGHT - (j * 10)), floorTexture));
                        }
                    }
                    break;
            }
        }

        public void unloadLevel()
        {
            floors.Clear();
            goal = null;
        }

        public void nextLevel()
        {
            level++;
            unloadLevel();
            loadLevel();
        }

        public int checkColliding(int px, int py, int ph, int pw)
        {
            // Check this for testing purposes
            if (goal != null)
            {
                if (Logic.axisAlignedBoundingBox(px, py, ph, pw, goal.x, goal.y, goal.height, goal.width))
                {
                    return 2;
                }
            }
            foreach (Floor floor in floors)
            {
                // Only want to check floors which are NEAR the player
               if (floor.x >= px - 50 && floor.x <= px + (pw + 50))
               {
                    if (Logic.axisAlignedBoundingBox(px, py, ph, pw, floor.x, floor.y, Floor.HEIGHT, Floor.WIDTH))
                    {
                        return 1;
                    }
                }
            }
            return 0;
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
