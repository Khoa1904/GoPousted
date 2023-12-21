using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_PRDTYPE")]
public class MST_INFO_PRDTYPE : IIdentifyEntity
{
	/// <summary>
	/// 소속코드 (4자리:본사, 6자리:매장)
	/// </summary>
	[Comment("소속코드 (4자리:본사, 6자리:매장)")]
	[Key, Column(Order = 1)]
	[Required]
	public string? OGN_CODE { get; set; }
	/// <summary>
	/// 구성코드 (CCD:052)
	/// </summary>
	[Comment("구성코드 (CCD:052)")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CODE_CODE { get; set; }
	/// <summary>
	/// 구성코드자릿수
	/// </summary>
	[Comment("구성코드자릿수")]
	public Int16 CODE_LEN { get; set; }
	/// <summary>
	/// 상품코드순서
	/// </summary>
	[Comment("상품코드순서")]
	public Int16 PRD_SEQ_NO { get; set; }
	/// <summary>
	/// 상품코드구성여부
	/// </summary>
	[Comment("상품코드구성여부")]
	public string? PRD_CODE_YN { get; set; }
	/// <summary>
	/// 스타일코드순서
	/// </summary>
	[Comment("스타일코드순서")]
	public Int16 STYLE_SEQ_NO { get; set; }
	/// <summary>
	/// 스타일코드구성여부
	/// </summary>
	[Comment("스타일코드구성여부")]
	public string? STYLE_CODE_YN { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "OGN_CODE_CODE_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}