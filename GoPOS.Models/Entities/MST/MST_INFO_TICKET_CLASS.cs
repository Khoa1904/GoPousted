using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_TICKET_CLASS")]
public class MST_INFO_TICKET_CLASS : IIdentifyEntity
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
	/// 티켓분류구분 (CCD_CODEM_T : 017) 1:상품권 2:쿠폰 3:식권
	/// </summary>
	[Comment("티켓분류구분 (CCD_CODEM_T : 017) 1:상품권 2:쿠폰 3:식권")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("paymentMethodSeCode")]
	public string? TK_CLASS_FLAG { get; set; }
	/// <summary>
	/// 티켓분류코드
	/// </summary>
	[Comment("티켓분류코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("ctgCode")]
	public string? TK_CLASS_CODE { get; set; }
	/// <summary>
	/// 티켓분류명
	/// </summary>
	[Comment("티켓분류명")]
    [JsonPropertyName("ctgNm")]
	public string? TK_CLASS_NAME { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
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
		return "SHOP_CODE_TK_CLASS_FLAG_TK_CLASS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment/ctg";
    }    
}