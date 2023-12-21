using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_MEMB_PNT_INFO")]
public class MST_MEMB_PNT_INFO : IIdentifyEntity
{
	/// <summary>
	/// 회원관리주체-소속코드
	/// </summary>
	[Comment("회원관리주체-소속코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? CST_OGN_CODE { get; set; }
	/// <summary>
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 회원-누적매출횟수
	/// </summary>
	[Comment("회원-누적매출횟수")]
	[Precision(15, 0)]
	public decimal ACC_SALE_CNT { get; set; }
	/// <summary>
	/// 회원-누적매출금액
	/// </summary>
	[Comment("회원-누적매출금액")]
	[Precision(15, 0)]
	public decimal ACC_SALE_AMT { get; set; }
	/// <summary>
	/// 회원-누적포인트
	/// </summary>
	[Comment("회원-누적포인트")]
	[Precision(15, 0)]
	public decimal ACC_POINT { get; set; }
	/// <summary>
	/// 회원-사용포인트
	/// </summary>
	[Comment("회원-사용포인트")]
	[Precision(15, 0)]
	public decimal USE_POINT { get; set; }
	/// <summary>
	/// 회원-가용포인트
	/// </summary>
	[Comment("회원-가용포인트")]
	[Precision(15, 0)]
	public decimal AVL_POINT { get; set; }
	/// <summary>
	/// 회원-조정포인트
	/// </summary>
	[Comment("회원-조정포인트")]
	[Precision(15, 0)]
	public decimal ADJ_POINT { get; set; }
	/// <summary>
	/// 회원-포인트누적횟수
	/// </summary>
	[Comment("회원-포인트누적횟수")]
	[Precision(15, 0)]
	public decimal POINT_ACC_CNT { get; set; }
	/// <summary>
	/// 회원-포인트사용횟수
	/// </summary>
	[Comment("회원-포인트사용횟수")]
	[Precision(15, 0)]
	public decimal POINT_USE_CNT { get; set; }
	/// <summary>
	/// 회원-최초매출일자
	/// </summary>
	[Comment("회원-최초매출일자")]
	public string? F_SALE_DATE { get; set; }
	/// <summary>
	/// 회원-최종매출일자
	/// </summary>
	[Comment("회원-최종매출일자")]
	public string? L_SALE_DATE { get; set; }
	/// <summary>
	/// 등록매장코드
	/// </summary>
	[Comment("등록매장코드")]
	public string? INSERT_SHOP_CODE { get; set; }
	/// <summary>
	/// 등록매장-누적매출횟수
	/// </summary>
	[Comment("등록매장-누적매출횟수")]
	[Precision(15, 0)]
	public decimal INSERT_ACC_SALE_CNT { get; set; }
	/// <summary>
	/// 등록매장-누적매출금액
	/// </summary>
	[Comment("등록매장-누적매출금액")]
	[Precision(15, 0)]
	public decimal INSERT_ACC_SALE_AMT { get; set; }
	/// <summary>
	/// 등록매장-최초매출일자
	/// </summary>
	[Comment("등록매장-최초매출일자")]
	public string? INSERT_F_SALE_DATE { get; set; }
	/// <summary>
	/// 등록매장-최종매출일자
	/// </summary>
	[Comment("등록매장-최종매출일자")]
	public string? INSERT_L_SALE_DATE { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "CST_OGN_CODE_CST_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}