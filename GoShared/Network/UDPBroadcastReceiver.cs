using log4net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Network
{
    [TransientClass]
    internal class UDPBroadcastReceiver
    {
        readonly ILog log = log4net.LogManager.GetLogger(typeof(UDPBroadcastReceiver));

        UdpClient? udpClient;
        IPEndPoint? ipEndPoint = null;
        AutoResetEvent stopSignal = new AutoResetEvent(false);

        public delegate void OnDataDelegate(byte[] data, IPEndPoint? remoteEP);
        public OnDataDelegate? OnData;

        public UDPBroadcastReceiver()
        {
            udpClient = null;
        }

        public void Listen(int port)
        {
            udpClient = new UdpClient(port, AddressFamily.InterNetwork);
            udpClient.EnableBroadcast = true;
            
            ipEndPoint = new IPEndPoint(IPAddress.Broadcast, port);

            stopSignal.Reset();
        }

        void ReceiveResult(IAsyncResult ar)
        {
            IPEndPoint? ep = null;
            byte[]? data = udpClient?.EndReceive(ar, ref ep);

            if (data == null)
                return;

            try
            {
                OnData?.Invoke(data, ep);
            }
            catch (Exception) 
            { 
            }

            udpClient?.BeginReceive(new AsyncCallback(ReceiveResult), null);
        }

        void ThreadProc()
        {
            // kick read
            udpClient?.BeginReceive(new AsyncCallback(ReceiveResult), null);

            while (!stopSignal.WaitOne(10))
            {
            }

            udpClient?.Close();
        }

        public void Stop()
        {
            stopSignal.Set();
        }
    }
}
