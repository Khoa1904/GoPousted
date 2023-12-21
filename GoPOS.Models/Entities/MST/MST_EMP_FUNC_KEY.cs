using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_EMP_FUNC_KEY")]
public class MST_EMP_FUNC_KEY : IIdentifyEntity
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
	/// 사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달
	/// </summary>
	[Comment("사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("staffSeCode")]
	public string? EMP_FLAG { get; set; }
	/// <summary>
	/// 기능키번호
	/// </summary>
	[Comment("기능키번호")]
	[Key, Column(Order = 3)]
	[Required]
    [JsonPropertyName("posfuncNo")]
	public string? FK_NO { get; set; }
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
		return "SHOP_CODE_EMP_FLAG_FK_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/funckey/staff";
    }    
}