using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using GoPOS.Service.Service.API;
using GoPOS.Service.Service.MST;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoPOS.Controllers;

[Route("client/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    public AuthController()
    {

    }

    /// <summary>
    /// Receive data from SubPOS only
    /// TRN data
    /// 
    /// If error, return errors in details
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    [HttpGet("token")]
    public async Task<IActionResult> Token(string licenseId, string licenseKey)
    {
        try
        {
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    licenseId, licenseKey);
            var result = await _apiRequest.LoginAsync(false);
            if (result.IsLogined)
                return Ok(ErrorApi.ReturnApiToken("200", new { accessTkn = result.LoginResponse.AccessToken, expiryDt = result.LoginResponse.ExpiryDt }));
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return Ok(ErrorApi.ReturnApiResult("9999", ex.Message, ex.ToFormattedString()));
        }
    }

}
