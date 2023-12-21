using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_SIDE_SEL_CODE")]
public class MST_SIDE_SEL_CODE : IIdentifyEntity
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
	/// 사이드메뉴-선택코드
	/// </summary>
	[Comment("사이드메뉴-선택코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("sideCode")]
	public string? SDS_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴-선택명
	/// </summary>
	[Comment("사이드메뉴-선택명")]
    [JsonPropertyName("goodsNm")]
	public string? SDS_NAME { get; set; }
	/// <summary>
	/// 사이드메뉴-선택분류코드
	/// </summary>
	[Comment("사이드메뉴-선택분류코드")]
    [JsonPropertyName("ctgCode")]
	public string? SDS_CLASS_CODE { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
    [JsonPropertyName("storeGcode")]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 선택메뉴-단가
	/// </summary>
	[Comment("선택메뉴-단가")]
	[Precision(10, 2)]
    [JsonPropertyName("addCost")]
	public decimal SDS_PRD_UPRC { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 선택메뉴SEQ
	/// </summary>
	[Comment("선택메뉴SEQ")]
    [JsonPropertyName("goodsSeq")]
	public Int16 SDS_ORDER_SEQ { get; set; }
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
		return "SHOP_CODE_SDS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/sidemenu/item";
    }    
}