using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoPOS.SerialPacketProcess;

public class PacketPrinter
{
    private static readonly Lazy<PacketPrinter> Instance = new(() => new PacketPrinter());
    private readonly List<string> _list;
    private readonly object _locker;
    private bool _continue;

    private PacketPrinter()
    {
        _locker = new object();
        _list = new List<string>();
    }

    public static PacketPrinter Go => Instance.Value;

    public void AddBuffer(string buffer)
    {
        lock (_locker)
        {
            _list.Add(buffer);
        }
    }

    private string GetBuffer()
    {
        lock (_locker)
        {
            if (_list.Count == 0)
            {
                return string.Empty;
            }

            var tmp = _list.ElementAt(0);
            _list.RemoveAt(0);
            return tmp;
        }
    }

    private void RemoveAllBuffer()
    {
        lock (_locker)
        {
            _list.OfType<IDisposable>().ForIn(x => x.Dispose());
            _list.Clear();
        }
    }

    public void PacketPrinterThreadStop()
    {
        _continue = false;
        RemoveAllBuffer();
        LogHelper.Logger.Info("Serial PacketPrinter STOP");
    }

    public async void PacketPrinterThreadStart(string portName, int baudRate)
    {
        _continue = true;
        LogHelper.Logger.Info("Serial PacketPrinter START");

        while (true)
        {
            if (_continue == false)
            {
                break;
            }

            await Task.Run(() =>
            {
                var buffer = GetBuffer();
                if (!string.IsNullOrWhiteSpace(buffer))
                {
                    PrintPacket(buffer, portName, baudRate);
                }

                Thread.Sleep(50);
            });
        }
    }

    private static void PrintPacket(string buffer, string portName, int baudRate)
    {
        try
        {
            SerialPortWriter.Go.Open(portName, baudRate);
            SerialPortWriter.Go.SendString(TypeHelper.HexStringToString(buffer));
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("PacketPrinter 에러 : " + ex.Message);
            //WindowHelper.InfoMessage("PacketPrinter(프린터출력) 에러 : " + ex.Message);
        }
        finally
        {
            SerialPortWriter.Go.Close();
        }
    }
}
