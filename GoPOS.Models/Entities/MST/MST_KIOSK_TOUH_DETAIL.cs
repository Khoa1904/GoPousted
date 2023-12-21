using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KIOSK_TOUH_DETAIL")]
public class MST_KIOSK_TOUH_DETAIL : IIdentifyEntity
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
	/// 국가코드
	/// </summary>
	[Comment("국가코드")]
	[Key, Column(Order = 5)]
	[Required]
	public string? NATION_CODE { get; set; }
	/// <summary>
	/// 상품폰트명
	/// </summary>
	[Comment("상품폰트명")]
	public string? PRD_FONT { get; set; }
	/// <summary>
	/// 상품폰트사이즈
	/// </summary>
	[Comment("상품폰트사이즈")]
	public string? PRD_FONT_SIZE { get; set; }
	/// <summary>
	/// 상품폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄
	/// </summary>
	[Comment("상품폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄")]
	public string? PRD_FONT_STYLE_FLAG { get; set; }
	/// <summary>
	/// 상품폰트색상값 RGB
	/// </summary>
	[Comment("상품폰트색상값 RGB")]
	public string? PRD_FONT_COLOR { get; set; }
	/// <summary>
	/// 가격폰트명
	/// </summary>
	[Comment("가격폰트명")]
	public string? PRICE_FONT { get; set; }
	/// <summary>
	/// 가격폰트사이즈
	/// </summary>
	[Comment("가격폰트사이즈")]
	public string? PRICE_FONT_SIZE { get; set; }
	/// <summary>
	/// 가격폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄
	/// </summary>
	[Comment("가격폰트형태 0:기본 1:굵게 2:기울임 3:굵게기울임 4:밑줄")]
	public string? PRICE_FONT_STYLE_FLAG { get; set; }
	/// <summary>
	/// 가격폰트색상값 RGB
	/// </summary>
	[Comment("가격폰트색상값 RGB")]
	public string? PRICE_FONT_COLOR { get; set; }
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
		return "SHOP_CODE_POS_NO_TU_CLASS_CODE_TU_KEY_CODE_NATION_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}