using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("ORD_POINTSAVE")]
public class ORD_POINTSAVE : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 영업일자
	/// </summary>
	[Comment("영업일자")]
	[Key, Column(Order = 2)]
	[Required]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	[Key, Column(Order = 3)]
	[Required]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 주문번호
	/// </summary>
	[Comment("주문번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? ORDER_NO { get; set; }
	/// <summary>
	/// 순번
	/// </summary>
	[Comment("순번")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SEQ_NO { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_ORDER_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}
    public string CST_NO { get; set; } = string.Empty;
    public string CARD_NO { get; set; } = string.Empty;
    public string LEVEL { get; set; } = string.Empty;
    public decimal TOT_SALE_AMT { get; set; } = 0;
    public decimal TOT_DC_AMT { get; set; } = 0;
    public decimal SAVE_AMT { get; set; } = 0;
    public decimal NO_SAVE_AMT { get; set; } = 0;
    public decimal TOT_PNT { get; set; } = 0;
    public decimal TOT_USE_PNT { get; set; } = 0;
    public decimal LAST_PNT { get; set; } = 0;
    public decimal PRE_PNT { get; set; } = 0;
    public decimal SAVE_PNT { get; set; } = 0;
    public string INSERT_DT { get; set; } = string.Empty;
}