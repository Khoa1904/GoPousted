using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_COUPON")]
public class MST_INFO_COUPON : IIdentifyEntity
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
	/// 쿠폰코드
	/// </summary>
	[Comment("쿠폰코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("cpnCode")]
	public string? TK_CPN_CODE { get; set; }
	/// <summary>
	/// 쿠폰명
	/// </summary>
	[Comment("쿠폰명")]
    [JsonPropertyName("cpnNm")]
	public string? TK_CPN_NAME { get; set; }
	/// <summary>
	/// 티켓분류코드
	/// </summary>
	[Comment("티켓분류코드")]
    [JsonPropertyName("ctgCode")]
	public string? TK_CLASS_CODE { get; set; }
	/// <summary>
	/// 쿠폰할인구분 (CCD_CODEM_T : 018)
	/// </summary>
	[Comment("쿠폰할인구분 (CCD_CODEM_T : 018)")]
    [JsonPropertyName("dscSeCode")]
	public string? CPN_DC_FLAG { get; set; }
	/// <summary>
	/// 쿠폰할인율
	/// </summary>
	[Comment("쿠폰할인율")]
    [JsonPropertyName("dscRt")]
	public Int16 CPN_DC_RATE { get; set; }
	/// <summary>
	/// 쿠폰할인액
	/// </summary>
	[Comment("쿠폰할인액")]
    [JsonPropertyName("dscPrice")]
	public int CPN_DC_AMT { get; set; }
	/// <summary>
	/// 쿠폰적용수량구분 0:전체 1:낱개
	/// </summary>
	[Comment("쿠폰적용수량구분 0:전체 1:낱개")]
    [JsonPropertyName("targetCntSeCode")]
	public string? CPN_QTY_FLAG { get; set; }
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
		return "SHOP_CODE_TK_CPN_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment/cpn";
    }    
}