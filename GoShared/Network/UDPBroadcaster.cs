using log4net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Network
{
    /// <summary>
    /// 네트워크에 UDP 로 전파
    /// </summary>
    [TransientClass]
    public class UDPBroadcaster
    {
        readonly ILog log = log4net.LogManager.GetLogger(typeof(UDPBroadcaster));

        readonly static Encoding defaultStringEncoding = Encoding.UTF8;
        readonly Socket socket;
        int targetPort = 0;
        IPEndPoint? target;

        public UDPBroadcaster()
        {
            socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.EnableBroadcast = true;
            socket.Ttl = 2;
        }

        public void Prepare(int targetPort)
        {
            this.targetPort = targetPort;
            target = new IPEndPoint(IPAddress.Broadcast, targetPort);
        }
        public void Broadcast(string src)
        {
            Broadcast(defaultStringEncoding.GetBytes(src));
        }

        public void Broadcast(byte[] src) 
        {
            _ = socket.SendTo(src, target);
        }
    }
}
