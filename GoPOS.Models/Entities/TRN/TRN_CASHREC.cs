using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_CASHREC")]
public class TRN_CASHREC : IIdentifyEntity
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
	/// 승인금액
	/// </summary>
	[Comment("승인금액")]
	[Precision(12, 2)]
	public decimal APPR_AMT { get; set; }
	/// <summary>
	/// 봉사료
	/// </summary>
	[Comment("봉사료")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; }
	/// <summary>
	/// 부가세
	/// </summary>
	[Comment("부가세")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; }
    /// <summary>
    /// 면세액
    /// </summary>
    [Comment("면세액")]
    [Precision(12, 2)]
    public decimal DUTY_AMT { get; set; }
    /// <summary>
    /// 승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인
    /// </summary>
    [Comment("승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 승인거래자유형 (CCD_CODEM_T : 034) 0:비승인 1:개인 2:사업자 5:자진발급
	/// </summary>
	[Comment("승인거래자유형 (CCD_CODEM_T : 034) 0:비승인 1:개인 2:사업자 5:자진발급")]
	public string? APPR_IDT_TYPE { get; set; }
	/// <summary>
	/// 승인인식구분 ( CCD_CODEM_T : 033) 0:비승인 1:주민번호 2:휴대폰번호 3:신용카드번호 4:사업자번호 5:자진발급
	/// </summary>
	[Comment("승인인식구분 ( CCD_CODEM_T : 033) 0:비승인 1:주민번호 2:휴대폰번호 3:신용카드번호 4:사업자번호 5:자진발급")]
	public string? APPR_IDT_FLAG { get; set; }
	/// <summary>
	/// 카드입력구분 (CCD_CODEM_T : 032) S:Swapping K:KeyIn
	/// </summary>
	[Comment("카드입력구분 (CCD_CODEM_T : 032) S:Swapping K:KeyIn")]
	public string? CARD_IN_FLAG { get; set; }
	/// <summary>
	/// 승인구분 0:승인 1:취소
	/// </summary>
	[Comment("승인구분 0:승인 1:취소")]
	public string? APPR_FLAG { get; set; }
	/// <summary>
	/// 승인인식번호
	/// </summary>
	[Comment("승인인식번호")]
	public string? APPR_IDT_NO { get; set; }
	/// <summary>
	/// 승인일자
	/// </summary>
	[Comment("승인일자")]
	public string? APPR_DATE { get; set; }
	/// <summary>
	/// 승인시각
	/// </summary>
	[Comment("승인시각")]
	public string? APPR_TIME { get; set; }
	/// <summary>
	/// 승인번호
	/// </summary>
	[Comment("승인번호")]
	public string? APPR_NO { get; set; }
	/// <summary>
	/// 승인응답메세지
	/// </summary>
	[Comment("승인응답메세지")]
	public string? APPR_MSG { get; set; }
	/// <summary>
	/// 밴사코드
	/// </summary>
	[Comment("밴사코드")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 밴사터미널번호
	/// </summary>
	[Comment("밴사터미널번호")]
	public string? VAN_TERM_NO { get; set; }
	/// <summary>
	/// 밴사전표번호
	/// </summary>
	[Comment("밴사전표번호")]
	public string? VAN_SLIP_NO { get; set; }
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
	/// 승인로그번호
	/// </summary>
	[Comment("승인로그번호")]
	public string? APPR_LOG_NO { get; set; }
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