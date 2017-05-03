using System;
using System.Collections.Generic;
using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace Anti_Latency
{
    /// The server class which handles connected users and game logic
    class Server
    {
        private NetServer server;
        private NetPeerConfiguration config;

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

        /// Messages from clients will be structure as 2 bytes (e.g. 11, 12, 21, etc)
        /// First = players ID (Each player will be assigned an ID by the server)
        /// Second = player action/state
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

        /// Send messages to a particuarly client or all clients
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

        /// Return player based on their ID
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

        /// Handles logic after recieving a users action
        public void applyLogic(ServerPlayer p, string action)
        {
            Tuple<int, int> pos = null;
            pos = Logic.actionTree(p, world, action);
            p.setX(pos.Item1);
            p.setY(pos.Item2);
        }

        /// Primary method for server, checks for messages every 20 milliseconds
        public void update()
        {
            DateTime fpsDelay = DateTime.Now;
            bool goalHit = false;
            while (server.Status == NetPeerStatus.Running)
            {
                checkMessages();
                if (DateTime.Now > fpsDelay)
                {
                    // Applies update logic to players (Such as gravity)
                    for (int i = 0; i < players.Count; i++)
                    {
                        Tuple<int, int> currPos = new Tuple<int, int>(players[i].getX(), players[i].getY());
                        Tuple<int, int> pos = Logic.update(players[i], currPos, world);
                        players[i].setX(pos.Item1);
                        players[i].setY(pos.Item2);
                        // Check if has touched goal
                        if (world.checkColliding(players[i].getX(), players[i].getY(), players[i].getHeight(), players[i].getWidth()) == 5)
                        {
                            goalHit = true;
                        }
                        else if (currPos.Item1 != pos.Item1 || currPos.Item2 != pos.Item2)
                        {
                            string send = players[i].getID() + "/" + players[i].getX() + "/" + players[i].getY();
                            sendMessages(send, null); // Send updated pos to all players
                        }
                    }
                    // Goal touched! Reset positions for next round!
                    if (goalHit)
                    {
                        Tuple<int, int> originalPos = world.getPlayerPos();
                        for (int i = 0; i < players.Count; i++)
                        {
                            players[i].setX(originalPos.Item1);
                            players[i].setY(originalPos.Item2);
                            string send = players[i].getID() + "/" + players[i].getX() + "/" + players[i].getY();
                            sendMessages(send, null); // Send updated pos to all players
                        }
                        goalHit = false;
                    }
                    fpsDelay = DateTime.Now.AddMilliseconds(20);
                }
            }
        }

        /// Returns servers status in Integer form
        public int serverStatus()
        {
            return (int)server.Status;
        }

        /// Closes the server
        public void closeServer()
        {
            if (server.Status == NetPeerStatus.Running)
            {
                server.Shutdown("Closing down");
            }
        }
    }
}
