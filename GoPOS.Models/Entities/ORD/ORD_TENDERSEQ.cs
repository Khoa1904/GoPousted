using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("ORD_TENDERSEQ")]
public class ORD_TENDERSEQ : IIdentifyEntity
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
	/// 주문결제순서
	/// </summary>
	[Comment("주문결제순서")]
	[Key, Column(Order = 5)]
	[Required]
	public Int16 PAY_SEQ_NO { get; set; }
	/// <summary>
	/// 판매구분 Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매구분 Y:정상 N:반품(취소)")]
	public string? SALE_YN { get; set; }
	/// <summary>
	/// 결제수단구분 (CCD_CODEM_T : 038) - 결제수단코드는 GOPOS에 맞게 정리 되어야 함 01:현금 02:신용카드 03:제휴카드 04:쿠폰 05:상품권 06:외상 07:서비스 08:식권  09:회원포인트
	/// </summary>
	[Comment("결제수단구분 (CCD_CODEM_T : 038) - 결제수단코드는 GOPOS에 맞게 정리 되어야 함 01:현금 02:신용카드 03:제휴카드 04:쿠폰 05:상품권 06:외상 07:서비스 08:식권  09:회원포인트")]
	public string? PAY_TYPE_FLAG { get; set; }
	/// <summary>
	/// 결제금액
	/// </summary>
	[Comment("결제금액")]
	[Precision(12, 2)]
	public decimal PAY_AMT { get; set; }
	/// <summary>
	/// 결제라인번호
	/// </summary>
	[Comment("결제라인번호")]
	public string? LINE_NO { get; set; }
	/// <summary>
	/// 판매원코드
	/// </summary>
	[Comment("판매원코드")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일시
	/// </summary>
	[Comment("수정일시")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_ORDER_NO_PAY_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}