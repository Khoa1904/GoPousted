using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_KICN_PRINTER")]
public class MST_KICN_PRINTER : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 프린터번호
	/// </summary>
	[Comment("프린터번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? PRT_NO { get; set; }
	/// <summary>
	/// 주방프린터명
	/// </summary>
	[Comment("주방프린터명")]
	public string? PRT_NAME { get; set; }
	/// <summary>
	/// 담당포스번호
	/// </summary>
	[Comment("담당포스번호")]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 프린터 종류 (공통코드에 추가)
	/// </summary>
	[Comment("프린터 종류 (공통코드에 추가)")]
	public string? PRT_TYPE_FLAG { get; set; }
	/// <summary>
	/// 프린터 포트 (공통코드에 추가)
	/// </summary>
	[Comment("프린터 포트 (공통코드에 추가)")]
	public string? PRT_PORT { get; set; }
	/// <summary>
	/// 프린터 속도 (공통코드에 추가)
	/// </summary>
	[Comment("프린터 속도 (공통코드에 추가)")]
	public string? PRT_SPEED { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 주방주문서 출력매수
	/// </summary>
	[Comment("주방주문서 출력매수")]
	public Int16 PRT_PAPER_QTY { get; set; }
	/// <summary>
	/// 층구분
	/// </summary>
	[Comment("층구분")]
	public string? FLOOR_NO { get; set; }
	/// <summary>
	/// 층전용 프린터구분(0:일반 1:전용)
	/// </summary>
	[Comment("층전용 프린터구분(0:일반 1:전용)")]
	public string? FLOOR_FLAG { get; set; }
	/// <summary>
	/// 프린터 TCP IP
	/// </summary>
	[Comment("프린터 TCP IP")]
	public string? PRT_TCP_IP { get; set; }
	/// <summary>
	/// 프린터 TCP PORT
	/// </summary>
	[Comment("프린터 TCP PORT")]
	public string? PRT_TCP_PORT { get; set; }
	/// <summary>
	/// 주방벨 사용여부
	/// </summary>
	[Comment("주방벨 사용여부")]
	public string? PRT_BELL_YN { get; set; }
	/// <summary>
	/// 등록일
	/// </summary>
	[Comment("등록일")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 수정일
	/// </summary>
	[Comment("수정일")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_PRT_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}