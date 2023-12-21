using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_PRD_JOINCARD")]
public class MST_INFO_PRD_JOINCARD : IIdentifyEntity
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
	/// 제휴카드코드
	/// </summary>
	[Comment("제휴카드코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("partnerCode")]
	public string? JCD_CODE { get; set; }
	/// <summary>
	/// 스타일/상품코드
	/// </summary>
	[Comment("스타일/상품코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("storeGcode")]
	public string? STYLE_PRD_CODE { get; set; }

	/// <summary>
	/// tempo data for binding in UI
	/// </summary>
	[NotMapped]
    public string? STYLE_PRD_NAME { get; set; }
	/// <summary>
	/// tempo data for binding in UI
	/// AMOUNT OF THIS PRODUCT IN ORDERPAY UI
	/// AFTER DISCOUNT APPLIED
	/// </summary>
	[NotMapped]
	public decimal? DC_ORDER_AMT { get; set; } = 0;
    /// <summary>
    /// 할인율
    /// </summary>
    [Comment("할인율")]
    [JsonPropertyName("dscRt")]
	public Int16 DC_RATE { get; set; }
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
		return "SHOP_CODE_JCD_CODE_STYLE_PRD_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment/partner-card/goods";
    }    
}