using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KIOSK_TOUH_CLASS_DETAIL")]
public class MST_KIOSK_TOUH_CLASS_DETAIL : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 터치분류코드
	/// </summary>
	[Comment("터치분류코드")]
	public string? TU_CLASS_CODE { get; set; }
	/// <summary>
	/// 국가코드
	/// </summary>
	[Comment("국가코드")]
	public string? NATION_CODE { get; set; }
	/// <summary>
	/// 폰트명
	/// </summary>
	[Comment("폰트명")]
	public string? FONT { get; set; }
	/// <summary>
	/// 폰트사이즈
	/// </summary>
	[Comment("폰트사이즈")]
	public Int16 FONT_SIZE { get; set; }
	/// <summary>
	/// 폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄
	/// </summary>
	[Comment("폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄")]
	public string? FONT_STYLE_FLAG { get; set; }
	/// <summary>
	/// 폰트색상값 RGB
	/// </summary>
	[Comment("폰트색상값 RGB")]
	public string? FONT_COLOR { get; set; }
	/// <summary>
	/// 배경색상값 RGB
	/// </summary>
	[Comment("배경색상값 RGB")]
	public string? BG_COLOR { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 등록자ID
	/// </summary>
	[Comment("등록자ID")]
	public string? INSERT_ID { get; set; }
	/// <summary>
	/// 수정일시
	/// </summary>
	[Comment("수정일시")]
	public string? UPDATE_DT { get; set; }
	/// <summary>
	/// 수정자ID
	/// </summary>
	[Comment("수정자ID")]
	public string? UPDATE_ID { get; set; }

	public string Base_PrimaryName()
	{
		return "";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}