using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_TICKET")]
public class MST_INFO_TICKET : IIdentifyEntity
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
	/// 티켓분류구분 1:상품권 3:식권
	/// </summary>
	[Comment("티켓분류구분 1:상품권 3:식권")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("paymentMethodSeCode")]
	public string? TK_CLASS_FLAG { get; set; }
	/// <summary>
	/// 상품권/식권코드
	/// </summary>
	[Comment("상품권/식권코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("giftCardCode")]
	public string? TK_GFT_CODE { get; set; }
	/// <summary>
	/// 상품권/식권명
	/// </summary>
	[Comment("상품권/식권명")]
    [JsonPropertyName("giftCardNm")]
	public string? TK_GFT_NAME { get; set; }
	/// <summary>
	/// 상품권/식권-액면가
	/// </summary>
	[Comment("상품권/식권-액면가")]
	[Precision(10, 0)]
    [JsonPropertyName("price")]
	public decimal TK_GFT_UPRC { get; set; }
	/// <summary>
	/// 상품권-일련번호사용유무
	/// </summary>
	[Comment("상품권-일련번호사용유무")]
    [JsonPropertyName("serialNoUseAt")]
	public string? TK_GFT_SEQ_YN { get; set; }
	/// <summary>
	/// 티켓분류코드
	/// </summary>
	[Comment("티켓분류코드")]
    [JsonPropertyName("ctgCode")]
	public string? TK_CLASS_CODE { get; set; }
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
		return "SHOP_CODE_TK_CLASS_FLAG_TK_GFT_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/payment";
    }    
}