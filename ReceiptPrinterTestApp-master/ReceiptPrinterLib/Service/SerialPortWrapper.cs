using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReceiptPrinterLib.Service
{
    internal class SerialPortWrapper
    {
        readonly ILogger logger;

        SerialPort? serialPort;

        public SerialPortWrapper(ILogger log)
        {
            this.logger = log;
        }

        public void Close()
        {
            serialPort?.Close();
            serialPort = null;
        }

        public bool Open(int portNo, int baudRate, int data = 8)
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM" + portNo;
            serialPort.BaudRate = baudRate; 
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;

            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

            Thread.Sleep(100);
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            return true;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] packet = new byte[1024];
            var read = serialPort?.Read(packet, 0, packet.Length);
            if (read.HasValue && read > 0)
                OnRead?.Invoke(this, packet.Take(read.Value).ToArray());
        }

        public void Write(byte[] packet)
        {
            try
            {
                serialPort?.Write(packet, 0, packet.Length);
                //logger.LogDebug("Writing data - " + packet.Length + " bytes");
                //logger.LogTrace("DATA - " + String.Join(" ", packet.Select(a => a.ToString("X2")).ToArray()));

            }
            catch(Exception e)
            {
                logger.LogError(e, "전송 중 오류");
            }
        }

        public delegate void OnReadEvent(object sender, byte[] buffer);
        public event OnReadEvent OnRead;
    }
}
