using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_MEMB_DLV_ADDR")]
public class MST_MEMB_DLV_ADDR : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 배달지-일련번호
	/// </summary>
	[Comment("배달지-일련번호")]
	[Key, Column(Order = 3)]
	[Required]
	public string? DLV_ADDR_SEQ { get; set; }
	/// <summary>
	/// 배달-주소
	/// </summary>
	[Comment("배달-주소")]
	public string? DLV_ADDR { get; set; }
	/// <summary>
	/// 배달-상세주소
	/// </summary>
	[Comment("배달-상세주소")]
	public string? DLV_ADDR_DTL { get; set; }
	/// <summary>
	/// 회원관리주체-소속코드
	/// </summary>
	[Comment("회원관리주체-소속코드")]
	public string? CST_OGN_CODE { get; set; }
	/// <summary>
	/// 최종배달일자
	/// </summary>
	[Comment("최종배달일자")]
	public string? DLV_LAST_DATE { get; set; }
	/// <summary>
	/// 배달-누적횟수
	/// </summary>
	[Comment("배달-누적횟수")]
	public int DLV_ACC_CNT { get; set; }
	/// <summary>
	/// 배달구역-대분류코드
	/// </summary>
	[Comment("배달구역-대분류코드")]
	public string? DLV_CL_CODE { get; set; }
	/// <summary>
	/// 배달구역-중분류코드
	/// </summary>
	[Comment("배달구역-중분류코드")]
	public string? DLV_CM_CODE { get; set; }
	/// <summary>
	/// 배달IF전송용주소
	/// </summary>
	[Comment("배달IF전송용주소")]
	public string? DLV_REMARK { get; set; }
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
		return "SHOP_CODE_CST_NO_DLV_ADDR_SEQ";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}