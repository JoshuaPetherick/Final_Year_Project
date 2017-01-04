using System;

namespace FinalYearProject
{
    class ClientSidePrediction : Technique
    {
        private int lastAction;

        public ClientSidePrediction()
        {

        }

        public override void update(int action)
        {
            lastAction = action;
            // Do Client side stuff in here...
        }

        public override int getLastAction()
        {
            return lastAction;
        }
    }
}
