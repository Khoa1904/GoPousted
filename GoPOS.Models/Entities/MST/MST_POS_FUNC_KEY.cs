using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_POS_FUNC_KEY")]
public class MST_POS_FUNC_KEY : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 기능키번호
	/// </summary>
	[Comment("기능키번호")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("posfuncNo")]
	public string? FN_NO { get; set; }
	/// <summary>
	/// 기능구분 (CCD_CODEM_T : 061)  01:영업관리 02:판매기능메뉴 03:매출현황 04:수발주 05:환경설정 06:결제메뉴 08:복합결제항목 09:출력항목 10:테이블-일반 11:테이블-포장 12:테이블-배달  99:공통메뉴
	/// </summary>
	[Comment("기능구분 (CCD_CODEM_T : 061)  01:영업관리 02:판매기능메뉴 03:매출현황 04:수발주 05:환경설정 06:결제메뉴 08:복합결제항목 09:출력항목 10:테이블-일반 11:테이블-포장 12:테이블-배달  99:공통메뉴")]
    [JsonPropertyName("posfuncGrpCode")]
	public string? FN_FLAG { get; set; }
	/// <summary>
	/// 포스기능명
	/// </summary>
	[Comment("포스기능명")]
    [JsonPropertyName("posfuncNm")]
	public string? FN_NAME { get; set; }
	/// <summary>
	/// 메뉴위치 (결제메뉴 제외) 결제메뉴 제외
	/// </summary>
	[Comment("메뉴위치 (결제메뉴 제외) 결제메뉴 제외")]
    [JsonPropertyName("positionNo")]
	public Int16 POSITION_NO { get; set; }
	/// <summary>
	/// x 결제메뉴 사용
	/// </summary>
	[Comment("x 결제메뉴 사용")]
	public Int16 COL_NUM { get; set; }
	/// <summary>
	/// y 결제메뉴 사용
	/// </summary>
	[Comment("y 결제메뉴 사용")]
	public Int16 ROW_NUM { get; set; }
	/// <summary>
	/// 폭 결제메뉴 사용
	/// </summary>
	[Comment("폭 결제메뉴 사용")]
    [JsonPropertyName("posfuncWidth")]
	public Int16 WIDTH_NUM { get; set; }
	/// <summary>
	/// 높이 결제메뉴 사용
	/// </summary>
	[Comment("높이 결제메뉴 사용")]
    [JsonPropertyName("posfuncHeight")]
	public Int16 HEIGHT_NUM { get; set; }
	/// <summary>
	/// 권한여부
	/// </summary>
	[Comment("권한여부")]
    [JsonPropertyName("authAt")]
	public string? AUTH_YN { get; set; }
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
		return "SHOP_CODE_FN_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/funckey/pos";
    }    
}