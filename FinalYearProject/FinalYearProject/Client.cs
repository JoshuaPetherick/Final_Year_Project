using System;
using Lidgren.Network;
//https://github.com/lidgren/lidgren-network-gen3

namespace FinalYearProject
{
    class Client
    {
        public string ID = "1"; // ID assigned by Server
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

        public void sendMessages(string action)
        {
            NetOutgoingMessage message = client.CreateMessage();
            string msg = ID + action;
            message.Write(msg);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        public int getStatus()
        {
            return (int)client.ConnectionStatus;
        }
    }
}
