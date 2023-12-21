

using GoShared.Helpers;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace GoPOS.Models;

public class ErrorApi
{
    public static ErrorResult ReturnErrorApi<T>(string resultCode, string resultMessage, T returnPost) where T : class, new()
    {
        HeaderApi header = JsonHelper.JsonToModel<ErrorResult>(JsonHelper.ModelToJson(returnPost)).model.Header;

        return new()
        {
            Header = new()
            {
                RequestId = header.RequestId,
                ClientCode = header.ClientCode,
                ResultCode = resultCode,
                ResultMessage = resultMessage
            },
            Body = new()
            {
                ReturnPost = returnPost
            }
        };
    }

    public static ApiServiceResult ReturnApiResult(string statusCode, string errorCode, string returnMessage)
    {
        return new()
        {
            Status = statusCode,
            Errors = new ApiServiceError()
            {
                Code = errorCode,
                Message = returnMessage
            },
            Results = returnMessage
        };
    }
    public static ApiServiceResult ReturnApiToken(string statusCode, object results)
    {
        return new()
        {
            Status = statusCode,
            Results = results
        };
    }
}

public class ApiServiceResult
{
    [JsonPropertyName("results")]
    public object Results { get; set; }

    [JsonPropertyName("error")]
    public ApiServiceError Errors { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }

}

public class ApiServiceError
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class ErrorResult
{
    public HeaderApi Header { get; set; } = new();
    public ErrorBody Body { get; set; } = new();
}

public class ErrorBody
{
    public object ReturnPost { get; set; } = new();
}
