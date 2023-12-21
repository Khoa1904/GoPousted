using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_FIX_DC")]
public class MST_INFO_FIX_DC : IIdentifyEntity
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
	/// 할인넘버
	/// </summary>
	[Comment("할인넘버")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("dscNo")]
	public string? DC_NO { get; set; }
	/// <summary>
	/// 할인구분 A:전체 S:선택
	/// </summary>
	[Comment("할인구분 A:전체 S:선택")]
    [JsonPropertyName("dscSe")]
	public string? DC_DIV_FLAG { get; set; }
	/// <summary>
	/// 할인유형 A:금액 P:%
	/// </summary>
	[Comment("할인유형 A:금액 P:%")]
    [JsonPropertyName("dscType")]
	public string? DC_TYPE_FLAG { get; set; }
	/// <summary>
	/// 할인값
	/// </summary>
	[Comment("할인값")]
	[Precision(12, 2)]
    [JsonPropertyName("dscPrice")]
	public decimal DC_VALUE { get; set; }
	/// <summary>
	/// 디스플레이순서
	/// </summary>
	[Comment("디스플레이순서")]
    [JsonPropertyName("arraySeq")]
	public Int16 DISP_SEQ_NO { get; set; }
	/// <summary>
	/// 자동닫기여부
	/// </summary>
	[Comment("자동닫기여부")]
    [JsonPropertyName("autoCloseAt")]
	public string? CLOSE_YN { get; set; }
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
		return "SHOP_CODE_DC_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment/dsc";
    }    
}