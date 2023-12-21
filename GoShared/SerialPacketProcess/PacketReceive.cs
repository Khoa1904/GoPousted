using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoPOS.SerialPacketProcess;

public class PacketReceive
{
    private static readonly Lazy<PacketReceive> Instance = new(() => new PacketReceive());
    private readonly List<string> _list;
    private readonly object _locker;
    private bool _continue;

    private PacketReceive()
    {
        _locker = new object();
        _list = new List<string>();
    }

    public static PacketReceive Go => Instance.Value;

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

    public void PacketReceiveThreadStop()
    {
        _continue = false;
        RemoveAllBuffer();
        LogHelper.Logger.Info("Serial PacketReceive STOP");
    }

    public async void PacketReceiveThreadStart()
    {
        _continue = true;
        LogHelper.Logger.Info("Serial PacketReceive START");

        while (true)
        {
            if (_continue == false)
            {
                break;
            }

            await Task.Run(() =>
            {
                var buffer = GetBuffer();
                if (!string.IsNullOrEmpty(buffer))
                {
                    ParsePacket(buffer);
                }

                Thread.Sleep(50);
            });
        }
    }

    private static void ParsePacket(string buffer)
    {
        try
        {
            PacketPrinter.Go.AddBuffer(buffer);

            string json = HeeyamLibrary.Go.GetRawHexToJson(buffer).TrimSafe();
            if (!string.IsNullOrEmpty(json))
            {
                //PacketProcessData.Go.AddBuffer(json, buffer); // 추가 버퍼처리(화면, DB 등)
                LogHelper.Logger.Info("PacketReceiveFirst JSON : " + json.LogEncrypt());
            }
            else
            {
                LogHelper.Logger.Info("PacketReceiveFirst JSON Error : Return Empty");
            }

            LogHelper.Logger.Info("PacketReceiveFirst Buffer : " + buffer.LogEncrypt());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("PacketReceiveFirst 에러 : " + ex.Message);
        }
    }
}
