using Lidgren.Network;

namespace FinalYearProject
{
    class ServerPlayer
    {
        private string ID;

        private int x;
        private int y;
        private NetConnection recipient;
        public static int PREFWIDTH = 45; // Values determined based on personal preference
        public static int PREFHEIGHT = 75; // Values determined based on personal preference

        public playerStates state = playerStates.IDLE; // Made public for Unit Test
        public enum playerStates { IDLE, JUMPING, FALLING };
        public int jumpPoint;

        public ServerPlayer(int x, int y, NetConnection recipient)
        {
            setX(x);
            setY(y);
            this.recipient = recipient;
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

        public NetConnection getRecipiant()
        {
            return recipient;
        }
    }
}
