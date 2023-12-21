using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_CASH")]
public class TRN_CASH : IIdentifyEntity
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
	/// 영수번호
	/// </summary>
	[Comment("영수번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? BILL_NO { get; set; }
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
	/// 결제액-현금
	/// </summary>
	[Comment("결제액-현금")]
	[Precision(12, 2)]
	public decimal CASH_AMT { get; set; }
	/// <summary>
	/// 거스름
	/// </summary>
	[Comment("거스름")]
	[Precision(12, 2)]
	public decimal RET_AMT { get; set; }
	/// <summary>
	/// 통화코드
	/// </summary>
	[Comment("통화코드")]
	public string? EX_CODE { get; set; }
	/// <summary>
	/// 환율
	/// </summary>
	[Comment("환율")]
	[Precision(12, 2)]
	public decimal EX_KRW { get; set; }
	/// <summary>
	/// 통화-받을금액
	/// </summary>
	[Comment("통화-받을금액")]
	[Precision(12, 2)]
	public decimal EX_EXP_AMT { get; set; }
	/// <summary>
	/// 통화-지불액
	/// </summary>
	[Comment("통화-지불액")]
	[Precision(12, 2)]
	public decimal EX_IN_AMT { get; set; }
	/// <summary>
	/// 통화-거스름
	/// </summary>
	[Comment("통화-거스름")]
	[Precision(12, 2)]
	public decimal EX_RET_AMT { get; set; }
	/// <summary>
	/// 원화-거스름
	/// </summary>
	[Comment("원화-거스름")]
	[Precision(12, 2)]
	public decimal KR_RET_AMT { get; set; }
	/// <summary>
	/// 통화-결제액 통화지불액 - 통화거스름
	/// </summary>
	[Comment("통화-결제액 통화지불액 - 통화거스름")]
	[Precision(12, 2)]
	public decimal EX_PAY_AMT { get; set; }
	/// <summary>
	/// 원화-결제액 통화결제액 * 환율
	/// </summary>
	[Comment("원화-결제액 통화결제액 * 환율")]
	[Precision(12, 2)]
	public decimal KR_PAY_AMT { get; set; }
	/// <summary>
	/// 원화-에누리액 원화결제액 - 현금결제액 - 원화거스름
	/// </summary>
	[Comment("원화-에누리액 원화결제액 - 현금결제액 - 원화거스름")]
	[Precision(12, 2)]
	public decimal KR_ETC_AMT { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 판매여부  Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매여부  Y:정상 N:반품(취소)")]
	public string? SALE_YN { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}