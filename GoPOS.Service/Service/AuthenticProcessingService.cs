using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using GoPOS.Service.Service.MST;
using GoShared.Helpers;
using Newtonsoft.Json;

namespace GoPOS.Service.Service
{
    public class AuthenticProcessingService : IAuthenticProcessingService
    {
        private readonly IWebInquiryService inquiryAPIService;



        public async Task<AuhthenResponseModelResult> AuthenticateValidateLicense(ListStoreModel model, AuthenRequestHeader header)
        {
            try
            {
                ApiRequest request = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer);
                var url = DataLocals.AppConfig.PosComm.SvrURLServer + "/client/auth/license";

                var pairs = new
                {
                    fchqCode = model.FchqCode,
                    storeCode = model.StoreCode,
                    posNo = model.PosNo,
                    macAdres = GetMacAddress(),
                    posIp = (model.MainPosId == "localhost" || model.MainPosId=="") ? "127.0.0.1" : model.MainPosId ,
                    // Comment tempo
                    posProgVerNo = DataLocals.AppConfig.PosInfo.Version
                };
                var result = await request.GetCall(pairs, url, true, header);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await result.Content.ReadAsStringAsync();                    
                    LogHelper.Logger.Trace(responseContent);
                    AuhthenResponseModelResult data = JsonConvert.DeserializeObject<AuhthenResponseModelResult>(result.Content.ReadAsStringAsync().Result);
                    return data;
                }
                else
                {
                    LogHelper.Logger.Trace(result.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                return null;
            }
        }

        public async Task<bool> SaveDataPosKeyMang()
        {
            try
            {
                ApiRequest request = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer);
                var result = inquiryAPIService.InqAccessToken().Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string GetMacAddress()
        {
            try
            {
                String firstMacAddress = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(nic =>
                        nic.OperationalStatus == OperationalStatus.Up &&
                        nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
                return firstMacAddress;
            }
            catch (Exception ex)
            {
                return null;
                Debug.WriteLine("Mac Address Exception" + ex.Message);
            }
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
    }
}
