﻿using System;

namespace FinalYearProject
{
    class Logic
    {
        private static int speed = 2;
        private static int gravity = 1;

        // Checks collsion, return what side it's colliding with (If it's colliding)
        public static int axisAlignedBoundingBox(int px, int py, int ph, int pw, int ox, int oy, int oh, int ow)
        {
            // http://gamedev.stackexchange.com/questions/29786/a-simple-2d-rectangle-collision-algorithm-that-also-determines-which-sides-that
            float w = 0.5f * (ow + pw);
            float h = 0.5f * (oh + ph);
            float dx = (ox + (ow/2)) - (px + (pw/2));
            float dy = (oy + (oh/2)) - (py + (ph/2));

            // Is colliding
            if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
            {
                float wy = w * dy;
                float hx = h * dx;

                if (wy > hx)
                {
                    if (wy > -hx)
                    {
                        return 1; // Top
                    }
                    else
                    {
                        return 2; // Left
                    }
                }
                else
                {
                    if (wy > -hx)
                    {
                        return 3; // Right
                    }
                    else
                    {
                        return 4; // Bottom
                    }
                        
                 }
            }
            return 0;
        }

        public static Tuple<int, int> actionTree(ServerPlayer player, World world, string action)
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
                        if (world.checkColliding(x, (y-1), ServerPlayer.PREFHEIGHT, ServerPlayer.PREFWIDTH) == 2)
                        {
                            x = x + speed;
                        }
                    }
                    break;

                case "2":
                    // Move Right
                    if ((x + speed) < (world.getWorldLength() - ServerPlayer.PREFWIDTH))
                    {
                        x = x + speed;
                        if (world.checkColliding(x, (y-1), ServerPlayer.PREFHEIGHT, ServerPlayer.PREFWIDTH) == 3)
                        {
                            x = x - speed;
                        }
                    }
                    break;

                case "3":
                    // Jump - Needs player to be passed
                    player.jumpPoint = y - 50;
                    player.state = ServerPlayer.playerStates.JUMPING;
                    break;
            }
            return new Tuple<int, int>(x, y);
        }

        /*
        *   Additional method required for Client Side Prediction technique (Different player type)
        */
        public static Tuple<int, int> actionTree(Player player, World world, string action)
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
                        if (world.checkColliding(x, (y-1), Player.PREFHEIGHT, Player.PREFWIDTH) == 2)
                        {
                            x = x + speed;
                        }
                    }
                    break;

                case "2":
                    // Move Right
                    if ((x + speed) < (world.getWorldLength() - Player.PREFWIDTH))
                    {
                        x = x + speed;
                        if (world.checkColliding(x, (y-1), Player.PREFHEIGHT, Player.PREFWIDTH) == 3)
                        {
                            x = x - speed;
                        }
                    }
                    break;

                case "3":
                    // Jump - Needs player to be passed
                    player.jumpPoint = y - 50;
                    player.state = Player.playerStates.JUMPING;
                    break;
            }
            return new Tuple<int, int>(x, y);
        }

        public static Tuple<int, int> update(ServerPlayer player, Tuple<int, int> pos, World world)
        {
            int x = pos.Item1;
            int y = pos.Item2;
            int colStatus = world.checkColliding(x, y, ServerPlayer.PREFHEIGHT, ServerPlayer.PREFWIDTH);
            if (player.state != ServerPlayer.playerStates.JUMPING && colStatus == 0)
            {
                player.state = ServerPlayer.playerStates.FALLING;
            }
            switch (player.state)
            {
                case ServerPlayer.playerStates.JUMPING:
                    y = y - speed;
                    if (y <= player.jumpPoint)
                    {
                        player.state = ServerPlayer.playerStates.FALLING;
                    }
                    else if (colStatus == 4)
                    {
                        y = y + speed;
                        player.state = ServerPlayer.playerStates.FALLING;
                    }
                    break;

                case ServerPlayer.playerStates.FALLING:
                    y = y + gravity;
                    colStatus = world.checkColliding(x, y, ServerPlayer.PREFHEIGHT, ServerPlayer.PREFWIDTH);
                    if (y > (Game1.GAMEHEIGHT - ServerPlayer.PREFHEIGHT) || colStatus == 1)
                    {
                        player.state = ServerPlayer.playerStates.IDLE;
                    }
                    break;
            }
            return new Tuple<int, int>(x, y);
        }

        /*
        *   Additional method required for Client Side Prediction technique (Different player type)
        */
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
                    colStatus = world.checkColliding(x, y, ServerPlayer.PREFHEIGHT, ServerPlayer.PREFWIDTH);
                    if (y <= player.jumpPoint)
                    {
                        player.state = Player.playerStates.FALLING;
                    }
                    else if (colStatus == 4)
                    {
                        y = y + speed;
                        player.state = Player.playerStates.FALLING;
                    }
                    break;

                case Player.playerStates.FALLING:
                    y = y + gravity;
                    colStatus = world.checkColliding(x, y, Player.PREFHEIGHT, Player.PREFWIDTH);
                    if (y > (Game1.GAMEHEIGHT - Player.PREFHEIGHT) || colStatus == 1)
                    {
                        player.state = Player.playerStates.IDLE;
                    }
                    break;
            }
            return new Tuple<int, int>(x, y);
        }
    }
}
