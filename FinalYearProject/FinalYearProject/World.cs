using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalYearProject
{
    class World
    {
        private int level;
        //private Goal goal;
        private List<Floor> floors = new List<Floor>();

        private int GAMEWIDTH;
        private int GAMEHEIGHT;

        private Texture2D floorTexture;
        private Texture2D goalTexture;

        public World(int level, int GAMEWIDTH, int GAMEHEIGHT)
        {
            this.level = level;
            this.GAMEWIDTH = GAMEWIDTH;
            this.GAMEHEIGHT = GAMEHEIGHT;
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
                            floors.Add(new Floor((i * 10), (GAMEHEIGHT - (j * 10)), floorTexture));
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < 400; i++)
                    { // Height - texture.height = 470
                        for (int j = 1; j < 4; j++)
                        {
                            floors.Add(new Floor((i * 10), (GAMEHEIGHT - (j * 10)), floorTexture));
                        }
                    }
                    break;
            }
        }

        public void unloadLevel()
        {
            floors.Clear();
            //goal = null;
        }

        public void nextLevel()
        {
            level++;
            unloadLevel();
            loadLevel();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Floor floor in floors)
            {
                floor.draw(spriteBatch);
            }
            //goal.draw(spriteBatch);
        }
    }
}
