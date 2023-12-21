using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("ORD_PRDT")]
public class ORD_PRDT : IIdentifyEntity
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
	/// <summary>
	/// 주문차수
	/// </summary>
	[Comment("주문차수")]
	public string? ORDER_SEQ_NO { get; set; }
	/// <summary>
	/// 상품주문구분 0:주문 1:주문취소 3:금액변경취소 4:금액변경주문
	/// </summary>
	[Comment("상품주문구분 0:주문 1:주문취소 3:금액변경취소 4:금액변경주문")]
	public string? PRD_FLAG { get; set; } = "0";
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	public string? PRD_CODE { get; set; }
    /// <summary>
    /// 상품명
    /// </summary>
    [Comment("상품명")]
	[NotMapped]
    public string? PRD_NAME { get; set; }
    /// <summary>
    /// 상품유형구분 (CCD_CODEM_T : 060) 0:일반 1:선택메뉴
    /// </summary>
    [Comment("상품유형구분 (CCD_CODEM_T : 060) 0:일반 1:선택메뉴")]
	public string? PRD_TYPE_FLAG { get; set; }
	/// <summary>
	/// 코너코드
	/// </summary>
	[Comment("코너코드")]
	public string? CORNER_CODE { get; set; }
	/// <summary>
	/// 판매수량
	/// </summary>
	[Comment("판매수량")]
	public int SALE_QTY { get; set; }
	/// <summary>
	/// 판매단가
	/// </summary>
	[Comment("판매단가")]
	[Precision(12, 2)]
	public decimal SALE_UPRC { get; set; }
	/// <summary>
	/// 매출액
	/// </summary>
	[Comment("매출액")]
	[Precision(12, 2)]
	public decimal SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 할인액
	/// </summary>
	[Comment("할인액")]
	[Precision(12, 2)]
	public decimal DC_AMT { get; set; } = 0;
	/// <summary>
	/// 기타에누리액(식권짜투리/에누리-끝전)
	/// </summary>
	[Comment("기타에누리액(식권짜투리/에누리-끝전)")]
	[Precision(12, 2)]
	public decimal ETC_AMT { get; set; } = 0;
	/// <summary>
	/// 봉사료액
	/// </summary>
	[Comment("봉사료액")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; } = 0;
	/// <summary>
	/// 부가세액
	/// </summary>
	[Comment("부가세액")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; } = 0;
	/// <summary>
	/// 실매출액
	/// </summary>
	[Comment("실매출액")]
	[Precision(12, 2)]
	public decimal DCM_SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 교환권번호 (외식-푸드코트)
	/// </summary>
	[Comment("교환권번호 (외식-푸드코트)")]
	public string? CHG_BILL_NO { get; set; }
	/// <summary>
	/// 과세여부
	/// N: 면세상품
	/// </summary>
	[Comment("과세여부")]
	public string? TAX_YN { get; set; }
	/// <summary>
	/// 배달포장구분 (CCD_CODEM_T : 030) 0:일반 1:배달 2:포장 전체배달을 선택했을때 상품 전체 배달 구분 처리 함
	/// </summary>
	[Comment("배달포장구분 (CCD_CODEM_T : 030) 0:일반 1:배달 2:포장 전체배달을 선택했을때 상품 전체 배달 구분 처리 함")]
	public string? DLV_PACK_FLAG { get; set; }
	/// <summary>
	/// 사이드메뉴-분류코드
	/// </summary>
	[Comment("사이드메뉴-분류코드")]
	public string? SDS_CLASS_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴부모코드
	/// </summary>
	[Comment("사이드메뉴부모코드")]
	public string? SDS_PARENT_CODE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [NotMapped]
    public string? SIDE_MENU_YN { get; set; }
    /// <summary>
    /// 할인액-일반
    /// </summary>
    [Comment("할인액-일반")]
	[Precision(12, 2)]
	public decimal DC_AMT_GEN { get; set; } = 0;
	/// <summary>
	/// 할인액-서비스 할인액 -서비스 OP 파악해야 함
	/// </summary>
	[Comment("할인액-서비스 할인액 -서비스 OP 파악해야 함")]
	[Precision(12, 2)]
	public decimal DC_AMT_SVC { get; set; } = 0;
	/// <summary>
	/// 할인액-제휴카드
	/// </summary>
	[Comment("할인액-제휴카드")]
	[Precision(12, 2)]
	public decimal DC_AMT_JCD { get; set; } = 0;
	/// <summary>
	/// 할인액-쿠폰
	/// </summary>
	[Comment("할인액-쿠폰")]
	[Precision(12, 2)]
	public decimal DC_AMT_CPN { get; set; } = 0;
	/// <summary>
	/// 할인액-회원
	/// </summary>
	[Comment("할인액-회원")]
	[Precision(12, 2)]
	public decimal DC_AMT_CST { get; set; } = 0;
	/// <summary>
	/// 할인액-식권
	/// </summary>
	[Comment("할인액-식권")]
	[Precision(12, 2)]
	public decimal DC_AMT_FOD { get; set; } = 0;
	/// <summary>
	/// 할인액-프로모션
	/// </summary>
	[Comment("할인액-프로모션")]
	[Precision(12, 2)]
	public decimal DC_AMT_PRM { get; set; } = 0;
	/// <summary>
	/// 할인액-신용카드-현장할인
	/// </summary>
	[Comment("할인액-신용카드-현장할인")]
	[Precision(12, 2)]
	public decimal DC_AMT_CRD { get; set; } = 0;
	/// <summary>
	/// 할인액-포장
	/// </summary>
	[Comment("할인액-포장")]
	[Precision(12, 2)]
	public decimal DC_AMT_PACK { get; set; } = 0;
	/// <summary>
	/// 오더스크린-처리구분  0:주문 1:처리완료
	/// </summary>
	[Comment("오더스크린-처리구분  0:주문 1:처리완료")]
	public string? ORDER_SCN_FLAG { get; set; } = "0";
	/// <summary>
	/// 오더스크린-처리일시
	/// </summary>
	[Comment("오더스크린-처리일시")]
	public string? ORDER_SCN_DT { get; set; }
	/// <summary>
	/// 오더스크린-처리포스번호
	/// </summary>
	[Comment("오더스크린-처리포스번호")]
	public string? ORDER_SCN_POS_NO { get; set; }
	/// <summary>
	/// 주문취소구분 0:정상 1:취소 2:변경
	/// </summary>
	[Comment("주문취소구분 0:정상 1:취소 2:변경")]
	public string? ORDER_CANCEL_FLAG { get; set; } = "0";
    /// <summary>
    /// 쿠폰코드 쿠폰 코드 추가 됨 - 23.06.18
    /// </summary>
    [Comment("쿠폰코드 쿠폰 코드 추가 됨 - 23.06.18")]
    public string? TK_CPN_CODE { get; set; }
	/// <summary>
	/// REMARK
	/// </summary>
	[Comment("Remark")]
    public string? REMARK { get; set; }
    /// <summary>
    /// 등록일시
    /// </summary>
    [Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	

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
}