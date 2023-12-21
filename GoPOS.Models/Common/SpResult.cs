namespace GoPOS.Models;

public class SpResult
{
    public EResultType ResultType { get; set; } = EResultType.ERROR;
    public string ResultCode { get; set; } = "9000";
    public string ResultMessage { get; set; } = "Default Unknown Message";
    public int ResultCount { get; set; } = 0;
}


public enum EResultType
{
    SUCCESS, // 0000
    EMPTY, // 1000
    ERROR, // 9000
}