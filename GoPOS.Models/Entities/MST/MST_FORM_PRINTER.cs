using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_FORM_PRINTER")]
public class MST_FORM_PRINTER : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 출력물종류코드
	/// </summary>
	[Comment("출력물종류코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("posOutputKndCode")]
	public string? PRT_CLASS_CODE { get; set; }
	/// <summary>
	/// 출력물폼
	/// </summary>
	[Comment("출력물폼")]
    [JsonPropertyName("posOutputTmplat")]
	public string? PRT_FORM { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
    [JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
    [JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_PRT_CLASS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "client/master/pos/output";
    }    
}