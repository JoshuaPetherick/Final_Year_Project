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
        private List<ServerPlayer> players = new List<ServerPlayer>();
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
                            string action = mess[1];
                            DateTime time = Convert.ToDateTime(mess[2]);

                            ServerPlayer p = getPlayer(id);
                            if (p == null)
                            {
                                Console.WriteLine("ERROR OCCURED: Unable to find player via ID (" + id + ")");
                            }
                            else
                            {
                                applyLogic(p, action); // Update player accordingly
                                string send = id + "/" + p.getX() + "/" + p.getY(); 
                                sendMessages(send, null); // Send updated pos to all players
                            }
                        }
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        if(message.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Tuple<int, int> startPos = world.getPlayerPos();
                            ServerPlayer player = new ServerPlayer(startPos.Item1, startPos.Item2, message.SenderConnection);
                            player.setID(currID.ToString());
                            currID++;
                            players.Add(player);
                            sendMessages(player.getID(), message.SenderConnection);
                        }
                        else if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            for (int pos = 0; pos < players.Count; pos++)
                            { 
                                if (message.SenderConnection == players[pos].getRecipiant())
                                {
                                    players.RemoveAt(pos);
                                }
                            }
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

        public ServerPlayer getPlayer(string ID)
        {
            foreach (ServerPlayer player in players)
            {
                if (player.getID().Equals(ID))
                {
                    return player;
                }
            }
            return null;
        }

        public void applyLogic(ServerPlayer p, string action)
        {
            Tuple<int, int> pos = null;
            pos = Logic.actionTree(p, world, action);
            pos = Logic.update(p, pos, world);
            p.setX(pos.Item1);
            p.setY(pos.Item2);
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
