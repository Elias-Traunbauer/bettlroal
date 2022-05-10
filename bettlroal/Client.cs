using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bettlroal
{
    class Client
    {
        Socket server;
        NetworkStream stream;
        BinaryFormatter binaryFormatter;

        public Client()
        {
            binaryFormatter = new BinaryFormatter();
        }

        public event EventHandler<NetworkData> RecievedMessage;

        public void SendMessage(Message msg)
        {
            NetworkData d = new NetworkData();
            d.msgs.Add(msg);

            lock (stream)
            {
                binaryFormatter.Serialize(stream, d);
            }
        }

        private void ServerClientLoop()
        {
            while (true)
            {
                while(!stream.DataAvailable)
                {
                    Thread.Sleep(25);
                }
                NetworkData msg = (NetworkData)binaryFormatter.Deserialize(stream);
                if (msg.type == NetworkData.DataType.Message)
                {
                    RecievedMessage.Invoke(this, msg);
                }
                else
                {
                    if (Stream.instance != null)
                    {
                        Stream.instance.UpdateImage(msg);
                    }
                }
            }
        }

        public void Connect(IPAddress ip, int port)
        {
            server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ip, port);
            stream = new NetworkStream(server);

            Thread d = new Thread(new ThreadStart(ServerClientLoop));
            d.Name = "Server Communication Thread";
            d.IsBackground = true;
            d.Start();
        }
    }
}
