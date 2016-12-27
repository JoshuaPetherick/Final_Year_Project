using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FinalYearProject
{
    class World
    {
        private int level;
        private ContentManager Content;

        //private Goal goal;
        private List<Floor> floors = new List<Floor>();

        public World(int level, ContentManager Content)
        {
            this.level = level;
            this.Content = Content;
            loadLevel();
        }

        private void loadLevel()
        {
            switch (level)
            {
                case 1:
                    for (int i = 0; i < 400; i++)
                    { // Height - texture.height = 470
                        for (int j = 0; j < 2; j++)
                        {
                            floors.Add(new Floor((i * 10), (470 - (j * 10)), Content.Load<Texture2D>("floor")));
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < 400; i++)
                    { // Height - texture.height = 470
                        for (int j = 0; j < 4; j++)
                        {
                            floors.Add(new Floor((i * 10), (470 - (j * 10)), Content.Load<Texture2D>("floor")));
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
