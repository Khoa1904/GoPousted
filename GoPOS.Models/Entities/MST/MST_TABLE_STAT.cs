using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_TABLE_STAT")]
public class MST_TABLE_STAT : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 테이블코드
	/// </summary>
	[Comment("테이블코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? TABLE_CODE { get; set; }
	/// <summary>
	/// 테이블정보코드 (공통코드) 700
	/// </summary>
	[Comment("테이블정보코드 (공통코드) 700")]
	[Key, Column(Order = 3)]
	[Required]
	public string? COM_CODE { get; set; }
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
		return "SHOP_CODE_TABLE_CODE_COM_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}