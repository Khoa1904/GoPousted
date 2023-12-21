using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace GoPOS.SerialPacketProcess;

public static class SerialManager
{
    private const string _YG = "1D5601";
    private const string _BM = "1B69";
    private static string mReceivedStr = string.Empty;

    public static void StartSerialPortEvent(string readPort, int readBaudrate, string writePort, int writeBaudrate)
    {
        if (!SerialPortList().Any())
        {
            LogHelper.Logger.Warn("설치한 시리얼포트가 없습니다");
            //WindowHelper.InfoMessage("설치한 시리얼포트가 없습니다");
            return;
        }

        foreach ((string value, int i) in SerialPortList().Select((value, i) => (value, i)))
        {
            LogHelper.Logger.Info("설치한 시리얼포트 : " + (i + 1) + " : " + value);
        }

        int r = 0;
        int w = 0;

        for (int i = 0; i < SerialPortList().Count; i++)
        {
            if (SerialPortList()[i] == readPort)
            {
                r++;
            }

            if (SerialPortList()[i] == writePort)
            {
                w++;
            }
        }

        if (r > 0)
        {
            OpenSerialPortManager(readPort, readBaudrate);
            LogHelper.Logger.Info("설정한(Read) 시리얼포트 : " + readPort + " / " + readBaudrate.ToString());

            SerialPortManager.Go.OnDataReceived += SerialPortManager_OnDataReceived;
            SerialPortManager.Go.OnStatusChanged += SerialPortManager_OnStatusChanged;

            PacketReceive.Go.PacketReceiveThreadStart();
        }
        else
        {
            LogHelper.Logger.Info($"설정한(Read) {readPort} 시리얼포트 없음 : Serial 장치에 설정한 목록이 없습니다");
        }

        if (w > 0)
        {
            OpenSerialPortWriter(writePort, writeBaudrate);
            LogHelper.Logger.Info("설정한(Write) 시리얼포트 : " + writePort + " / " + writeBaudrate.ToString());

            SerialPortWriter.Go.OnDataReceived += SerialPortWriter_OnDataReceived;
            SerialPortWriter.Go.OnStatusChanged += SerialPortWriter_OnStatusChanged;

            PacketPrinter.Go.PacketPrinterThreadStart(writePort, writeBaudrate);
        }
        else
        {
            LogHelper.Logger.Info($"설정한(Write) {writePort} 시리얼포트 없음 : Serial 장치에 설정한 목록이 없습니다");
        }
    }

    private static List<string> SerialPortList()
    {
        return SerialPort.GetPortNames().ToList();
    }

    private static void OpenSerialPortManager(string portName, int baudRate)
    {
        try
        {
            SerialPortManager.Go.Open(portName, baudRate); // 포트 지정하기

            if (!SerialPortManager.Go.IsOpen)
            {
                LogHelper.Logger.Warn("수신(receive) 시리얼포트에 연결할 수 없습니다");
                //WindowHelper.InfoMessage("수신(receive) 시리얼포트에 연결할 수 없습니다");
            }

            LogHelper.Logger.Info("수신(receive) 시리얼포트 동작 중...");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("수신(receive) 시리얼포트 연결 에러 : " + ex.Message);
            //WindowHelper.InfoMessage("수신(receive) 시리얼포트 연결 에러 ; " + ex.Message);
        }
    }

    private static void OpenSerialPortWriter(string portName, int baudRate)
    {
        try
        {
            SerialPortWriter.Go.Open(portName, baudRate); // 포트 지정하기

            if (!SerialPortWriter.Go.IsOpen)
            {
                LogHelper.Logger.Warn("송신(printer) 시리얼포트에 연결할 수 없습니다");
                //WindowHelper.InfoMessage("송신(printer) 시리얼포트에 연결할 수 없습니다");
            }

            SerialPortWriter.Go.Close();
            LogHelper.Logger.Info("송신(printer) 시리얼포트 대기 중...");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("송신(printer) 시리얼포트 연결 에러 : " + ex.Message);
            //WindowHelper.InfoMessage("송신(printer) 시리얼포트 연결 에러 : " + ex.Message);
        }
    }

    private static void SerialPortManager_OnStatusChanged(object? sender, string statusException)
    {
        try
        {
            LogHelper.Logger.Warn("SerialPort(Read) Status Changed : " + statusException);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("SerialPort(Read) Status Changed Error : " + ex.Message);
        }
    }

    private static void SendBuffer(string buffer)
    {
        PacketReceive.Go.AddBuffer(buffer);
    }

    private static void SerialPortManager_OnDataReceived(object? sender, string data)
    {
        try
        {

            mReceivedStr = mReceivedStr.TrimSafe() + TypeHelper.StringToHexString(data).TrimSafe();
            string strTemp;

            if (mReceivedStr.Contains(_YG))
            {
                strTemp = mReceivedStr;
                mReceivedStr = mReceivedStr[..(mReceivedStr.IndexOf(_YG, StringComparison.Ordinal) + 6)].Trim();
                SendBuffer(mReceivedStr);
                mReceivedStr = strTemp.Remove(0, mReceivedStr.IndexOf(_YG, StringComparison.Ordinal) + 6).Trim();

                if (!mReceivedStr.Contains(_YG))
                {
                    return;
                }

                mReceivedStr = mReceivedStr[..(mReceivedStr.IndexOf(_YG, StringComparison.Ordinal) + 6)].Trim();
                SendBuffer(mReceivedStr);
                mReceivedStr = string.Empty;
            }
            else if (mReceivedStr.Contains(_BM))
            {
                strTemp = mReceivedStr;
                mReceivedStr = mReceivedStr.Substring(0, mReceivedStr.IndexOf(_BM, StringComparison.Ordinal) + 4).Trim();
                SendBuffer(mReceivedStr);
                mReceivedStr = strTemp.Remove(0, mReceivedStr.IndexOf(_BM, StringComparison.Ordinal) + 4).Trim();

                if (!mReceivedStr.Contains(_BM))
                {
                    return;
                }

                mReceivedStr = mReceivedStr[..(mReceivedStr.IndexOf(_BM, StringComparison.Ordinal) + 4)].Trim();
                SendBuffer(mReceivedStr);
                mReceivedStr = string.Empty;
            }
            else
            {
                if (mReceivedStr.Length <= 40960)
                {
                    return;
                }

                mReceivedStr = string.Empty;
                LogHelper.Logger.Warn("처리바이트 초과 : " + mReceivedStr.Length);
            }
        }
        catch (Exception ex)
        {
            mReceivedStr = string.Empty;
            LogHelper.Logger.Error("Receive Serial Data Error : " + ex.Message);
        }
    }

    private static void SerialPortWriter_OnDataReceived(object? sender, string data)
    {
        LogHelper.Logger.Info("SerialPort(Write) and Read : " + TypeHelper.StringToHexString(data).TrimSafe());
    }

    private static void SerialPortWriter_OnStatusChanged(object? sender, string exception)
    {
        try
        {
            LogHelper.Logger.Info("SerialPort(Write) Status Changed : " + exception);

            if (!exception.Contains("write timed out"))
            {
                return;
            }

            //WindowHelper.InfoMessage("SerialPort(Printer)", "출력할 프린터가 없거나 동작하지 않습니다");
            LogHelper.Logger.Info("출력할 프린터가 없거나 동작하지 않습니다");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("SerialPort(Write) Status Changed Error : " + ex.Message);
        }
    }
}