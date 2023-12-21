using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_CNFG_DETAIL")]
public class MST_CNFG_DETAIL : IIdentifyEntity
{
	/// <summary>
	/// 환경세팅코드
	/// </summary>
	[Comment("환경세팅코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("envCode")]
	public string? ENV_SET_CODE { get; set; }
	/// <summary>
	/// 환경세팅값코드
	/// </summary>
	[Comment("환경세팅값코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("envPreferCode")]
	public string? ENV_VALUE_CODE { get; set; }
	/// <summary>
	/// 환경세팅값명
	/// </summary>
	[Comment("환경세팅값명")]
    [JsonPropertyName("envPreferNm")]
	public string? ENV_VALUE_NAME { get; set; }
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
		return "ENV_SET_CODE_ENV_VALUE_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/env/prefer";

    }    
}