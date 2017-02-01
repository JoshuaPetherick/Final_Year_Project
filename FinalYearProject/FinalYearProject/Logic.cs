
using System;

namespace FinalYearProject
{
    class Logic
    {
        private static int speed = 2;
        private static int gravity = 1;

        public static bool axisAlignedBoundingBox(int px, int py, int ph, int pw, int ox, int oy, int oh, int ow)
        {
            // https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection
            if (px < (ox + ow) && (px + pw) > ox &&
                py < (oy + oh) && (ph + py) > oy)
            {
                return true;
            }
            return false;
        }

        public static Tuple<int, int> actionTree(Player player, string action)
        {
            int x = player.getX();
            int y = player.getY();
            switch (action)
            {
                case "1":
                    // Move Left
                    if ((x - speed) > 0)
                    {
                        x = x - speed;
                    }
                    break;

                case "2":
                    // Move Right
                    if ((x + speed) < (Game1.GAMEWIDTH - Player.PREFWIDTH))
                    {
                        x = x + speed;
                    }
                    break;

                case "3":
                    // Jump - Needs player to be passed
                    player.jumpPoint = y - 40;
                    player.state = Player.playerStates.JUMPING;
                    break;
            }
            return new Tuple<int, int>(x, y);
        }

        public static Tuple<int, int> update(Player player, Tuple<int, int> pos, World world)
        {
            int x = pos.Item1;
            int y = pos.Item2;
            int colStatus = world.checkColliding(x, y, Player.PREFHEIGHT, Player.PREFWIDTH);
            if (player.state != Player.playerStates.JUMPING && colStatus == 0)
            {
                player.state = Player.playerStates.FALLING;
            }
            switch (player.state)
            {
                case Player.playerStates.JUMPING:
                    y = y - speed;
                    if (player.getY() <= player.jumpPoint)
                    {
                        player.state = Player.playerStates.FALLING;
                    }
                    break;

                case Player.playerStates.FALLING:
                    y = y + gravity;
                    if (player.getY() > (Game1.GAMEHEIGHT - Player.PREFHEIGHT) || colStatus == 1)
                    {
                        y = y - gravity;
                        player.state = Player.playerStates.IDLE;
                    }
                    break;
            }
            return new Tuple<int, int>(x, y);
        }
    }
}
