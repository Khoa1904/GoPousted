using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_POS_TUCH_PRODUCT")]
public class MST_POS_TUCH_PRODUCT : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 판매주문구분 (S:판매 O:주문)
	/// </summary>
	[Comment("판매주문구분 (S:판매 O:주문)")]
	[Key, Column(Order = 2)]
	[Required]
	public string? TC_FLAG { get; set; }
	/// <summary>
	/// 터치분류코드
	/// </summary>
	[Comment("터치분류코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? TC_CLASS_CODE { get; set; }
	/// <summary>
	/// 터치키코드
	/// </summary>
	[Comment("터치키코드")]
	[Key, Column(Order = 4)]
	[Required]
	public string? TC_KEY_CODE { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	public string? PRD_CODE { get; set; }
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
		return "SHOP_CODE_TC_FLAG_TC_CLASS_CODE_TC_KEY_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}