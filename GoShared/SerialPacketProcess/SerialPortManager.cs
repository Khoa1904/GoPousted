using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace GoPOS.SerialPacketProcess;

public class SerialPortManager
{
    private static readonly Lazy<SerialPortManager> Lazy = new(() => new SerialPortManager());

    private readonly SerialPort _serialPort;
    private volatile bool _keepReading;
    private Thread? _readThread;

    private SerialPortManager()
    {
        _serialPort = new SerialPort();
        _readThread = null;
        _keepReading = false;
    }

    public static SerialPortManager Go => Lazy.Value;

    public bool IsOpen => _serialPort.IsOpen;

    public event EventHandler<string>? OnStatusChanged;
    public event EventHandler<string>? OnDataReceived;
    public event EventHandler<bool>? OnSerialPortOpened;

    /*
    <param name="portname">COM1 / COM3 / COM4 / etc.</param>
    <param name="baudrate">0 / 100 / 300 / 600 / 1200 / 2400 / 4800 / 9600 / 14400 / 19200 / 38400 / 56000 / 57600 / 115200 / 128000 / 256000</param>
    <param name="parity">None / Odd / Even / Mark / Space</param>
    <param name="databits">5 / 6 / 7 / 8</param>
    <param name="stopbits">None / One / Two / OnePointFive</param>
    <param name="handshake">None / XOnXOff / RequestToSend / RequestToSendXOnXOff</param>
    */
    public void Open(string portname = "COM21", int baudrate = 9600, Parity parity = Parity.None, int databits = 8, StopBits stopbits = StopBits.One, Handshake handshake = Handshake.None)
    {
        Close();
        try
        {
            _serialPort.PortName = portname;
            _serialPort.BaudRate = baudrate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = databits;
            _serialPort.StopBits = stopbits;
            _serialPort.Handshake = handshake;
            _serialPort.ReadTimeout = 50;
            _serialPort.WriteTimeout = 50;
            _serialPort.Open();
            StartReading();
        }
        catch (IOException)
        {
            OnStatusChanged?.Invoke(this, $"{portname} does not exist.");
        }
        catch (UnauthorizedAccessException)
        {
            OnStatusChanged?.Invoke(this, $"{portname} already in use.");
        }
        catch (Exception ex)
        {
            OnStatusChanged?.Invoke(this, "Error: " + ex.Message);
        }

        if (_serialPort.IsOpen)
        {
            var sb = StopBits.None.ToString()[..1];
            switch (_serialPort.StopBits)
            {
                case StopBits.One:
                    sb = "1";
                    break;
                case StopBits.OnePointFive:
                    sb = "1.5";
                    break;
                case StopBits.Two:
                    sb = "2";
                    break;
                case StopBits.None:
                    break;
                default:
                    break;
            }

            var p = _serialPort.Parity.ToString()[..1];
            var hs = _serialPort.Handshake == Handshake.None ? "No Handshake" : _serialPort.Handshake.ToString();
            OnStatusChanged?.Invoke(this, $"Connected to {_serialPort.PortName}: {_serialPort.BaudRate} bps, {_serialPort.DataBits}{p}{sb}, {hs}.");
            OnSerialPortOpened?.Invoke(this, true);
        }
        else
        {
            OnStatusChanged?.Invoke(this, $"{portname} already in use OR unable to open serial port");
            OnSerialPortOpened?.Invoke(this, false);
        }
    }

    public void Close()
    {
        StopReading();
        _serialPort.Close();
        OnStatusChanged?.Invoke(this, "Connection closed.");
        OnSerialPortOpened?.Invoke(this, false);
    }

    public void SendString(string message)
    {
        if (!_serialPort.IsOpen)
        {
            return;
        }

        try
        {
            _serialPort.Write(message);
            OnStatusChanged?.Invoke(this, $"Message sent: {message}");
        }
        catch (Exception ex)
        {
            OnStatusChanged?.Invoke(this, $"Failed to send string: {ex.Message}");
        }
    }

    private void StartReading()
    {
        if (_keepReading)
        {
            return;
        }

        _keepReading = true;
        _readThread = new Thread(ReadPort);
        _readThread.Start();
    }

    private void StopReading()
    {
        if (!_keepReading)
        {
            return;
        }

        _keepReading = false;
        _readThread?.Join();
        _readThread = null;
    }

    private void ReadPort()
    {
        while (_keepReading)
        {
            if (_serialPort.IsOpen)
            {
                var readBuffer = new byte[_serialPort.ReadBufferSize + 1];
                try
                {
                    var count = _serialPort.Read(readBuffer, 0, _serialPort.ReadBufferSize);
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var data = Encoding.GetEncoding(51949).GetString(readBuffer, 0, count);
                    OnDataReceived?.Invoke(this, data);
                }
                catch (TimeoutException)
                {
                }
            }
            else
            {
                TimeSpan waitTime = new(0, 0, 0, 0, 50);
                Thread.Sleep(waitTime);
            }
        }
    }
}
