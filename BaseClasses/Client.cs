﻿using System;
using Lidgren.Network;
using System.Collections.Generic;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public string ID = "0"; // ID assigned by Server
        private NetPeerConfiguration config;
        private NetClient client;

        public bool local;
        private double delay = 0; // Millisecond Delay
        private DateTime lastSent = DateTime.Now;
        private ServerPlayer localPlayer = new ServerPlayer(0, 0, null);
        private List<string> actions = new List<string>(); // Array of Actions

        public Client(string ip, int prt)
        {
            local = false;
            connect(ip, prt); 
        }

        public Client (bool local)
        {
            this.local = local;
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
            Tuple<int, int> pos = null;
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
                            if (data.Length == 1)
                            {
                                ID = data;
                            }
                            else
                            {
                                string[] mess = data.Split('/');
                                string newID = mess[0];
                                string newX = mess[1];
                                string newY = mess[2];
                                if (ID == newID)
                                {
                                    pos = new Tuple<int, int>(Int32.Parse(newX), Int32.Parse(newY));
                                }
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
                        pos = Logic.actionTree(localPlayer, world, processAction());
                    }
                    else
                    {
                        pos = new Tuple<int, int>(localPlayer.getX(), localPlayer.getY());
                    }
                    pos = Logic.update(localPlayer, pos, world);
                    localPlayer.setX(pos.Item1);
                    localPlayer.setY(pos.Item2);
                }
            }
            return pos;
        }

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

        public int getStatus()
        {
            return (int)client.ConnectionStatus;
        }

        public string getAction()
        {
            return actions[0];
        }

        public string processAction()
        {
            string action = actions[0];
            actions.RemoveAt(0);
            return action;
        }

        public ServerPlayer getPlayer()
        {
            return localPlayer;
        }
    }
}
