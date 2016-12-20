using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Server
    {
        private NetServer server;
        private NetPeerConfiguration config;

        public Server (int port)
        {
            config = new NetPeerConfiguration("MultiplayerTest")
            {
                Port = port
            };
            server = new NetServer(config);
            server.Start(); 
        }

        ~Server()
        {
            closeServer();
        }

        public void closeServer()
        {
            if (server.Status == NetPeerStatus.Running)
            {
                server.Shutdown("Closing down");
            }
        }

        public int serverStatus()
        {
            return (int)server.Status;
        }
    }
}
