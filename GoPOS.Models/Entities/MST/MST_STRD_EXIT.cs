using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_STRD_EXIT")]
public class MST_STRD_EXIT : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 퇴장시간구분코드
	/// </summary>
	[Comment("퇴장시간구분코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? EXIT_CODE { get; set; }
	/// <summary>
	/// 퇴장초과시간-분단위
	/// </summary>
	[Comment("퇴장초과시간-분단위")]
	public string? EXIT_MM { get; set; }
	/// <summary>
	/// 퇴장시간초과금액
	/// </summary>
	[Comment("퇴장시간초과금액")]
	[Precision(10, 0)]
	public decimal EXIT_ADD_AMT { get; set; }
	/// <summary>
	/// 퇴장초과시간-상품코드
	/// </summary>
	[Comment("퇴장초과시간-상품코드")]
	public string? EXIT_ADD_PRD_CODE { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
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
		return "SHOP_CODE_EXIT_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}