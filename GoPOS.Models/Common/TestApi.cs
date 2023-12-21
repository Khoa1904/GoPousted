using System.Text.Json.Serialization;

namespace GoPOS.Models;

public class TestApi
{
    public HeaderApi Header { get; set; } = new();
    public Test Body { get; set; } = new();
}

public class Test
{
    public string UserID { get; set; } = string.Empty;
    public string UserPW { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("Phone")]
    public string PhoneNumber { get; set; } = string.Empty;

    public TestSub TestSub { get; set; } = new TestSub();

    [JsonIgnore]
    public string Remark { get; set; } = string.Empty;
}

public class TestSub
{
    public string SubID { get; set; } = string.Empty;
}