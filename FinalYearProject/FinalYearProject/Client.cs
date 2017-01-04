using System;
using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public int ID; // ID assigned by Server
        private NetPeerConfiguration config;
        private NetClient client;

        Technique technique;

        public Client (string ip, int prt, Technique technique)
        {
            config = new NetPeerConfiguration("FYP");
            client = new NetClient(config);
            client.Start();
            try
            {
                client.Connect(host: ip, port: prt);
                this.technique = technique;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        public void updatePlayer(int action)
        {
            technique.update(action);
        }

        public void getMessages()
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
