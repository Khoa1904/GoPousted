using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace GoPOS.Models;

[Table("MST_TUCH_CLASS")]
public class MST_TUCH_CLASS : IIdentifyEntity
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
	/// 판매주문구분 (S:판매 O:발주)
	/// </summary>
	[Comment("판매주문구분 (S:판매 O:발주)")]
	[Key, Column(Order = 2)]
	[Required]
	[DefaultValue("S")]
	[JsonPropertyName("saleSeCode")]
	public string? TU_FLAG { get; set; } = "S";
	/// <summary>
	/// 터치분류코드
	/// </summary>
	[Comment("터치분류코드")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("tchkeyGrpCode")]
	public string? TU_CLASS_CODE { get; set; }
	/// <summary>
	/// 터치분류명
	/// </summary>
	[Comment("터치분류명")]
    [JsonPropertyName("tchkeyGrpNm")]
	public string? TU_CLASS_NAME { get; set; }
	/// <summary>
	/// 페이지수
	/// </summary>
	[Comment("페이지수")]
    [JsonPropertyName("tchkeyGrpPageNo")]
	public string? TU_PAGE { get; set; }
	/// <summary>
	/// 메뉴위치
	/// </summary>
	[Comment("메뉴위치")]
    [JsonPropertyName("tchkeyGrpPositionNo")]
	public string? POSITION_NO { get; set; }
	/// <summary>
	/// 위치X
	/// </summary>
	[Comment("위치X")]
	public Int16 X { get; set; }
    /// <summary>
    /// 위치Y
    /// </summary>
    [Comment("위치Y")]
	public Int16 Y { get; set; }
    /// <summary>
    /// 폭
    /// </summary>
    [Comment("폭")]
    [JsonPropertyName("tchkeyGrpWidth")]
	public string? WIDTH { get; set; }
	/// <summary>
	/// 높이
	/// </summary>
	[Comment("높이")]
    [JsonPropertyName("tchkeyGrpHeight")]
	public string? HEIGHT { get; set; }
	/// <summary>
	/// 등록일
	/// </summary>
	[Comment("등록일")]
    [JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일
	/// </summary>
	[Comment("수정일")]
    [JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_TU_FLAG_TU_CLASS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/touchkey";
    }    
}