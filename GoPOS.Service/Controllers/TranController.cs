using GoPOS.Models;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Service.API;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoPOS.Controllers;

[Route("client")]
[ApiController]
public class TranController : ControllerBase
{
    private readonly ITranApiService tranApiService;
    private string? RemoteIP => HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    public TranController(ITranApiService tranApiService)
    {
        this.tranApiService = tranApiService;
    }

    /// <summary>
    /// Receive data from SubPOS only
    /// TRN data
    /// 
    /// If error, return errors in details
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    [HttpPost("tran")]
    public async Task<IActionResult> ReceiveTran([FromBody] JsonElement jsonElement)
    {
        try
        {
            var res = tranApiService.SaveTRDataByJson(jsonElement.GetRawText());
            return Ok(ErrorApi.ReturnApiResult(res.ResultCode, res.ResultCode, res.ResultMessage));
        }
        catch (Exception ex)
        {
            return Ok(ErrorApi.ReturnApiResult("9999", ex.Message, ex.ToFormattedString()));
        }
    }

    /// <summary>
    /// Receive data from SubPOS only
    /// TRN data
    /// 
    /// If error, return errors in details
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    [HttpPost("tran/non")]
    public async Task<IActionResult> ReceiveNonTran([FromBody] JsonElement jsonElement)
    {
        try
        {
            var res = tranApiService.SaveNTRDataByJson(jsonElement.GetRawText());
            return Ok(ErrorApi.ReturnApiResult(res.ResultCode, res.ResultCode, res.ResultMessage));
        }
        catch (Exception ex)
        {
            return Ok(ErrorApi.ReturnApiResult("9999", ex.Message, ex.ToFormattedString()));
        }
    }


    /// <summary>
    /// Receive data from SubPOS only
    /// TRN data
    /// 
    /// If error, return errors in details
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    [HttpPost("tran/account")]
    public async Task<IActionResult> ReceiveSettAccount([FromBody] JsonElement jsonElement)
    {
        try
        {
            var res = tranApiService.SaveSettAccountByJson(jsonElement.GetRawText());
            return Ok(ErrorApi.ReturnApiResult(res.ResultCode, res.ResultCode, res.ResultMessage));
        }
        catch (Exception ex)
        {
            return Ok(ErrorApi.ReturnApiResult("9999", ex.Message, ex.ToFormattedString()));
        }
    }
}
