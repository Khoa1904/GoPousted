using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace GoPOS.Models;

[Table("MST_INFO_CARD")]
public class MST_INFO_CARD : IIdentifyEntity
{
	/// <summary>
	/// 카드사코드
	/// </summary>
	[Comment("카드사코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("cardCorpCode")]
	public string? CRDCP_CODE { get; set; }
	/// <summary>
	/// 카드사명
	/// </summary>
	[Comment("카드사명")]
    [JsonPropertyName("cardCorpNm")]
	public string? CRDCP_NAME { get; set; }
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

    public string? CARD_ICON_PATH
    {
        get
        {
            return CRDCP_CODE + ".png";
        }
    }

    public string Base_PrimaryName()
	{
		return "CRDCP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/card-corp";

    }    
}