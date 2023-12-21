using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_DLV_SMS")]
public class MST_DLV_SMS : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 배달 메시지 코드
	/// </summary>
	[Comment("배달 메시지 코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? DLV_MSG_CODE { get; set; }
	/// <summary>
	/// 기본 전송 여부 (0:일반 1:기본-기본은 매장당 1개만 가능)
	/// </summary>
	[Comment("기본 전송 여부 (0:일반 1:기본-기본은 매장당 1개만 가능)")]
	public string? BASE_FLAG { get; set; }
	/// <summary>
	/// 전송 문구
	/// </summary>
	[Comment("전송 문구")]
	public string? DLV_MSG { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일시
	/// </summary>
	[Comment("수정일시")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_DLV_MSG_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}