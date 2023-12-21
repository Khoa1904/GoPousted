using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KICN_PRNT_INFO")]
public class MST_KICN_PRNT_INFO : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 프린터번호
	/// </summary>
	[Comment("프린터번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? PRT_NO { get; set; }
	/// <summary>
	/// 변경프린터포트
	/// </summary>
	[Comment("변경프린터포트")]
	[Key, Column(Order = 3)]
	[Required]
	public string? DST_PRT_NO { get; set; }
	/// <summary>
	/// 출력시분:시작
	/// </summary>
	[Comment("출력시분:시작")]
	[Key, Column(Order = 4)]
	[Required]
	public string? PRT_TM_FROM { get; set; }
	/// <summary>
	/// 출력시분:종료
	/// </summary>
	[Comment("출력시분:종료")]
	[Key, Column(Order = 5)]
	[Required]
	public string? PRT_TM_TO { get; set; }
	/// <summary>
	/// 출력구분 0변경 1:추가
	/// </summary>
	[Comment("출력구분 0변경 1:추가")]
	public string? PRT_FLAG { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_PRT_NO_DST_PRT_NO_PRT_TM_FROM_PRT_TM_TO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}