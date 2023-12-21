using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;

namespace GoShared.Helpers
{
    public static class SystemTime
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME theDateTime);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
            public string wNow;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct REG_TIME_ZONE_INFORMATION
        {
            public int Bias;
            public int StandardBias;
            public int DaylightBias;
            public SYSTEMTIME StandardDate;
            public SYSTEMTIME DaylightDate;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct TIME_ZONE_INFORMATION
        {
            public int Bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string StandardName;
            public SYSTEMTIME StandardDate;
            public int StandardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DaylightName;
            public SYSTEMTIME DaylightDate;
            public int DaylightBias;
        }


        public static void SetTime(string time)
        {
            SetTime(time, true);
        }

        /// <summary>
        /// 시스템시간 설정
        /// </summary>
        /// <param name="systemTime"></param>
        public static void SetTime(string time, bool fromUTC)
        {
            DateTime sys = DateTime.UtcNow;
            SYSTEMTIME sTime = new SYSTEMTIME();
            DateTime sysTime = DateTime.SpecifyKind(DateTime.ParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture), DateTimeKind.Utc);

            sTime.wYear = (ushort)sysTime.Year;
            sTime.wMonth = (ushort)sysTime.Month;
            sTime.wDay = (ushort)(sysTime.Day);
            sTime.wHour = (ushort)(sysTime.Hour);
            sTime.wMinute = (ushort)(sysTime.Minute);
            sTime.wSecond = (ushort)(sysTime.Second);
            sTime.wNow = sysTime.Year.ToString() + sysTime.Month.ToString() + sysTime.Day.ToString();
            var status = SetSystemTime(ref sTime);
        }
    }
}
