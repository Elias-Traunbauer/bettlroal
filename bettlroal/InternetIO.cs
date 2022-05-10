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
using Mono.Nat;

namespace bettlroal
{
    class InternetIO
    {
        private static InternetIO _instance;

        public static InternetIO instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InternetIO();
                }
                return _instance;
            }
        }

        public event EventHandler<NetworkData> MessagesUpdate;

        public IOType mode = IOType.Client;

        INatDevice device;

        public InternetIO ()
        {
            NatUtility.DeviceFound += NatUtility_DeviceFound;
            NatUtility.StartDiscovery();
        }

        private void NatUtility_DeviceFound(object sender, DeviceEventArgs e)
        {
            device = e.Device;
        }

        public void DeleteMapping()
        {
            if (mode == IOType.Server)
            {
                if (device != null)
                {
                    device.DeletePortMap(new Mapping(Protocol.Tcp, 60900, 60900));
                }
            }
        }

        public void SendImageUpdate(NetworkData d)
        {
            if (mode == IOType.Client)
            {
                
            }
            else
            {
                server.BroadcastData(d);
            }
        }

        public string StartServer(bool portf)
        {
            if (device != null && portf)
            {
                device.CreatePortMap(new Mapping(Protocol.Tcp, 60900, 60900));
                mode = IOType.Server;
                server = new Server();
                server.StartServer();
                server.RecievedMessage += MessagesUpdate;
                return device.GetExternalIP().ToString();
            }
            else if (!portf)
            {
                mode = IOType.Server;
                server = new Server();
                server.StartServer();
                server.RecievedMessage += MessagesUpdate;
                IPAddress ip = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();
                return ip.ToString();
            }
            else
            {
                return "error";
            }
        }

        Client client;
        Server server;

        public void Connect(IPAddress ip, int port)
        {
            mode = IOType.Client;
            client = new Client();
            client.Connect(ip, port);
            client.RecievedMessage += MessagesUpdate;
        }

        public void FlushMsgs()
        {
            server.BroadcastMessages();
        }

        public void SendMessage(Message msg)
        {
            if(mode == IOType.Client)
            {
                client.SendMessage(msg);
            }
            else
            {
                server.messageBuffer.Add(msg);
                server.BroadcastMessages();
                NetworkData d = new NetworkData();
                d.msgs.Add(msg);
                MessagesUpdate.Invoke(this, d);
            }
        }

        public enum IOType
        {
            Client,
            Server
        }

    }
}
