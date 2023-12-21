using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_PRD_COUPON")]
public class MST_INFO_PRD_COUPON : IIdentifyEntity
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
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("storeGcode")]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 쿠폰적용상품SEQ
	/// </summary>
	[Comment("쿠폰적용상품SEQ")]
    [JsonPropertyName("arraySeq")]
	public string? INDEX_NO { get; set; }
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
		return "SHOP_CODE_TK_CPN_CODE_PRD_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment/cpn/goods";
    }    
}