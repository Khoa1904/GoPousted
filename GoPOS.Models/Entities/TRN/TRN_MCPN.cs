using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_MCPN")]
public class TRN_MCPN : IIdentifyEntity
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
	/// 모바일쿠폰-바코드
	/// </summary>
	[Comment("모바일쿠폰-바코드")]
	public string? MCP_BAR_CODE { get; set; }
	/// <summary>
	/// 모바일쿠폰-바코드 입력구분
	/// </summary>
	[Comment("모바일쿠폰-바코드 입력구분")]
	public string? MCP_INSERT_FLAG { get; set; }
	/// <summary>
	/// 모바일쿠폰-터미널번호
	/// </summary>
	[Comment("모바일쿠폰-터미널번호")]
	public string? MCP_TERM_NO { get; set; }
	/// <summary>
	/// 모바일쿠폰-가맹번호
	/// </summary>
	[Comment("모바일쿠폰-가맹번호")]
	public string? MCP_JOIN_NO { get; set; }
	/// <summary>
	/// 승인처리구분 (CCD_CODEM_T:031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인
	/// </summary>
	[Comment("승인처리구분 (CCD_CODEM_T:031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 승인요청금액
	/// </summary>
	[Comment("승인요청금액")]
	[Precision(12, 2)]
	public decimal APPR_REQ_AMT { get; set; }
	/// <summary>
	/// 승인금액
	/// </summary>
	[Comment("승인금액")]
	[Precision(12, 2)]
	public decimal APPR_AMT { get; set; }
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
	/// 응답메세지
	/// </summary>
	[Comment("응답메세지")]
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
	/// 모바일쿠폰 액면가 상품쿠폰-승인금액과 동일
	/// </summary>
	[Comment("모바일쿠폰 액면가 상품쿠폰-승인금액과 동일")]
	[Precision(12, 2)]
	public decimal MCP_UAMT { get; set; } = 0;
	/// <summary>
	/// 결제후잔액 정액제쿠폰인경우 발생함
	/// </summary>
	[Comment("결제후잔액 정액제쿠폰인경우 발생함")]
	[Precision(12, 2)]
	public decimal REPAY_MCP_AMT { get; set; }
	/// <summary>
	/// 모바일 쿠폰 형태 0:상품쿠폰 1:정액쿠폰
	/// </summary>
	[Comment("모바일 쿠폰 형태 0:상품쿠폰 1:정액쿠폰")]
	public string? MCP_FLAG { get; set; }
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