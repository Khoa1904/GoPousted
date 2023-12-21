using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_RTN_REASON")]
public class MST_INFO_RTN_REASON : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	[JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 메세지구분  (0:반품 1:취소)
	/// </summary>
	[Comment("메세지구분  (0:반품 1:취소)")]
	[Key, Column(Order = 2)]
	[Required]
	[JsonPropertyName("msgSeCode")]
	public string? MSG_FLAG { get; set; }
	/// <summary>
	/// 메세지코드
	/// </summary>
	[Comment("메세지코드")]
	[Key, Column(Order = 3)]
	[Required]
	[JsonPropertyName("msgCode")]
	public string? MSG_CODE { get; set; }
	/// <summary>
	/// 메세지
	/// </summary>
	[Comment("메세지")]
	[JsonPropertyName("msgName")]
	public string? MSG_NAME { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	[JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	[JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일시
	/// </summary>
	[Comment("수정일시")]
	[JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_MSG_FLAG_MSG_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/return/msg";
    }    
}