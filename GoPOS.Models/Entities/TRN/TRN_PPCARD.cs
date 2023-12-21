using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_PPCARD")]
public class TRN_PPCARD : IIdentifyEntity
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
	/// 선결제-회원번호
	/// </summary>
	[Comment("선결제-회원번호")]
	public string? PPC_CST_NO { get; set; }
	/// <summary>
	/// 선결제-금액
	/// </summary>
	[Comment("선결제-금액")]
	[Precision(12, 2)]
	public decimal PPC_AMT { get; set; }
	/// <summary>
	/// 선결제-잔액
	/// </summary>
	[Comment("선결제-잔액")]
	[Precision(12, 2)]
	public decimal PPC_BALANCE { get; set; }
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
    /// <summary>
    /// 승인번호
    /// </summary>
    [Comment("")]
    public string? ORG_APPR_INFO { get; set; }
    /// <summary>
    /// 승인번호
    /// </summary>
    [Comment("승인번호")]
    public string? APPR_NO { get; set; }
    [Comment("")]
    public string? ORG_APPR_NO { get; set; }
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