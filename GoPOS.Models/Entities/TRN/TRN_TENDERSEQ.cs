using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;


namespace GoPOS.Models;

[Table("TRN_TENDERSEQ")]
public class TRN_TENDERSEQ : IIdentifyEntity
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
	/// 주문결제순서
	/// </summary>
	[Comment("주문결제순서")]
	[Key, Column(Order = 5)]
	[Required]
	public Int16 PAY_SEQ_NO { get; set; }
	/// <summary>
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 판매구분  Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매구분  Y:정상 N:반품(취소)")]
	public string? SALE_YN { get; set; }
	/// <summary>
	/// 결제수단구분 (CCD_CODEM_T : 038) - 01:현금 02:신용카드 03:제휴카드 04:쿠폰 05:상품권 06:외상 07:서비스 08:식권  09:회원포인트
	/// </summary>
	[Comment("결제수단구분 (CCD_CODEM_T : 038) - 01:현금, 02:현금영수증, 03:신용카드, " +
		"04:은련카드, 05:간편결제,06:제휴할인카드, 07:상품권, 08:식권, 09:외상, 10:선불카드," +
		"11:선결제, 12:전자상품권, 13:모바일상품권, 14:회원포인트적립, 15:회원포인트사용,16:사원카드")]
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
    public string? PAY_TYPE_NAME
    {  //01:현금 02:신용카드 03:제휴카드 04:쿠폰 05:상품권 06:외상 07:서비스 08:식권  09:회원포인트
		get
		{
			return OrderPayConsts.PayTypeFlagNameByCode(PAY_TYPE_FLAG);
		}
	}

    public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO_PAY_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}