using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_BRAND")]
public class MST_INFO_BRAND : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 브랜드코드
	/// </summary>
	[Comment("브랜드코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? BRAND_CODE { get; set; }
	/// <summary>
	/// 브랜드명
	/// </summary>
	[Comment("브랜드명")]
	public string? BRAND_NAME { get; set; }
	/// <summary>
	/// 수입구분 0:국내 1:수입
	/// </summary>
	[Comment("수입구분 0:국내 1:수입")]
	public string? IMPORT_FLAG { get; set; }
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
		return "SHOP_CODE_BRAND_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}