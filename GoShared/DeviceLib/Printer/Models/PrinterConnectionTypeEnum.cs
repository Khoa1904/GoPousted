using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.DeviceLib.Printer.Models
{
    /// <summary>
    /// 프린터 접속 방법
    /// </summary>
    public enum PrinterConnectionTypeEnum
    {
        NotSpecified,   // 선택할 필요 없는 경우
        SerialPort,     // 시리얼 포트
        TcpIp           // TCP/IP 연결
    }

}
