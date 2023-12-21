using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_SIDE_DETP_CODE")]
public class MST_SIDE_DETP_CODE : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴-속성코드
	/// </summary>
	[Comment("사이드메뉴-속성코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("attrCode")]
	public string? SDA_CODE { get; set; }
	/// <summary>
	/// 사이드메뉴-속성명
	/// </summary>
	[Comment("사이드메뉴-속성명")]
    [JsonPropertyName("attrNm")]
	public string? SDA_NAME { get; set; }
	/// <summary>
	/// 사이드메뉴-속성분류코드
	/// </summary>
	[Comment("사이드메뉴-속성분류코드")]
    [JsonPropertyName("attrgrpCode")]
	public string? SDA_CLASS_CODE { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 사이드메뉴-속성SEQ
	/// </summary>
	[Comment("사이드메뉴-속성SEQ")]
    [JsonPropertyName("attrSeq")]
	public Int16 SDA_ORDER_SEQ { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
    [JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
    [JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SDA_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/sidemenu/attribute";
    }    
}