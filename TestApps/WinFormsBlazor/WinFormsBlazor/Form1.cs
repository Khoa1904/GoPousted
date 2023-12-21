using log4net;
using log4net.Config;

using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

using WinFormsBlazor.Pages;
using WinFormsBlazor.Service;

using Application = System.Windows.Forms.Application;

namespace WinFormsBlazor
{
    public partial class Form1 : Form
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(Form1));                                                           //	Log Level(ALL, DEBUG, INFO, WARN, ERROR, FATAL)

        //  wwwroot 경로 문제
        //  exe 파일 있는 디렉토리의 WinFormsBlazor.staticwebassets.runtime.json 파일의 ContentRoots 경로 수정 또는 삭제
        //  dapper sqlite 사용법 : https://endev.tistory.com/9
        //  보안 토큰요청	GET	https://gopos.outlier.kr/SHOPAUTH/TOKEN	통신용 보안 토큰 요청

        //	https://learn.microsoft.com/ko-kr/aspnet/core/blazor/call-web-api?view=aspnetcore-6.0&pivots=server

        #region ini 입력 메소드
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder returnValue, int size, string filePath);
        #endregion

        public string IniPath = Application.StartupPath + @"\gopos.ini";
        public string InstallPath = @"c:\GoPosKiosk";
        public string ApiURL = "https://gopos.outlier.kr/";

        public Form1()
        {
            InitializeComponent();
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            //  ini 저장
            WritePrivateProfileString("PosInfo", "StoreNo", "V41712", IniPath);
            WritePrivateProfileString("PosInfo", "StoreName", "히얌", IniPath);
            WritePrivateProfileString("PosInfo", "PosNo", "01", IniPath);
            WritePrivateProfileString("PosInfo", "PosName", "메인포스", IniPath);

            WritePrivateProfileString("PosInfo", "SaleDate", "20230301", IniPath);                                                          //	엽업일
            WritePrivateProfileString("PosInfo", "BillNo", "0001", IniPath);                                                                //	영수증 번호
            WritePrivateProfileString("PosInfo", "OrderNo", "0001", IniPath);                                                               //	주문 번호
            WritePrivateProfileString("PosInfo", "SaleNo", "0001", IniPath);                                                                //	비매출 영수증 번호
            WritePrivateProfileString("PosInfo", "CSaleNo", "0001", IniPath);                                                               //	외상입금 영수증 번호
            WritePrivateProfileString("PosInfo", "AccoutNo", "01", IniPath);                                                                //	정산차수

            WritePrivateProfileString("PosInfo", "EodFlag", "1", IniPath);
            WritePrivateProfileString("PosInfo", "Version", "202303010001", IniPath);

            WritePrivateProfileString("MainPosInfo", "MainPosIp", "172.0.0.3", IniPath);
            WritePrivateProfileString("MainPosInfo", "MainPosPort", "663", IniPath);
            WritePrivateProfileString("PosComm", "SvrURL", "", IniPath);
            WritePrivateProfileString("PosComm", "MainPOSIP", "127.0.0.1", IniPath);
            WritePrivateProfileString("PosComm", "MainPosPort", "663", IniPath);
            WritePrivateProfileString("PosFTP", "FtpSvrIP", "", IniPath);
            WritePrivateProfileString("PosFTP", "FtpSvrPort", "", IniPath);
            WritePrivateProfileString("PosFTP", "User", "", IniPath);
            WritePrivateProfileString("PosFTP", "Pass", "", IniPath);

            Thread.Sleep(1000);                                                                                                             //  Write 이 후 바로 못 읽기에 시간을 줍니다..

            GetToken();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWindowsFormsBlazorWebView();
            serviceCollection.AddSingleton<WeatherForecastService>();
            serviceCollection.AddSingleton<BlogService>();
            serviceCollection.AddSingleton<CartService>();
            serviceCollection.AddSingleton<OptionService>();

#if DEBUG
            //	https://medium.com/@devmawin/software-development-and-hybrid-app-development-with-blazorwebview-blazor-59297f399811
            //	https://github.com/dotnet/maui/commit/cfc3fab4b07db3c5aeabf20819efc7b140144215
            serviceCollection.AddBlazorWebViewDeveloperTools();
#endif
            //UpdateProgram();

            blazorWebView.HostPage = @"wwwroot\index.html";
            blazorWebView.Services = serviceCollection.BuildServiceProvider();
            blazorWebView.RootComponents.Add<App>("#app");
        }

        private void GetToken()
        {
            //  https://stackoverflow.com/questions/70185058/how-to-replace-obsolete-webclient-with-httpclient-in-net-6
            //  https://s-engineer.tistory.com/337

            //  ini 읽기
            StringBuilder StoreNo = new();
            StringBuilder PosNo = new();

            GetPrivateProfileString("PosInfo", "StoreNo", "", StoreNo, StoreNo.Capacity, IniPath);
            GetPrivateProfileString("PosInfo", "PosNo", "", PosNo, PosNo.Capacity, IniPath);

            //MessageBox.Show("INI StoreNo : " + StoreNo.ToString());

            //  API 통신
            /*
            string ApiURL = "https://smspos.heeyam.com/dev/posAPI/data.json";
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiURL);
            var response = httpClient.Send(request);
            using var reader = new StreamReader(response.Content.ReadAsStream());
            var jsonData = reader.ReadToEnd();
            //MessageBox.Show(jsonData);

            JObject jObject = JObject.Parse(jsonData);
            JToken? jToken = jObject["SalesRecord"];
            if (jToken is not null)
            {
                foreach (JToken data in jToken)
                {
                    //MessageBox.Show("■" + data["date"] + ": " + data["item"]);
                }
            }
            */
            try
            {
                string TokenApiURL = "https://gopos.outlier.kr/client/auth/token?serialNo=B07W1HK38WRWQRAM5WSO&licenseKey=uVgCwEZGIr3TVHl4oec8";
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, TokenApiURL);
                var response = httpClient.Send(request);
                var reader = new StreamReader(response.Content.ReadAsStream());
                var jsonData = reader.ReadToEnd();
                //MessageBox.Show(jsonData);

                JObject jObject = JObject.Parse(jsonData);
                JToken? jToken = jObject["results"]?["accessTkn"];
                if (jToken is not null)
                {
                    var TokenResult = jToken.ToString();
                    MessageBox.Show("토큰 처리결과 : " + TokenResult);
                }
            }
            catch (Exception e)
            {
                logger.Error("GetToken : " + e.Message);
            }

            //  tcp/ip 통신 구현 남음
            //  https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/sockets/socket-services
            //  https://stickode.tistory.com/143
        }

        private async void UpdateProgram()
        {

            string localDownPath = InstallPath + @"\updateTemp";
            string downFile = localDownPath + @"\kiosk.zip";
            string localBackupPath = InstallPath + @"\Backup\" + DateTime.Now.ToString("yyyyMMdd");

            string remoteUri = "https://smspos.heeyam.com/dev/kiosk.zip";
            try
            {
                var di1 = new DirectoryInfo(localDownPath);
                var di2 = new DirectoryInfo(localBackupPath);
                if (di1.Exists == false)
                {
                    try
                    {
                        di1.Create();
                    }
                    catch (Exception e)
                    {
                        logger.Error($"di1.Create : {e.Message}");
                    }
                }

                if (di2.Exists == false)
                {
                    try
                    {
                        di2.Create();
                    }
                    catch (Exception e)
                    {
                        logger.Error($"di2.Create : {e.Message}");
                    }
                }

                using HttpClient httpClient = new();
                var result = await httpClient.GetAsync(remoteUri);
                result.EnsureSuccessStatusCode();

                using var stream = await result.Content.ReadAsStreamAsync();
                using var fs = new FileStream(downFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                await stream.CopyToAsync(fs);
                fs.Close();

                var copyResult = CopyDirectory(InstallPath, localBackupPath);
                if (copyResult)
                {
                    var zipResult = UnzipFile(downFile, InstallPath);
                    if (zipResult)
                    {
                        if (File.Exists(downFile))
                        {
                            try
                            {
                                File.Delete(downFile);
                            }
                            catch (Exception e)
                            {
                                logger.Error($"The deletion failed : {e.Message}");
                            }
                        }
                        else
                        {
                            logger.Debug($"UnzipFile : No File");
                        }
                    }
                }
            }
            catch (HttpRequestException hre)
            {
                logger.Debug($"{nameof(HttpRequestException)} - {hre.Message}");
            }
            catch (Exception e)
            {
                logger.Error($"InstallWebview2RuntimeAsync Error : {e.Message}{Environment.NewLine} Data : {e.Data}{Environment.NewLine} StackTrace : {e.StackTrace}");
            }
        }

        public static bool CopyDirectory(string sourceFolder, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                string[] files = Directory.GetFiles(sourceFolder);
                string[] folders = Directory.GetDirectories(sourceFolder);

                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Path.Combine(destFolder, name);
                    File.Copy(file, dest);
                }

                foreach (string folder in folders)
                {
                    if (folder == sourceFolder + @"\updateTemp" || folder == sourceFolder + @"\Backup")
                    {

                    }
                    else
                    {
                        string name = Path.GetFileName(folder);
                        string dest = Path.Combine(destFolder, name);
                        CopyDirectory(folder, dest);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error($"CopyDirectory : {e.Message}");
                return false;
            }
        }

        public static bool UnzipFile(string zipPath, string unzipPath)
        {
            try
            {
                //	https://learn.microsoft.com/ko-kr/dotnet/api/system.io.compression.zipfile.extracttodirectory?view=net-6.0
                //await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, unzipPath));

                ZipFile.ExtractToDirectory(zipPath, unzipPath, true);
                Thread.Sleep(1000);

                return true;
            }
            catch (Exception e)
            {
                logger.Error($"UnzipFile : {e.Message}");
                return false;
            }
        }
    }
}