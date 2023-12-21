using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_HEADER")]
public class TRN_HEADER : IIdentifyEntity
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
	/// 주문번호 주문 등록 있을 경우 주문 번호(후불 POS 사용)
	/// </summary>
	[Comment("주문번호 주문 등록 있을 경우 주문 번호(후불 POS 사용)")]
	public string? ORDER_NO { get; set; }
	/// <summary>
	/// 총매출액
	/// </summary>
	[Comment("총매출액")]
	[Precision(12, 2)]
	public decimal TOT_SALE_AMT { get; set; }
	/// <summary>
	/// 총할인액
	/// </summary>
	[Comment("총할인액")]
	[Precision(12, 2)]
	public decimal TOT_DC_AMT { get; set; }
	/// <summary>
	/// 봉사료
	/// </summary>
	[Comment("봉사료")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; }
	/// <summary>
	/// 기타에누리액 식권짜투리/에누리-끝전
	/// </summary>
	[Comment("기타에누리액 식권짜투리/에누리-끝전")]
	[Precision(12, 2)]
	public decimal TOT_ETC_AMT { get; set; }
	/// <summary>
	/// 실매출액
	/// </summary>
	[Comment("실매출액")]
	[Precision(12, 2)]
	public decimal DCM_SALE_AMT { get; set; }
	/// <summary>
	/// 과세매출액
	/// </summary>
	[Comment("과세매출액")]
	[Precision(12, 2)]
	public decimal VAT_SALE_AMT { get; set; }
	/// <summary>
	/// 부가세
	/// </summary>
	[Comment("부가세")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; }
	/// <summary>
	/// 면세매출액
	/// </summary>
	[Comment("면세매출액")]
	[Precision(12, 2)]
	public decimal NO_VAT_SALE_AMT { get; set; }
	/// <summary>
	/// 순매출액
	/// </summary>
	[Comment("순매출액")]
	[Precision(12, 2)]
	public decimal NO_TAX_SALE_AMT { get; set; }
	/// <summary>
	/// 받을금액
	/// </summary>
	[Comment("받을금액")]
	[Precision(12, 2)]
	public decimal EXP_PAY_AMT { get; set; }
	/// <summary>
	/// 받은금액
	/// </summary>
	[Comment("받은금액")]
	[Precision(12, 2)]
	public decimal GST_PAY_AMT { get; set; }
	/// <summary>
	/// 거스름돈
	/// </summary>
	[Comment("거스름돈")]
	[Precision(12, 2)]
	public decimal RET_PAY_AMT { get; set; }
	/// <summary>
	/// 현금영수증발행금액
	/// </summary>
	[Comment("현금영수증발행금액")]
	[Precision(12, 2)]
	public decimal CASH_BILL_AMT { get; set; }
	/// <summary>
	/// 할인액-일반
	/// </summary>
	[Comment("할인액-일반")]
	[Precision(12, 2)]
	public decimal DC_GEN_AMT { get; set; }
	/// <summary>
	/// 할인액-서비스
	/// </summary>
	[Comment("할인액-서비스")]
	[Precision(12, 2)]
	public decimal DC_SVC_AMT { get; set; }
	/// <summary>
	/// 할인액-제휴카드
	/// </summary>
	[Comment("할인액-제휴카드")]
	[Precision(12, 2)]
	public decimal DC_PCD_AMT { get; set; }
	/// <summary>
	/// 할인액-쿠폰
	/// </summary>
	[Comment("할인액-쿠폰")]
	[Precision(12, 2)]
	public decimal DC_CPN_AMT { get; set; }
	/// <summary>
	/// 할인액-회원
	/// </summary>
	[Comment("할인액-회원")]
	[Precision(12, 2)]
	public decimal DC_CST_AMT { get; set; }
	/// <summary>
	/// 할인액-식권
	/// </summary>
	[Comment("할인액-식권")]
	[Precision(12, 2)]
	public decimal DC_TFD_AMT { get; set; }
	/// <summary>
	/// 할인액-프로모션
	/// </summary>
	[Comment("할인액-프로모션")]
	[Precision(12, 2)]
	public decimal DC_PRM_AMT { get; set; }
	/// <summary>
	/// 할인액-신용카드현장할인
	/// </summary>
	[Comment("할인액-신용카드현장할인")]
	[Precision(12, 2)]
	public decimal DC_CRD_AMT { get; set; }
	/// <summary>
	/// 할인액-포장할인
	/// </summary>
	[Comment("할인액-포장할인")]
	[Precision(12, 2)]
	public decimal DC_PACK_AMT { get; set; }
	/// <summary>
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 배달주문구분  0:일반/대기 2:배달
	/// </summary>
	[Comment("배달주문구분  0:일반/대기 2:배달")]
	public string? DLV_ORDER_FLAG { get; set; }
	/// <summary>
	/// 객층관리사용구분 0:미사용 1:사용
	/// </summary>
	[Comment("객층관리사용구분 0:미사용 1:사용")]
	public string? FD_GST_FLAG_YN { get; set; }
	/// <summary>
	/// 객층구분
	/// </summary>
	[Comment("객층구분")]
	public string? FD_GST_FLAG_1 { get; set; }
	/// <summary>
	/// 객층구분
	/// </summary>
	[Comment("객층구분")]
	public string? FD_GST_FLAG_2 { get; set; }
	/// <summary>
	/// 반품-원거래영수번호 원거래 매장코드(6) + 원거래 영업일(8) + 원거래POSNO(2) + 원거래영수증번호(4)
	/// </summary>
	[Comment("반품-원거래영수번호 원거래 매장코드(6) + 원거래 영업일(8) + 원거래POSNO(2) + 원거래영수증번호(4)")]
	public string? ORG_BILL_NO { get; set; }
	/// <summary>
	/// 반품-사유코드 반품 사유코드
	/// </summary>
	[Comment("반품-사유코드 반품 사유코드")]
	public string? RTN_MSG_CODE { get; set; }
	/// <summary>
	/// 반품-사유 반품 사유
	/// </summary>
	[Comment("반품-사유 반품 사유")]
	public string? RTN_MSG { get; set; }
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
		return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}