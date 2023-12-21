using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_VAN_CARDMAP")]
public class MST_INFO_VAN_CARDMAP : IIdentifyEntity
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
	/// 밴사-카드사코드
	/// </summary>
	[Comment("밴사-카드사코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("vanCardCorpCode")]
	public string? VAN_CRDCP_CODE { get; set; }
	/// <summary>
	/// 밴사-카드사명
	/// </summary>
	[Comment("밴사-카드사명")]
    [JsonPropertyName("vanCardCorpNm")]
	public string? VAN_CRDCP_NAME { get; set; }
	/// <summary>
	/// 카드사코드
	/// </summary>
	[Comment("카드사코드")]
    [JsonPropertyName("cardCorpCode")]
	public string? CRDCP_CODE { get; set; }
	/// <summary>
	/// TRS 사용유무 (0:미사용,1:사용)
	/// </summary>
	[Comment("TRS 사용유무 (0:미사용,1:사용)")]
	public string? TRS_FLAG { get; set; }
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
		return "VAN_CODE_VAN_CRDCP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/card-corp/van";

    }    
}