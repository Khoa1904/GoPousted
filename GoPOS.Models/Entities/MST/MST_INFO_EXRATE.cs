using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_EXRATE")]
public class MST_INFO_EXRATE : IIdentifyEntity
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
	/// 영업일자
	/// </summary>
	[Comment("영업일자")]
	[JsonPropertyName("saleDt")]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 통화구분 (SCD_CODEM_T:143)
	/// </summary>
	[Comment("통화구분 (SCD_CODEM_T:143)")]
	[Key, Column(Order = 2)]
	[Required]
	[JsonPropertyName("exchangeCode")]
	public string? EX_CODE { get; set; }
	/// <summary>
	/// 환율금액(통화)
	/// </summary>
	[Comment("환율금액(통화)")]
	[Precision(10, 2)]
	[JsonPropertyName("exchangeFor")]
	public decimal EX_FOR { get; set; }
    /// <summary>
    /// 환율금액(원화)
    /// </summary>
    [Comment("환율금액(원화)")]
	[Precision(10, 2)]
	[JsonPropertyName("exchangeKrw")]
	public decimal EX_KRW { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	[JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
    /// <summary>
    /// 수정일시
    /// </summary>
    [Comment("수정일시")]
	[JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_EX_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/exchange";
    }    
}