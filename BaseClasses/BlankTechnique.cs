using System;

namespace Anti_Latency
{
    /// Empty technique used when no other technique selected
    class BlankTechnique : Technique
    {
        private string lastAction;

        public override void update(Client clint, Player player, World world, string action)
        {
            if (!action.Equals("0"))
            {
                clint.sendMessages(action);
                lastAction = action;
            }
        }

        public override Tuple<int, int> process(Client clnt, World world)
        {
            return clnt.getMessages(world);
        }

        public override string getLastAction()
        {
            return lastAction;
        }
    }
}
