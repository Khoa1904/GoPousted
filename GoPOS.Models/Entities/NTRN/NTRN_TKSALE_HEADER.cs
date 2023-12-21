using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_TKSALE_HEADER")]
public class NTRN_TKSALE_HEADER : IIdentifyEntity
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
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 판매여부  Y:판매 N:반품
	/// </summary>
	[Comment("판매여부  Y:판매 N:반품")]
	public string? SALE_YN { get; set; }
	/// <summary>
	/// 티켓분류구분 1:상품권 3:식권
	/// </summary>
	[Comment("티켓분류구분 1:상품권 3:식권")]
	public string? TK_CLASS_FLAG { get; set; }
	/// <summary>
	/// 총판매금액
	/// </summary>
	[Comment("총판매금액")]
	[Precision(12, 2)]
	public decimal TOT_SALE_AMT { get; set; } = 0;
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
	/// 받을금액
	/// </summary>
	[Comment("받을금액")]
	[Precision(12, 2)]
	public decimal EXP_PAY_AMT { get; set; } = 0;
	/// <summary>
	/// 받은금액
	/// </summary>
	[Comment("받은금액")]
	[Precision(12, 2)]
	public decimal GST_PAY_AMT { get; set; } = 0;
	/// <summary>
	/// 거스름돈
	/// </summary>
	[Comment("거스름돈")]
	[Precision(12, 2)]
	public decimal RET_PAY_AMT { get; set; } = 0;
	/// <summary>
	/// 현금
	/// </summary>
	[Comment("현금")]
	[Precision(12, 2)]
	public decimal CASH_AMT { get; set; } = 0;
	/// <summary>
	/// 신용카드
	/// </summary>
	[Comment("신용카드")]
	[Precision(12, 2)]
	public decimal CRD_CARD_AMT { get; set; } = 0;
	/// <summary>
	/// 원천판매번호 ( 영업일자+판매번호 )
	/// </summary>
	[Comment("원천판매번호 ( 영업일자+판매번호 )")]
	public string? ORG_SALE_NO { get; set; }
	/// <summary>
	/// 판매원코드
	/// </summary>
	[Comment("판매원코드")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 서버전송구분 0:미전송 1:전송 2:오류
	/// </summary>
	[Comment("서버전송구분 0:미전송 1:전송 2:오류")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 서버전송일시
	/// </summary>
	[Comment("서버전송일시")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// 등록일시 System Date(8) + System Time(6)
	/// </summary>
	[Comment("등록일시 System Date(8) + System Time(6)")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_SALE_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}