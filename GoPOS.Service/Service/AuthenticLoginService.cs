using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace GoPOS.Service.Service
{
    public class AuthenticLoginService : IAuthenticLoginService
    {



        public async Task<ResponseModel> Login(string username, string password)
        {

            try
            {

                ApiRequest request = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer);

                var url = DataLocals.AppConfig.PosComm.SvrURLServer + "/client/auth/store";

                var pairs = new
                {
                    userId = username,
                    password = password
                };

                ResponseModel response = new ResponseModel();
                response.results = null;

                var result = await request.GetCall(pairs, url);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await result.Content.ReadAsStringAsync();
                    var message = result.Content.ReadAsStringAsync().Result;


            
                    if (message.Contains("8990001"))
                    {
                        response.status = "8990001";
                        return response;
                    }
                    else if (message.Contains("8250008") || message.Contains("8250006"))
                    {
                        response.status = "8250008";
                        return response;
                    }
                     

                    ResponseModel data =
                        JsonConvert.DeserializeObject<ResponseModel>(message);

                    return data;

                }
            }
            catch (Exception ex)
            {
                
            }
          



            return null;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public bool IsUserLoggedIn { get; }
    }
  
}
