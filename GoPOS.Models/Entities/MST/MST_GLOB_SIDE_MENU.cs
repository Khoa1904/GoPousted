using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_GLOB_SIDE_MENU")]
public class MST_GLOB_SIDE_MENU : IIdentifyEntity
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
	/// 사이드메뉴선택분류코드
	/// </summary>
	[Comment("사이드메뉴선택분류코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("ctgCode")]
	public string? SDS_CLASS_CODE { get; set; }
	/// <summary>
	/// 국가코드
	/// </summary>
	[Comment("국가코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("langCode")]
	public string? NATION_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴선택분류명
	/// </summary>
	[Comment("사이드메뉴선택분류명")]
    [JsonPropertyName("ctgNm")]
	public string? SDS_CLASS_NAME { get; set; }
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
		return "SHOP_CODE_SDS_CLASS_CODE_NATION_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/multi/sidemenu";
    }    
}