using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using GoShared;
using GoShared.Helpers;

namespace GoPOS.Common.Helpers
{
    public static class WindowsHelper
    {
        #region Windows
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void DragMoveWindow()
        {
            ReleaseCapture();
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, 0x112, 0xf012, 0);
        }

        public static void BringToFront(string title)
        {
            var handle = FindWindow(null, title);

            if (handle == IntPtr.Zero)
            {
                return;
            }

            _ = SetForegroundWindow(handle);
        }

        public static async Task<string> GetLocalIp()
        {
            return await Task.Run(() =>
            {
                try
                {
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                    string clientIp = string.Empty;

                    foreach (IPAddress addressList in host.AddressList)
                    {
                        if (addressList.AddressFamily == AddressFamily.InterNetwork)
                        {
                            clientIp = addressList.ToString();
                            break;
                        }
                    }

                    return clientIp.TrimSafe();
                }
                catch
                {
                    return string.Empty;
                }
            });
        }

        public static async Task<string> GetPublicIp()
        {
            try
            {
                using var client = new HttpClient();

                using HttpResponseMessage response = await client.GetAsync("https://ipinfo.io/ip");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return await GetLocalIp();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void StartWindowCenter(Window window)
        {
            Rect workArea = SystemParameters.WorkArea;
            window.Left = (workArea.Width - window.Width) / 2 + workArea.Left;
            window.Top = (workArea.Height - window.Height) / 2 + workArea.Top;
        }

        public static void RestartMainWindow()
        {
            var processModule = Process.GetCurrentProcess().MainModule;

            if (processModule is not { FileName: { } })
            {
                return;
            }

            Process.Start(processModule.FileName);
            Application.Current.Shutdown();
        }

        private static WindowChrome GetChrome()
        {
            Thickness t = SystemParameters.WindowResizeBorderThickness;

            WindowChrome customChrome = new()
            {
                GlassFrameThickness = new Thickness(0, 0, 0, 0),
                CornerRadius = new CornerRadius(0, 0, 0, 0),
                CaptionHeight = 0,
                NonClientFrameEdges = NonClientFrameEdges.None,
                ResizeBorderThickness = new Thickness(t.Left, t.Top, t.Right, t.Bottom)
            };
            return customChrome;
        }

        private static void UndoFullscreen(Window window)
        {
            WindowChrome.SetWindowChrome(window, GetChrome());
        }

        public static void WindowNormal(Window window)
        {
            UndoFullscreen(window);
            window.WindowState = WindowState.Normal;
        }

        public static void WindowMax(Window window)
        {
            UndoFullscreen(window);
            window.WindowState = WindowState.Maximized;
        }

        public static void WindowFull(Window window)
        {
            window.WindowState = WindowState.Maximized;
            WindowChrome.SetWindowChrome(window, null);
        }

        public static void MaxMinChangeScreen(Window window)
        {
            if (window.WindowState != WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 모니터크기와 윈도우크기가 같으면 전체화면으로 전환
        /// </summary>
        /// <param name="window">Window Object(MainWindow etc)</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FullscreenSize(Window window, int width, int height)
        {
            var w = Convert.ToInt32(SystemParameters.PrimaryScreenWidth);
            var h = Convert.ToInt32(SystemParameters.PrimaryScreenHeight);

            if (w != width || h != height)
            {
                window.AllowsTransparency = true;
                window.Topmost = true;

                var geometryDefault = new RectangleGeometry
                {
                    RadiusX = 10,
                    RadiusY = 10,
                    Rect = new Rect(0, 0, width, height)
                };
                window.Clip = geometryDefault;
                return;
            }

            window.AllowsTransparency = false;
            window.Topmost = true;
            window.WindowState = WindowState.Maximized;

            var geometry = new RectangleGeometry
            {
                RadiusX = 0,
                RadiusY = 0,
                Rect = new Rect(0, 0, width, height)
            };
            window.Clip = geometry;
        }

        #endregion

        #region Keyboard
        [DllImport("user32.dll")]
        private static extern UInt32 SendInput(UInt32 nInputs, ref INPUT pInputs, int cbSize);
        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        private static extern IntPtr GetMessageExtraInfo();
        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }
        [Flags()]
        private enum MOUSEEVENTF
        {
            MOVE = 0x0001,  //mouse move     
            LEFTDOWN = 0x0002,  //left button down     
            LEFTUP = 0x0004,  //left button up     
            RIGHTDOWN = 0x0008,  //right button down     
            RIGHTUP = 0x0010,  //right button up     
            MIDDLEDOWN = 0x0020, //middle button down     
            MIDDLEUP = 0x0040,  //middle button up     
            XDOWN = 0x0080,  //x button down     
            XUP = 0x0100,  //x button down     
            WHEEL = 0x0800,  //wheel button rolled     
            VIRTUALDESK = 0x4000,  //map to entire virtual desktop     
            ABSOLUTE = 0x8000,  //absolute move     
        }
        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public Int32 type;//0-MOUSEINPUT;1-KEYBDINPUT;2-HARDWAREINPUT     
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public Int32 dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public Int16 wVk;
            public Int16 wScan;
            public Int32 dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }
        //Simulate mouse   
        public static void Click()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;
            SendInput(1, ref input_down, Marshal.SizeOf(input_down));
            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;
            SendInput(1, ref input_up, Marshal.SizeOf(input_up));
        }
        //Simulate keystrokes  Send unicode characters to send any character
        public static void SendUnicode(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                INPUT input_down = new INPUT();
                input_down.type = (int)InputType.INPUT_KEYBOARD;
                input_down.ki.dwFlags = (int)KEYEVENTF.UNICODE;
                input_down.ki.wScan = (short)message[i];
                input_down.ki.wVk = 0;
                SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     
                INPUT input_up = new INPUT();
                input_up.type = (int)InputType.INPUT_KEYBOARD;
                input_up.ki.wScan = (short)message[i];
                input_up.ki.wVk = 0;
                input_up.ki.dwFlags = (int)(KEYEVENTF.KEYUP | KEYEVENTF.UNICODE);
                SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      
            }
        }
        //Simulate keystrokes 
        public static void SendKeyBoradKey(short key)
        {
            INPUT input_down = new INPUT();
            input_down.type = (int)InputType.INPUT_KEYBOARD;
            input_down.ki.dwFlags = 0;
            input_down.ki.wVk = key;
            SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     

            INPUT input_up = new INPUT();
            input_up.type = (int)InputType.INPUT_KEYBOARD;
            input_up.ki.wVk = key;
            input_up.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      

        }
        //Send non-unicode characters, only send lowercase letters and numbers (发送非unicode字符，只能发送小写字母和数字)     
        public static void SendNoUnicode(string message)
        {
            string str = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < message.Length; i++)
            {
                short sendChar = 0;
                if (str.IndexOf(message[i].ToString().ToLower()) > -1)
                    sendChar = (short)GetKeyByChar(message[i]);
                else
                    sendChar = (short)message[i];
                INPUT input_down = new INPUT();
                input_down.type = (int)InputType.INPUT_KEYBOARD;
                input_down.ki.dwFlags = 0;
                input_down.ki.wVk = sendChar;
                SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     
                INPUT input_up = new INPUT();
                input_up.type = (int)InputType.INPUT_KEYBOARD;
                input_up.ki.wVk = sendChar;
                input_up.ki.dwFlags = (int)KEYEVENTF.KEYUP;
                SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      
            }
        }
        private static Key GetKeyByChar(char c)
        {
            string str = "abcdefghijklmnopqrstuvwxyz";
            int index = str.IndexOf(c.ToString().ToLower());
            switch (index)
            {
                case 0:
                    return Key.A;
                case 1:
                    return Key.B;
                case 2:
                    return Key.C;
                case 3:
                    return Key.D;
                case 4:
                    return Key.E;
                case 5:
                    return Key.F;
                case 6:
                    return Key.G;
                case 7:
                    return Key.H;
                case 8:
                    return Key.I;
                case 9:
                    return Key.J;
                case 10:
                    return Key.K;
                case 11:
                    return Key.L;
                case 12:
                    return Key.M;
                case 13:
                    return Key.N;
                case 14:
                    return Key.O;
                case 15:
                    return Key.P;
                case 16:
                    return Key.Q;
                case 17:
                    return Key.R;
                case 18:
                    return Key.S;
                case 19:
                    return Key.T;
                case 20:
                    return Key.U;
                case 21:
                    return Key.V;
                case 22:
                    return Key.W;
                case 23:
                    return Key.X;
                case 24:
                    return Key.Y;
                default:
                    return Key.Z;
            }
        }

        #endregion

        #region WPF

        public static IEnumerable<T> FindVisualChildren<T>([NotNull] this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var queue = new Queue<DependencyObject>(new[] { parent });

            while (queue.Any())
            {
                var reference = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(reference);

                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    if (child is T children)
                        yield return children;

                    queue.Enqueue(child);
                }
            }
        }

        //public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        //{

        //    if (depObj == null) yield return (T)Enumerable.Empty<T>();
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //    {
        //        DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
        //        if (ithChild == null) continue;
        //        if (ithChild is T t) yield return t;
        //        foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
        //    }
        //}

        #endregion
    }
}
