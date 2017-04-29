﻿using System;

namespace Anti_Latency
{
    class ClientSidePrediction : Technique
    {
        private string lastAction;

        public override void update(Client clint, Player player, World world, string action)
        {
            if (!action.Equals("0"))
            {
                clint.sendMessages(action);
                lastAction = action;
            }
            // CLIENT-SIDE PREDICTION
            // <---------------------------------------------------------->
            Tuple<int, int> pos = Logic.actionTree(player, world, action);
            pos = Logic.update(player, pos, world);
            player.setX(pos.Item1);
            player.setY(pos.Item2);
            // <---------------------------------------------------------->
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
