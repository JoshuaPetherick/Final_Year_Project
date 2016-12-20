﻿using System;
using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public int ID; // ID assigned by Server
        private NetPeerConfiguration config;
        private NetClient client;

        public Client (string ip, int prt)
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

        public void getMessages()
        {
            NetIncomingMessage message;
            while ((message = client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        var data = message.ReadString();
                        Console.WriteLine(data);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
        }

        public void sendMessages(int msg)
        {
            NetOutgoingMessage message = client.CreateMessage();
            message.Write(ID + "" + msg);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public int getStatus()
        {
            return (int)client.ConnectionStatus;
        }
    }
}
