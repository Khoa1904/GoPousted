using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KICN_PRNT_CNFG")]
public class MST_KICN_PRNT_CNFG : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 주방프린터번호
	/// </summary>
	[Comment("주방프린터번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? KIT_PRT_NO { get; set; }
	/// <summary>
	/// 환경세팅코드
	/// </summary>
	[Comment("환경세팅코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? ENV_SET_CODE { get; set; }
	/// <summary>
	/// 환경세팅값
	/// </summary>
	[Comment("환경세팅값")]
	public string? ENV_SET_VALUE { get; set; }
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
		return "SHOP_CODE_KIT_PRT_NO_ENV_SET_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}