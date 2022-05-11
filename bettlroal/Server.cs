using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
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
            if (Environment.TickCount - lastBroadcast > 50)
            {
                if (messageBuffer.Count == 0)
                {
                    return;
                }
                lastBroadcast = Environment.TickCount;
                NetworkData d = new NetworkData();
                lock (messageBuffer)
                {
                    d.msgs = messageBuffer;
                    foreach (var item in clients)
                    {
                        try
                        {
                            lock (item)
                            {
                                binaryFormatter.Serialize(item, d);
                            }
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

        public void BroadcastData(NetworkData d)
        {
            lock (clients)
            {
                foreach (var item in clients)
                {
                    try
                    {
                        lock (item)
                        {
                            binaryFormatter.Serialize(item, d);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
        }

        private void ServerClientLoop(Object obj)
        {
            Socket client = (Socket)obj;
            NetworkStream stream = new NetworkStream(client);
            lock (clients)
            {
                clients.Add(stream);
            }

            while (true)
            {
                while (!stream.DataAvailable)
                {
                    Thread.Sleep(25);
                }
                NetworkData msg = (NetworkData)binaryFormatter.Deserialize(stream);
                messageBuffer.Add(msg.msgs[0]);
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
            d.Name = "Server Accept Thread";
            d.IsBackground = true;
            d.Start();
        }

        private void ServerLoop()
        {
            while (true)
            {
                Socket s = server.Accept();

                Thread d = new Thread(new ParameterizedThreadStart(ServerClientLoop));
                d.Name = "Client Communication Thread";
                d.IsBackground = true;
                d.Start(s);
            }
        }
    }
}
