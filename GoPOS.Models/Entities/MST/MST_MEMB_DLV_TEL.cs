using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_MEMB_DLV_TEL")]
public class MST_MEMB_DLV_TEL : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 배달전화-일련번호
	/// </summary>
	[Comment("배달전화-일련번호")]
	[Key, Column(Order = 3)]
	[Required]
	public string? DLV_TEL_SEQ { get; set; }
	/// <summary>
	/// 배달-전화번호
	/// </summary>
	[Comment("배달-전화번호")]
	public string? DLV_TEL_NO { get; set; }
	/// <summary>
	/// 배달-전화단축번호
	/// </summary>
	[Comment("배달-전화단축번호")]
	public string? DLV_S_TEL_NO { get; set; }
	/// <summary>
	/// 회원관리주체-소속코드
	/// </summary>
	[Comment("회원관리주체-소속코드")]
	public string? CST_OGN_CODE { get; set; }
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
		return "SHOP_CODE_CST_NO_DLV_TEL_SEQ";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}