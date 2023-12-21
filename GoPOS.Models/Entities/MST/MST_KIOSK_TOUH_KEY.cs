using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KIOSK_TOUH_KEY")]
public class MST_KIOSK_TOUH_KEY : IIdentifyEntity
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
	/// 터치분류코드
	/// </summary>
	[Comment("터치분류코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? TU_CLASS_CODE { get; set; }
	/// <summary>
	/// 터치키코드
	/// </summary>
	[Comment("터치키코드")]
	[Key, Column(Order = 4)]
	[Required]
	public string? TU_KEY_CODE { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 페이지수
	/// </summary>
	[Comment("페이지수")]
	public Int16 TU_PAGE { get; set; }
	/// <summary>
	/// X 좌표
	/// </summary>
	[Comment("X 좌표")]
	public Int16 X { get; set; }
	/// <summary>
	/// Y 좌표
	/// </summary>
	[Comment("Y 좌표")]
	public Int16 Y { get; set; }
	/// <summary>
	/// 너비
	/// </summary>
	[Comment("너비")]
	public Int16 WIDTH { get; set; }
	/// <summary>
	/// 높이
	/// </summary>
	[Comment("높이")]
	public Int16 HEIGHT { get; set; }
	/// <summary>
	/// 등록자ID
	/// </summary>
	[Comment("등록자ID")]
	public string? INSERT_ID { get; set; }
	/// <summary>
	/// 수정자ID
	/// </summary>
	[Comment("수정자ID")]
	public string? UPDATE_ID { get; set; }
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
		return "SHOP_CODE_POS_NO_TU_CLASS_CODE_TU_KEY_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}