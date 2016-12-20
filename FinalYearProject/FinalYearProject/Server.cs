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
        private List<Player> players = new List<Player>();

        public Server (int port)
        {
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

        // States:
        // 1. Move Left
        // 2. Move Right
        // 3. Jump

        public void checkMessages()
        {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        Console.WriteLine(message.ReadString());
                        //Console.WriteLine(data);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        var dat = message.ReadByte();
                        Console.WriteLine(dat);
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
        }

        public void sendMessages(string msg)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write(msg);
            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
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
