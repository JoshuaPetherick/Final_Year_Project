using System;
using Lidgren.Network;
using System.Collections.Generic;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public string ID = "1"; // ID assigned by Server
        private NetPeerConfiguration config;
        private NetClient client;

        private List<string> actions = new List<string>(); // Array of Actions
        private double delay = 50; // Millisecond Delay
        private DateTime lastSent = DateTime.Now;
        private Player localPlayer = new Player(0, 0);
        public bool local = false; 

        public Client (string ip, int prt)
        {
            if (!local)
            {
                connect(ip, prt);
            }
        }

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

        public Tuple<int, int> getMessages(World world)
        {
            Tuple<int, int> pos = new Tuple<int, int>(localPlayer.getX(), localPlayer.getY());
            if (!local)
            {
                NetIncomingMessage message;
                while ((message = client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            // handle server messages
                            var data = message.ReadString();
                            Console.WriteLine(data);
                            if (data.Length < 1)
                            {
                                pos = new Tuple<int, int>(data[0], data[1]);
                            }
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            break;

                        default:
                            Console.WriteLine("Unhandled message with type: " + message.MessageType);
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
                        string action = actions[0];
                        actions.RemoveAt(0);
                        pos = Logic.actionTree(localPlayer, action);
                    }
                }
                pos = Logic.update(localPlayer, pos, world);
                localPlayer.setX(pos.Item1);
                localPlayer.setY(pos.Item2);
            }
            return pos;
        }

        public void sendMessages(string action)
        {
            if (!local)
            {
                NetOutgoingMessage message = client.CreateMessage();
                string msg = ID + action;
                message.Write(msg);
                client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            }
            else
            {
                actions.Add(action);
            }
        }

        public int getStatus()
        {
            return (int)client.ConnectionStatus;
        }
    }
}
