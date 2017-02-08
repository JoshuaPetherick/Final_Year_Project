using System;
using System.Collections.Generic;

namespace FinalYearProject
{
    class ServerReconcilliation : Technique
    {
        private List<string> actions = new List<string>(); // Array of Actions

        public override void update(Client clint, Player player, World world, string action)
        {
            if (!action.Equals("0"))
            {
                clint.sendMessages(action);
                actions.Add(action);
            }
            Tuple<int, int> pos = Logic.actionTree(player, action);
            pos = Logic.update(player, pos, world);
            player.setX(pos.Item1);
            player.setY(pos.Item2);
        }

        public override Tuple<int, int> process(Client clnt, World world)
        {
            return clnt.getMessages(world);
        }

        public override string getLastAction()
        {
            return actions[(actions.Count-1)];
        }
    }
}
