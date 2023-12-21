using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_DLV_AREA_LCLASS")]
public class MST_DLV_AREA_LCLASS : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 배달구역코드-대
	/// </summary>
	[Comment("배달구역코드-대")]
	[Key, Column(Order = 2)]
	[Required]
	public string? DLV_CL_CODE { get; set; }
	/// <summary>
	/// 배달구역명-대
	/// </summary>
	[Comment("배달구역명-대")]
	public string? DLV_CL_NAME { get; set; }
	/// <summary>
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 판매원코드
	/// </summary>
	[Comment("판매원코드")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 변경구분 0:미변경(서버수신) 1:변경(로컬변경)
	/// </summary>
	[Comment("변경구분 0:미변경(서버수신) 1:변경(로컬변경)")]
	public string? MODIFY_FLAG { get; set; }
	/// <summary>
	/// 서버전송구분 0:미전송 1:전송
	/// </summary>
	[Comment("서버전송구분 0:미전송 1:전송")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 서버전송시각
	/// </summary>
	[Comment("서버전송시각")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// 화면포지션번호
	/// </summary>
	[Comment("화면포지션번호")]
	public Int16 POSITION_NO { get; set; }
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
		return "SHOP_CODE_DLV_CL_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}