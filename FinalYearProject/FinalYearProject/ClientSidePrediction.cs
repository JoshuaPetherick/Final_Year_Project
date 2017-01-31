using System;

namespace FinalYearProject
{
    class ClientSidePrediction : Technique
    {
        private string lastAction;

        public override void update(string action, Client clint)
        {
            clint.sendMessages(action);
            lastAction = action;
        }

        public override string getLastAction()
        {
            return lastAction;
        }
    }
}
