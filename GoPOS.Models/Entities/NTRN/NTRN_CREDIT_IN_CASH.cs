using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_CREDIT_IN_CASH")]
public class NTRN_CREDIT_IN_CASH : IIdentifyEntity
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
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 외상입금번호
	/// </summary>
	[Comment("외상입금번호")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SALE_NO { get; set; }
	/// <summary>
	/// 결제액-현금
	/// </summary>
	[Comment("결제액-현금")]
	[Precision(12, 2)]
	public decimal CASH_AMT { get; set; } = 0;
	/// <summary>
	/// 등록일시 System Date(8) + System Time(6)
	/// </summary>
	[Comment("등록일시 System Date(8) + System Time(6)")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_CST_NO_SALE_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}