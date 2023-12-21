using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_PRD_PRNT_MAP")]
public class MST_PRD_PRNT_MAP : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 프린터번호
	/// </summary>
	[Comment("프린터번호")]
	[Key, Column(Order = 3)]
	[Required]
	public string? PRT_NO { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 등록일
	/// </summary>
	[Comment("등록일")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일
	/// </summary>
	[Comment("수정일")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_PRD_CODE_PRT_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}