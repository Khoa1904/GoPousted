using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_VAN_EASYPAYMAP")]
public class MST_INFO_VAN_EASYPAYMAP : IIdentifyEntity
{
	/// <summary>
	/// 밴사코드
	/// </summary>
	[Comment("밴사코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("vanCode")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 밴간편결제코드
	/// </summary>
	[Comment("밴간편결제코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("vanPayCorpCode")]
	public string? VAN_PAYCP_CODE { get; set; }
	/// <summary>
	/// 밴간편결제명
	/// </summary>
	[Comment("밴간편결제명")]
    [JsonPropertyName("vanPayCorpNm")]
	public string? VAN_PAYCP_NAME { get; set; }
	/// <summary>
	/// 간편결제코드
	/// </summary>
	[Comment("간편결제코드")]
    [JsonPropertyName("payCorpCode")]
	public string? PAYCP_CODE { get; set; }
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
		return "VAN_CODE_VAN_PAYCP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/pay-corp/van";

    }    
}