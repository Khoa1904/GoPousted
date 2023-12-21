namespace GoPOS.Models;

public class HeaderApi
{
    public string RequestId { get; set; } = string.Empty;
    public string ClientCode { get; set; } = string.Empty;
    public string ResultCode { get; set; } = string.Empty;
    public string ResultMessage { get; set; } = string.Empty;
}