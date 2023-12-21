using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_CNFG_CODE")]
public class MST_CNFG_CODE : IIdentifyEntity
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
	/// 환경세팅명
	/// </summary>
	[Comment("환경세팅명")]
    [JsonPropertyName("envCodeNm")]
    public string? ENV_SET_NAME { get; set; }
	/// <summary>
	/// 환경세팅구분 (CCD_CODEM_T : 005)
	/// </summary>
	[Comment("환경세팅구분 (CCD_CODEM_T : 005)")]
    [JsonPropertyName("envSetFg")]
    public string? ENV_SET_FLAG { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
    public string? USE_YN { get; set; }
	/// <summary>
	/// 환경그룹코드 (CCD_CODEM_T : 117)
	/// </summary>
	[Comment("환경그룹코드 (CCD_CODEM_T : 117)")]
    [JsonPropertyName("grpCode")]
	public string? ENV_GROUP_CODE { get; set; }
	/// <summary>
	/// 비고
	/// </summary>
	[Comment("비고")]
    [JsonPropertyName("envCodeNote")]
	public string? REMARK { get; set; }
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
		return "ENV_SET_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/env";
    }    
}