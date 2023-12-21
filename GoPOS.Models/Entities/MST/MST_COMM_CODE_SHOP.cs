using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_COMM_CODE_SHOP")]
public class MST_COMM_CODE_SHOP : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 공통코드구분
	/// </summary>
	[Comment("공통코드구분")]
	[Key, Column(Order = 2)]
	[Required]
	public string? COM_CODE_FLAG { get; set; }
	/// <summary>
	/// 공통코드
	/// </summary>
	[Comment("공통코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? COM_CODE { get; set; }
	/// <summary>
	/// 공통코드명
	/// </summary>
	[Comment("공통코드명")]
	public string? COM_CODE_NAME { get; set; }
	/// <summary>
	/// 공통코드항목1
	/// </summary>
	[Comment("공통코드항목1")]
	public string? COM_CODE_ITEM_01 { get; set; }
	/// <summary>
	/// 공통코드항목2
	/// </summary>
	[Comment("공통코드항목2")]
	public string? COM_CODE_ITEM_02 { get; set; }
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
		return "SHOP_CODE_COM_CODE_FLAG_COM_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}