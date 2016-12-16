using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public int ID = 0;
        private NetPeerConfiguration config;
        private NetClient client;

        public Client (string ip, int port)
        {
            config = new NetPeerConfiguration("FYP");
            client = new NetClient(config);
            client.Start();
            try
            {
                client.Connect(ip, port);
                ID = (int)client.ConnectionStatus;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
        }
    }
}
