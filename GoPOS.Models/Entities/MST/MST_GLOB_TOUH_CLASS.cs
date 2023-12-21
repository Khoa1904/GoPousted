using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_GLOB_TOUH_CLASS")]
public class MST_GLOB_TOUH_CLASS : IIdentifyEntity
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
	/// 터치분류코드
	/// </summary>
	[Comment("터치분류코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("ctgCode")]
	public string? TU_CLASS_CODE { get; set; }
	/// <summary>
	/// 국가코드
	/// </summary>
	[Comment("국가코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("langCode")]
	public string? NATION_CODE { get; set; }
	/// <summary>
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	[Key, Column(Order = 4)]
	[Required]
	[JsonPropertyName("posNo")]
	public string? POS_NO { get; set; } = "";
	/// <summary>
	/// 터치분류명
	/// </summary>
	[Comment("터치분류명")]
    [JsonPropertyName("ctgNm")]
	public string? TU_CLASS_NAME { get; set; }
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
		return "SHOP_CODE_TU_CLASS_CODE_NATION_CODE_POS_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "client/master/multi/touchkey";
    }    
}