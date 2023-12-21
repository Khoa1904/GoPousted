using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_COMM_CODE")]
public class MST_COMM_CODE : IIdentifyEntity
{
	/// <summary>
	/// 공통코드구분
	/// </summary>
	[Comment("공통코드구분")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("codeGrp")]
	public string? COM_CODE_FLAG { get; set; }
	/// <summary>
	/// 공통코드
	/// </summary>
	[Comment("공통코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("code")]
	public string? COM_CODE { get; set; }
	/// <summary>
	/// 공통코드명
	/// </summary>
	[Comment("공통코드명")]
    [JsonPropertyName("codeNm")]
	public string? COM_CODE_NAME { get; set; }
	/// <summary>
	/// 공통코드항목1
	/// </summary>
	[Comment("공통코드항목1")]
    [JsonPropertyName("codeCntsts1")]
	public string? COM_CODE_ITEM_01 { get; set; }
	/// <summary>
	/// 공통코드항목2
	/// </summary>
	[Comment("공통코드항목2")]
    [JsonPropertyName("codeCntsts2")]
	public string? COM_CODE_ITEM_02 { get; set; }
	/// <summary>
	/// 사용컬럼명
	/// </summary>
	[Comment("사용컬럼명")]
    [JsonPropertyName("useColNm")]
	public string? USE_COL_NAME { get; set; }
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
		return "COM_CODE_FLAG_COM_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/cmcode";
	}    
}