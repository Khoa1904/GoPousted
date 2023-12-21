using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_SIDE_SEL_GROUP")]
public class MST_SIDE_SEL_GROUP : IIdentifyEntity
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
	/// 사이드메뉴-선택그룹코드
	/// </summary>
	[Comment("사이드메뉴-선택그룹코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("grpCode")]
	public string? SDS_GROUP_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴-선택그룹명
	/// </summary>
	[Comment("사이드메뉴-선택그룹명")]
    [JsonPropertyName("grpNm")]
	public string? SDS_GROUP_NAME { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
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
		return "SHOP_CODE_SDS_GROUP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/sidemenu/group";
    }    
}