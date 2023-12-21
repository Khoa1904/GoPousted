using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_EASYPAY")]
public class TRN_EASYPAY : IIdentifyEntity
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
	/// 승인처리구분
	/// </summary>
	[Comment("승인처리구분")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 결제카드번호
	/// </summary>
	[Comment("결제카드번호")]
	public string? PAY_CARD_NO { get; set; }
	/// <summary>
	/// 승인요청금액
	/// </summary>
	[Comment("승인요청금액")]
	[Precision(12, 2)]
	public decimal APPR_REQ_AMT { get; set; } = 0;
	/// <summary>
	/// 봉사료
	/// </summary>
	[Comment("봉사료")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; } = 0;
	/// <summary>
	/// 부가세
	/// </summary>
	[Comment("부가세")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; } = 0;
    /// <summary>
    /// 면세액
    /// </summary>
    [Comment("면세액")]
    [Precision(12, 2)]
    public decimal DUTY_AMT { get; set; }
    /// <summary>
    /// 할부개월구분 (CCD_CODEM_T : 059) 0:일시불 1:할부 2:할인-일시불 3:할인-할부 → 어떻게 구분??
    /// </summary>
    [Comment("할부개월구분 (CCD_CODEM_T : 059) 0:일시불 1:할부 2:할인-일시불 3:할인-할부 → 어떻게 구분??")]
	public string? INST_MM_FLAG { get; set; }
	/// <summary>
	/// 할부개월수
	/// </summary>
	[Comment("할부개월수")]
	public Int16 INST_MM_CNT { get; set; } = 0;
	/// <summary>
	/// 결제방법 (Q:QR, B:Barcode, K:Keyin)
	/// </summary>
	[Comment("결제방법 (Q:QR, B:Barcode, K:Keyin)")]
	public string? PAY_METHOD_FLAG { get; set; }
	/// <summary>
	/// 승인일자
	/// </summary>
	[Comment("승인일자")]
	public string? APPR_DATE { get; set; }
	/// <summary>
	/// 승인시간
	/// </summary>
	[Comment("승인시간")]
	public string? APPR_TIME { get; set; }
	/// <summary>
	/// 승인번호
	/// </summary>
	[Comment("승인번호")]
	public string? APPR_NO { get; set; }
	/// <summary>
	/// 밴사코드
	/// </summary>
	[Comment("밴사코드")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 간편결제코드
	/// </summary>
	[Comment("간편결제코드")]
	public string? PAYCP_CODE { get; set; }
	/// <summary>
	/// 밴간편결제코드
	/// </summary>
	[Comment("밴간편결제코드")]
	public string? VAN_PAYCP_CODE { get; set; }
	/// <summary>
	/// 카드발급사코드
	/// </summary>
	[Comment("카드발급사코드")]
	public string? ISS_CRDCP_CODE { get; set; }
	/// <summary>
	/// 카드발급사명
	/// </summary>
	[Comment("카드발급사명")]
	public string? ISS_CRDCP_NAME { get; set; }
	/// <summary>
	/// 카드매입사코드
	/// </summary>
	[Comment("카드매입사코드")]
	public string? PUR_CRDCP_CODE { get; set; }
	/// <summary>
	/// 카드매입사명
	/// </summary>
	[Comment("카드매입사명")]
	public string? PUR_CRDCP_NAME { get; set; }
	/// <summary>
	/// 카드사가맹번호
	/// </summary>
	[Comment("카드사가맹번호")]
	public string? CRDCP_TERM_NO { get; set; }
	/// <summary>
	/// 승인응답메시지내용
	/// </summary>
	[Comment("승인응답메시지내용")]
	public string? APPR_MSG { get; set; }
	/// <summary>
	/// 원거래승인일자
	/// </summary>
	[Comment("원거래승인일자")]
	public string? ORG_APPR_DATE { get; set; }
	/// <summary>
	/// 원거래승인번호
	/// </summary>
	[Comment("원거래승인번호")]
	public string? ORG_APPR_NO { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 판매여부 Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매여부 Y:정상 N:반품(취소)")]
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