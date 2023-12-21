using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_KDS_PRD")]
public class MST_INFO_KDS_PRD : IIdentifyEntity
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
	[JsonPropertyName("storeGcode")]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 3)]
	[Required]
	[JsonPropertyName("kitptrNo")]
	public string? KDS_NO { get; set; }
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
		return "SHOP_CODE_PRD_CODE_KDS_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
        return "/client/master/print/kitchen/goods";
    }
}