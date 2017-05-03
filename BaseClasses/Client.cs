using System;
using Lidgren.Network;
using System.Collections.Generic;
//https://github.com/lidgren/lidgren-network-gen3

namespace Anti_Latency
{
    /// Lidgren client class designed to communicate with Server 
    class Client
    {
        public string ID = "0"; // ID assigned by Server
        private NetPeerConfiguration config;
        private NetClient client;

        public bool local;
        private double delay = 0; // Millisecond Delay
        private DateTime lastSent = DateTime.Now;
        private List<ServerPlayer> players = new List<ServerPlayer>();
        private List<string> actions = new List<string>(); // Array of Actions

        /// (Server) Pass across ip address and port 
        public Client(string ip, int prt)
        {
            local = false;
            connect(ip, prt); 
        }

        /// (Local) Pass world object and create test player
        public Client (bool local, World world, int delay)
        {
            this.local = local;
            this.delay = delay;
            players.Add(new ServerPlayer(0, 0, null));
            players[0].setX(world.getPlayerPos().Item1);
            players[0].setY(world.getPlayerPos().Item2);
        }

        /// Connect to server
        public void connect(string ip, int prt)
        {
            config = new NetPeerConfiguration("FYP");
            client = new NetClient(config);
            client.Start();
            try
            {
                client.Connect(host: ip, port: prt);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        /// (Server) Check if recieved messages from server. (Local) If past delay time then run game logic and return updated logic
        public Tuple<int, int> getMessages(World world)
        {
            Tuple<int, int> pos = null;
            if (!local)
            {
                NetIncomingMessage message;
                while ((message = client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            // Handle server messages
                            var data = message.ReadString();
                            if (data.Length == 1)
                            {
                                ID = data;
                            }
                            else
                            {
                                string[] mess = data.Split('/');
                                string newID = mess[0];
                                int newX = Int32.Parse(mess[1]);
                                int newY = Int32.Parse(mess[2]);
                                if (ID == newID)
                                {
                                    pos = new Tuple<int, int>(newX, newY);
                                }
                                else
                                {
                                    bool newP = true;
                                    for(int i = 0; i < players.Count; i++)
                                    {
                                        if(newID == players[i].getID())
                                        {
                                            newP = false;
                                            players[i].updateEffect(newX);
                                            players[i].setX(newX);
                                            players[i].setY(newY); 
                                        }
                                    }
                                    if (newP)
                                    {
                                        players.Add(new ServerPlayer(newX, newY, null));
                                        players[(players.Count - 1)].setID(newID);
                                    }
                                }
                            }
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            break;

                        default:
                            Console.WriteLine("Unhandled message with type: " + message.MessageType);
                            Console.WriteLine(message.ReadString());
                            break;
                    }
                }
            }
            else
            {
                // Check Timer
                if (lastSent.AddMilliseconds(delay) <= DateTime.Now)
                {
                    // Send Positions
                    lastSent = DateTime.Now;
                    // Process action
                    if (actions.Count > 0)
                    {
                        pos = Logic.actionTree(players[0], world, processAction());
                    }
                    else
                    {
                        pos = new Tuple<int, int>(players[0].getX(), players[0].getY());
                    }
                    if (world.checkColliding(pos.Item1, pos.Item2, players[0].getHeight(), players[0].getWidth()) == 5)
                    {
                        Tuple<int, int> originalPos = world.getPlayerPos();
                        for (int i = 0; i < players.Count; i++)
                        {
                            players[0].setX(originalPos.Item1);
                            players[0].setY(originalPos.Item2);
                        }
                    }
                    else
                    {
                        pos = Logic.update(players[0], pos, world);
                        players[0].updateEffect(pos.Item1);
                        players[0].setX(pos.Item1);
                        players[0].setY(pos.Item2);
                    }
                }
            }
            return pos;
        }

        /// (Server) Send message to server. (Local) Only store action in actions array
        public void sendMessages(string action)
        {
            if (!local)
            {
                NetOutgoingMessage message = client.CreateMessage();
                string msg = ID + '/' + action + '/' + DateTime.Now.TimeOfDay.ToString();
                message.Write(msg);
                client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            }
            actions.Add(action);
        }

        /// Return connection status
        public int getStatus()
        {
            return (int)client.ConnectionStatus;
        }

        /// Return latest action
        public string getAction()
        {
            return actions[0];
        }

        /// Created for testing purposes
        public string processAction()
        {
            string action = actions[0];
            actions.RemoveAt(0);
            return action;
        }

        /// (Server) Return array of connected players. (Local) Return test player
        public List<ServerPlayer> getPlayers()
        {
            return players;
        }
    }
}
