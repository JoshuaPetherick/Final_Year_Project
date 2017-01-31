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

        //private int MAX_PLAYERS = 8;
        private List<Player> players = new List<Player>();
        private World world;

        public Server (int port, World world)
        {
            this.world = world;
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
                            string id = msg[0].ToString();
                            string state = msg[1].ToString();
                            applyLogic(id, state);
                        }
                        else if (msg.ToString().Equals("0"))
                        {
                            Player player = new Player(0, 0);
                            player.setID(server.ConnectionsCount.ToString());
                            players.Add(player);
                            sendMessages(player.getID(), message.SenderConnection);
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

        public void sendMessages(string msg, NetConnection recipient)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write(msg);
            if (recipient != null)
            {
                server.SendMessage(message, recipient, NetDeliveryMethod.ReliableOrdered);
            }
            else
            {
                server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void applyLogic(string ID, string state)
        {
            foreach (Player player in players)
            {
                if (player.getID().Equals(ID))
                {
                    Tuple<int, int> pos = Logic.actionTree(new Tuple<int, int>(player.getX(), player.getY()), state);
                    break;
                }
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
