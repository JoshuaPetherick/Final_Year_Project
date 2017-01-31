using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalYearProject
{
    class World
    {
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
                    for (int i = 0; i < 400; i++)
                    { // Height - texture.height = 470
                        for (int j = 1; j < 4; j++)
                        {
                            floors.Add(new Floor((i * 10), (Game1.GAMEHEIGHT - (j * 10)), floorTexture));
                        }
                    }
                    goal = new Goal(680, Game1.GAMEHEIGHT - 430, goalTexture);
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
                if (axisAlignedBoundingBox(px, py, ph, pw, goal.x, goal.y, goal.height, goal.width))
                {
                    return 2;
                }
            }
            foreach (Floor floor in floors)
            {
                // Only want to check floors which are NEAR the player
               if (floor.x >= px && floor.x <= (px + pw))
               {
                    if (axisAlignedBoundingBox(px, py, ph, pw, floor.x, floor.y, floor.height, floor.width))
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        public bool axisAlignedBoundingBox(int px, int py, int ph, int pw, int ox, int oy, int oh, int ow)
        {
            // https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection
            if (px < (ox+ow) && (px + pw) > ox &&
                py < (oy + oh) && (ph + py) > oy)
            {
                return true;
            }
            return false;
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
