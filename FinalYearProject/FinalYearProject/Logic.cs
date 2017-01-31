
using System;

namespace FinalYearProject
{
    public static class Logic
    {
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

        public static Tuple<int, int> actionTree(Tuple<int, int> pos, string action)
        {
            int x = pos.Item1;
            int y = pos.Item2;
            switch (action)
            {
                case "1":
                    // Move Left
                    x = x - 1;
                    break;

                case "2":
                    // Move Right
                    x = x + 1;
                    break;

                case "3":
                    // Jump
                    break;
            }
            return new Tuple<int, int>(x, y);
        }
    }
}
