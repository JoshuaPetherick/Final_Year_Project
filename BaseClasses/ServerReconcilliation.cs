using System;
using System.Collections.Generic;

namespace Anti_Latency
{
    /// Server Reconciliation  Technique
    class ServerReconcilliation : Technique
    {
        private List<string> actions = new List<string>(); // Array of Actions
        private List<Tuple<int, int>> positions = new List<Tuple<int, int>>();

        /// Recieve input from player. Process and return expected position
        public override void update(Client clint, Player player, World world, string action)
        {
            if (!action.Equals("0"))
            {
                clint.sendMessages(action);
                actions.Add(action);
            }
            // CLIENT-SIDE PREDICTION
            // <---------------------------------------------------------->
            Tuple<int, int> pos = Logic.actionTree(player, world, action);
            pos = Logic.update(player, pos, world);
            player.setX(pos.Item1);
            player.setY(pos.Item2);
            positions.Add(pos);
            // <---------------------------------------------------------->
            
        }

        /// Recieve input from server. Check if matches positions array, return nothing if matches, else return server position
        public override Tuple<int, int> process(Client clnt, World world)
        {
            Tuple<int, int> pos = clnt.getMessages(world);
            // SERVER RECONCILLIATION
            // <--------------------------------------------------------------------------------->
            // if new position matches a previous position then don't reset pos - otherwise do so
            if (pos != null)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    if (positions[i].Item1 == pos.Item1 && positions[i].Item2 == pos.Item2)
                    {
                        positions.RemoveAt(i);
                        return null; // Return empty as no need to change player position
                    }
                }
                positions.Clear(); // None match, reset array
            }
            return pos;
            // <--------------------------------------------------------------------------------->
        }

        /// Created for testing purposes
        public override string getLastAction()
        {
            return actions[(actions.Count-1)];
        }
    }
}
