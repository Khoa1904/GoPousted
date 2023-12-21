using System;
using System.Runtime.InteropServices;

namespace GoPOS.SerialPacketProcess;

public class HeeyamLibrary
{
    private static readonly Lazy<HeeyamLibrary> Instance = new(() => new HeeyamLibrary());
    private readonly object _locker;

    private HeeyamLibrary()
    {
        _locker = new object();
    }

    public static HeeyamLibrary Go => Instance.Value;

    [DllImport(@"ExtDLL\HeeyamParseLib.dll", EntryPoint = "LibPingTest", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static extern IntPtr LibPingTest([MarshalAs(UnmanagedType.LPWStr)] string inputString);

    /// <summary>
    ///     HeeyamParseLib : PingTest
    /// </summary>
    /// <param name="inputString">string</param>
    /// <returns>string</returns>
    public string GetLibPingTest(string inputString)
    {
        lock (_locker)
        {
            return Marshal.PtrToStringAuto(LibPingTest(inputString)) ?? "ERROR";
        }
    }

    [DllImport(@"ExtDLL\HeeyamParseLib.dll", EntryPoint = "RawHexToJson", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static extern IntPtr RawHexToJson([MarshalAs(UnmanagedType.LPWStr)] string inputString);

    /// <summary>
    ///     HeeyamParseLib : RawHexToText
    /// </summary>
    /// <param name="inputString">string</param>
    /// <returns>string</returns>
    public string GetRawHexToJson(string inputString)
    {
        lock (_locker)
        {
            return Marshal.PtrToStringAuto(RawHexToJson(inputString)) ?? "ERROR";
        }
    }
}
