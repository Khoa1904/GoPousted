using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KICN_MEMO")]
public class MST_KICN_MEMO : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 주방메모코드
	/// </summary>
	[Comment("주방메모코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? MEMO_CODE { get; set; }
	/// <summary>
	/// 주방메모명칭
	/// </summary>
	[Comment("주방메모명칭")]
	public string? MEMO_NAME { get; set; }
	/// <summary>
	/// 사용유무
	/// </summary>
	[Comment("사용유무")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 최초입력일시
	/// </summary>
	[Comment("최초입력일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_MEMO_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}