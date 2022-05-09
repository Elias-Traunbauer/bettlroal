using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bettlroal
{
    class Server
    {
        Socket server;
        BinaryFormatter binaryFormatter;

        List<NetworkStream> clients;
        public List<Message> messageBuffer;

        public Server()
        {
            clients = new List<NetworkStream>();
            messageBuffer = new List<Message>();
            binaryFormatter = new BinaryFormatter();
        }

        public event EventHandler<NetworkData> RecievedMessage;

        int lastBroadcast;

        public void BroadcastMessages()
        {
            Debug.WriteLine(Environment.TickCount - lastBroadcast);
            if (Environment.TickCount - lastBroadcast > 50)
            {
                lastBroadcast = Environment.TickCount;
                NetworkData d = new NetworkData();
                lock (messageBuffer)
                {
                    d.msgs = messageBuffer;
                    foreach (var item in clients)
                    {
                        try
                        {
                            binaryFormatter.Serialize(item, d);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                messageBuffer.Clear();
            }
        }

        private void ServerClientLoop(Object obj)
        {
            Socket client = (Socket)obj;
            NetworkStream stream = new NetworkStream(client);
            clients.Add(stream);
            server.ReceiveBufferSize = 8;
            server.SendBufferSize = 8;

            while (true)
            {
                while(!stream.DataAvailable)
                {
                    Thread.Sleep(25);
                }
                Debug.WriteLine("Data avai");
                NetworkData msg = (NetworkData)binaryFormatter.Deserialize(stream);
                messageBuffer.Add(msg.msgs[0]);
                Debug.WriteLine("Count " + msg.msgs.Count);
                BroadcastMessages();
                RecievedMessage.Invoke(this, msg);
            }
        }

        public void StartServer()
        {
            server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, 60900));
            server.Listen(100);
            Thread d = new Thread(ServerLoop);
            d.IsBackground = true;
            d.Start();
        }

        private void ServerLoop()
        {
            while (true)
            {
                Socket s = server.Accept();
                Thread d = new Thread(new ParameterizedThreadStart(ServerClientLoop));
                d.IsBackground = true;
                d.Start(s);
            }
        }
    }
}
