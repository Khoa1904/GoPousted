using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_CNFG_POS")]
public class MST_CNFG_POS : IIdentifyEntity
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
    [JsonPropertyName("posNo")]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("envCode")]
	public string? ENV_SET_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
    [JsonPropertyName("envPreferCode")]
	public string? ENV_SET_VALUE { get; set; }
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
    [JsonPropertyName("updateAt")]
	public string? MODIFY_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
    [JsonPropertyName("insertdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
    [JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SEND_FLAG { get; set; } = "0";
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SEND_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_POS_NO_ENV_SET_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/env/prefer/pos";

    }    
}