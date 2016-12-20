using System;
using System.Collections.Generic;
using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Server
    {
        private NetServer server;
        private NetPeerConfiguration config;

        private int MAX_PLAYERS = 8;
        private List<Player> players = new List<Player>();

        public Server (int port)
        {
            config = new NetPeerConfiguration("FYP")
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

        // Messages from clients will be structure as 2 bytes (e.g. 11, 12, 21, etc)
        // First bit = players ID (Each player will be assigned an ID by the server)
        // Second bit = player action/state

        public void checkMessages()
        {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle client messages
                        string msg = message.ReadString();
                        if (msg.Length == 2)
                        {
                            applyLogic((int)msg[0], (int)msg[1]);
                        }
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        var dat = message.ReadByte();
                        Console.WriteLine(dat);
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(message.ReadString());
                        break;

                    default:
                        Console.WriteLine("Unhandled message with type: " + message.MessageType);
                        break;
                }
            }
        }

        public void sendMessages(string msg)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write(msg);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }

        public void applyLogic(int ID, int state)
        {
            Player player;
            foreach (Player p in players)
            {
                if (p.getID() == ID)
                {
                    player = p;
                    break;
                }
            }
            switch(state)
            {
                case 1:
                    // Move Left
                    break;

                case 2:
                    // Move Right
                    break;

                case 3:
                    // Jump
                    break;
            }
        }

        public int serverStatus()
        {
            return (int)server.Status;
        }

        public void closeServer()
        {
            if (server.Status == NetPeerStatus.Running)
            {
                server.Shutdown("Closing down");
            }
        }
    }
}
