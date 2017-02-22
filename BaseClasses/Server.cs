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
        private World world;
        private int currID = 1;
        private List<Player> players = new List<Player>();
        private List<Tuple<string, DateTime>> actions = new List<Tuple<string, DateTime>>();

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
                        if (!msg.ToString().Equals("0"))
                        {
                            string[] mess = msg.Split('/');
                            string id = mess[0];
                            string state = mess[1];
                            DateTime time = Convert.ToDateTime(mess[2]);

                            Player p = applyLogic(id);
                            if (p == null)
                            {
                                Console.WriteLine("ERROR OCCURED: Unable to find player via ID (" + id + ")");
                            }
                            else
                            {
                                // Do something with pos and state
                                // Send updated pos to all players
                                // string send = id + "/" + pos.item1 + "/" + pos.item2; 
                                // sendMessages(send, null);
                            }
                        }
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        if(message.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Player player = new Player(0, 0);
                            player.setID(currID.ToString());
                            currID++;
                            players.Add(player);
                            sendMessages(player.getID(), message.SenderConnection);
                        }
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(message.ReadString());
                        break;

                    default:
                        Console.WriteLine("Unhandled message with type: " + message.MessageType);
                        Console.WriteLine(message.ReadString());
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

        public Player applyLogic(string ID)
        {
            foreach (Player player in players)
            {
                if (player.getID().Equals(ID))
                {
                    return player;
                }
            }
            return null;
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
