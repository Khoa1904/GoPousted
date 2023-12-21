using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_CHGE_PRNT_GROUP")]
public class MST_CHGE_PRNT_GROUP : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 프린터번호
	/// </summary>
	[Comment("프린터번호")]
	[Key, Column(Order = 3)]
	[Required]
	public string? PRT_NO { get; set; }
	/// <summary>
	/// 전용프린터구분
	/// </summary>
	[Comment("전용프린터구분")]
	public string? PRT_FLAG { get; set; }
	/// <summary>
	/// 입력일시
	/// </summary>
	[Comment("입력일시")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_POS_NO_PRT_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}