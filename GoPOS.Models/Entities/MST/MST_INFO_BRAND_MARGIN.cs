using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_BRAND_MARGIN")]
public class MST_INFO_BRAND_MARGIN : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 판매마진코드
	/// </summary>
	[Comment("판매마진코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? SALE_MG_CODE { get; set; }
	/// <summary>
	/// 브랜드코드
	/// </summary>
	[Comment("브랜드코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? BRAND_CODE { get; set; }
	/// <summary>
	/// 판매형태 (SCD_CODEM_T : 301)
	/// </summary>
	[Comment("판매형태 (SCD_CODEM_T : 301)")]
	[Key, Column(Order = 4)]
	[Required]
	public string? SALE_MG_FLAG { get; set; }
	/// <summary>
	/// 판매할인율
	/// </summary>
	[Comment("판매할인율")]
	public Int16 SALE_DC_RATE { get; set; }
	/// <summary>
	/// 마진율
	/// </summary>
	[Comment("마진율")]
	public Int16 MG_RATE { get; set; }
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
		return "SHOP_CODE_SALE_MG_CODE_BRAND_CODE_SALE_MG_FLAG";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}