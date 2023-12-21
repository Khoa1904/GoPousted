using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

public enum EKDS_Type { Kitchen = 'P' , KDS = 'K'}
public enum EKDS_SenbdType { Socket = 'S', Http = 'H', SerialPort = 'C' }
[Table("MST_INFO_KDS")]
public class MST_INFO_KDS : IIdentifyEntity
{
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 1)]
	[Required]
	[JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 2)]
	[Required]
	[JsonPropertyName("kitptrNo")]
	public string? KDS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrNm")]
	public string? KDS_NAME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrSeCode")]
	public string? KDS_TYPE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrTypeCode")]
	public string? KDS_TYPE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("floorSeCode")]
	public string? FLOOR_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("sendSeCode")]
	public string? SEND_TYPE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrUrl")]
	public string? KDS_URL { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrTcpIpVal")]
	public string? KDS_TCP_IP { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrTcpPortVal")]
	public string? KDS_TCP_PORT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    [JsonPropertyName("kitptrSpdVal")]
    public string? KDS_SPEED { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    [JsonPropertyName("kitptrStartDt")]
	public string? PRT_TM_FROM { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("kitptrEndDt")]
	public string? PRT_TM_TO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_KDS_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
        return "/client/master/print/kitchen";
    }
}