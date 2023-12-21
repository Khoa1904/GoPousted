using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_TKSALE_PRDT")]
public class NTRN_TKSALE_PRDT : IIdentifyEntity
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
	/// 판매번호
	/// </summary>
	[Comment("판매번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? SALE_NO { get; set; }
	/// <summary>
	/// 순번
	/// </summary>
	[Comment("순번")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SEQ_NO { get; set; }
	/// <summary>
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 티켓분류구분 1:상품권 3:식권
	/// </summary>
	[Comment("티켓분류구분 1:상품권 3:식권")]
	public string? TK_CLASS_FLAG { get; set; }
	/// <summary>
	/// 티켓일련번호
	/// </summary>
	[Comment("티켓일련번호")]
	public string? TK_GFT_SEQ { get; set; }
	/// <summary>
	/// 티켓코드
	/// </summary>
	[Comment("티켓코드")]
	public string? TK_GFT_CODE { get; set; }
	/// <summary>
	/// 티켓액면가
	/// </summary>
	[Comment("티켓액면가")]
	[Precision(12, 2)]
	public decimal TK_GFT_UPRC { get; set; }
	/// <summary>
	/// 판매수량
	/// </summary>
	[Comment("판매수량")]
	public int SALE_QTY { get; set; }
	/// <summary>
	/// 판매금액
	/// </summary>
	[Comment("판매금액")]
	[Precision(12, 2)]
	public decimal SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 실매출금액
	/// </summary>
	[Comment("실매출금액")]
	[Precision(12, 2)]
	public decimal DCM_SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 할인금액
	/// </summary>
	[Comment("할인금액")]
	[Precision(12, 2)]
	public decimal DC_AMT { get; set; } = 0;
	/// <summary>
	/// 등록일시 System Date(8) + System Time(6)
	/// </summary>
	[Comment("등록일시 System Date(8) + System Time(6)")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_SALE_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}