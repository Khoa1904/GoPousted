using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GoPOS.NetworkPacketProcess;

public class NicePayLibrary
{
    private static readonly Lazy<NicePayLibrary> Instance = new(() => new NicePayLibrary());
    private readonly object _locker;

    private NicePayLibrary()
    {
        _locker = new object();
    }

    public static NicePayLibrary Go => Instance.Value;

    /* Return Value(int)
            1 : 정상
            0 : 단말기 Recv Timeout
           -1 : PORT OPEN 실패
           -2 : PORT OPEN 되어 있는 상태
           -3 : ACK 에러 (단말기 반응 없을경우)
           -4 : LRC 에러, 취소
            9 : 사용자정의(exception error)
         */
    [DllImport(@"ExtDLL\PosToCatReq.dll", CharSet = CharSet.Unicode)]
    private static extern int ReqToCat(int port, int baud, byte[] sendBuf, byte[] recvBuf);

    public int CPayToCat(byte[] sendBuf, byte[] recvBuf)
    {
        lock (_locker)
        {
            try
            {
                var port = 3; // ex)포트넘버 : IoC.Get<FormSettingViewModel>().PortNumberSaved.ToNumber();
                return port < 1 ? 9 : ReqToCat(port, 9600, sendBuf, recvBuf);
            }
            catch
            {
                return 9;
            }
        }
    }

    [DllImport(@"ExtDLL\PosToCatReq.dll", CharSet = CharSet.Unicode)]
    private static extern int ReqStop();

    public int CPayStop()
    {
        lock (_locker)
        {
            try
            {
                return ReqStop();
            }
            catch
            {
                return 9;
            }
        }
    }

    [DllImport(@"DLL\VPOS.dll", EntryPoint = "GetPosCode", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.SysInt)]
    private static extern IntPtr GetPosCode([MarshalAs(UnmanagedType.LPWStr)] string inputStr);

    public string GetPosCodeEx()
    {
        lock (_locker)
        {
            return Marshal.PtrToStringAuto(GetPosCode("*nice*pay*")) ?? "ERROR";
        }
    }

#pragma warning disable CA2101 // P/Invoke 문자열 인수에 대해 마샬링을 지정하세요.
    [DllImport(@"ExtDLL\NicePosMegaGift.dll", CharSet = CharSet.Ansi)]
#pragma warning restore CA2101 // P/Invoke 문자열 인수에 대해 마샬링을 지정하세요.
    private static extern int Pos_Send(string ip, int port, string sendData, string signData, byte[] recvData);

    public int PosSendSale(string ip, int port, string sendData, string signData, byte[] recvData)
    {
        lock (_locker)
        {
            return Encoding.ASCII.GetByteCount(sendData) == 286 ? Pos_Send(ip, port, sendData, signData, recvData) : 0;
        }
    }

    public int PosSendMms(string ip, int port, string sendData, string signData, byte[] recvData)
    {
        lock (_locker)
        {
            return Encoding.ASCII.GetByteCount(sendData) == 353 ? Pos_Send(ip, port, sendData, signData, recvData) : 0;
        }
    }
}
