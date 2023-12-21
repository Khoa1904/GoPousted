using Caliburn.Micro;
using GoPOS.ViewModels;
using GoPOS.ViewModels.Samples;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;
using GoShared.Helpers;

namespace GoPOS.Helpers;

public static class WindowHelper
{
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

    private static string EncryptDes(string source, string key)
    {
        using TripleDES tripleDesCryptoService = TripleDES.Create();
        using MD5 hashMd5Provider = MD5.Create();
        byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        tripleDesCryptoService.Key = byteHash;
        tripleDesCryptoService.Mode = CipherMode.ECB;
        byte[] data = Encoding.UTF8.GetBytes(source);
        return Convert.ToBase64String(tripleDesCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
    }

    private static string DecryptDes(string encrypt, string key)
    {
        using TripleDES tripleDesCryptoService = TripleDES.Create();
        using MD5 hashMd5Provider = MD5.Create();
        byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        tripleDesCryptoService.Key = byteHash;
        tripleDesCryptoService.Mode = CipherMode.ECB;
        byte[] data = Convert.FromBase64String(encrypt);
        return Encoding.UTF8.GetString(tripleDesCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
    }

    public static string EncryptString(string str)
    {
        return string.IsNullOrWhiteSpace(str) ? "" : EncryptDes(str, " ");
    }

    public static string DecryptString(string str)
    {
        return string.IsNullOrWhiteSpace(str) ? "" : DecryptDes(str, " ");
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
}
