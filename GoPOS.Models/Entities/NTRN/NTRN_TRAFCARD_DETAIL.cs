using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_TRAFCARD_DETAIL")]
public class NTRN_TRAFCARD_DETAIL : IIdentifyEntity
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
	/// 승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인
	/// </summary>
	[Comment("승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 선불카드번호
	/// </summary>
	[Comment("선불카드번호")]
	public string? PPC_CARD_NO { get; set; }
	/// <summary>
	/// 승인요청금액
	/// </summary>
	[Comment("승인요청금액")]
	[Precision(12, 2)]
	public decimal APPR_REQ_AMT { get; set; } = 0;
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
	/// 카드사코드
	/// </summary>
	[Comment("카드사코드")]
	public string? CRDCP_CODE { get; set; }
	/// <summary>
	/// 신용카드 승인번호
	/// </summary>
	[Comment("신용카드 승인번호")]
	public string? CARD_APPR_NO { get; set; }
	/// <summary>
	/// 신용카드 승인일시
	/// </summary>
	[Comment("신용카드 승인일시")]
	public string? CARD_APPR_DT { get; set; }
	/// <summary>
	/// 승인금액
	/// </summary>
	[Comment("승인금액")]
	[Precision(12, 2)]
	public decimal APPR_AMT { get; set; } = 0;
	/// <summary>
	/// 선불카드 잔액
	/// </summary>
	[Comment("선불카드 잔액")]
	[Precision(12, 2)]
	public decimal PPC_BALANCE { get; set; }
	/// <summary>
	/// 선불카드 유효기간
	/// </summary>
	[Comment("선불카드 유효기간")]
	public string? VALID_TERM { get; set; }
	/// <summary>
	/// 선불카드 터미널번호
	/// </summary>
	[Comment("선불카드 터미널번호")]
	public string? PPC_TERM_NO { get; set; }
	/// <summary>
	/// 선불카드 승인로그번호
	/// </summary>
	[Comment("선불카드 승인로그번호")]
	public string? APPR_LOG_NO { get; set; }
	/// <summary>
	/// 원거래 승인일자
	/// </summary>
	[Comment("원거래 승인일자")]
	public string? ORG_APPR_DATE { get; set; }
	/// <summary>
	/// 원거래 승인시간
	/// </summary>
	[Comment("원거래 승인시간")]
	public string? ORG_APPR_TIME { get; set; }
	/// <summary>
	/// 원거래 승인번호
	/// </summary>
	[Comment("원거래 승인번호")]
	public string? ORG_APPR_NO { get; set; }
	/// <summary>
	/// 원거래 승인로그번호
	/// </summary>
	[Comment("원거래 승인로그번호")]
	public string? ORG_APPR_LOG_NO { get; set; }
	/// <summary>
	/// 승인 일련번호
	/// </summary>
	[Comment("승인 일련번호")]
	public string? POS_UNIQUE_NO { get; set; }
	/// <summary>
	/// 원거래 승인 일련번호
	/// </summary>
	[Comment("원거래 승인 일련번호")]
	public string? ORG_POS_UNIQUE_NO { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 판매원번호
	/// </summary>
	[Comment("판매원번호")]
	public string? EMP_NO { get; set; }

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