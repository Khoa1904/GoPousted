using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_EASYPAY")]
public class MST_INFO_EASYPAY : IIdentifyEntity
{
	/// <summary>
	/// 간편결제코드
	/// </summary>
	[Comment("간편결제코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("payCorpCode")]
	public string? PAYCP_CODE { get; set; }
	/// <summary>
	/// 간편결제이름
	/// </summary>
	[Comment("간편결제이름")]
    [JsonPropertyName("payCorpNm")]
	public string? PAYCP_NAME { get; set; }
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
		return "PAYCP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/pay-corp";

    }    
}