using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_SHOP_FUNC_KEY")]
public class MST_SHOP_FUNC_KEY : IIdentifyEntity
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
	public string? FK_NO { get; set; }
	/// <summary>
	/// 기능키구분 (CCD_CODEM_T : 061)  01:영업관리 02:판매기능메뉴 03:매출현황 04:수발주 05:환경설정 06:결제메뉴 08:복합결제항목 09:출력항목 10:테이블-일반 11:테이블-포장 12:테이블-배달  99:공통메뉴
	/// </summary>
	[Comment("기능키구분 (CCD_CODEM_T : 061)  01:영업관리 02:판매기능메뉴 03:매출현황 04:수발주 05:환경설정 06:결제메뉴 08:복합결제항목 09:출력항목 10:테이블-일반 11:테이블-포장 12:테이블-배달  99:공통메뉴")]
    [JsonPropertyName("posfuncGrpCode")]
	public string? FK_FLAG { get; set; }
	/// <summary>
	/// 기능키명
	/// </summary>
	[Comment("기능키명")]
    [JsonPropertyName("posfuncNm")]
	public string? FK_NAME { get; set; }
	/// <summary>
	/// 권한여부
	/// </summary>
	[Comment("권한여부")]
    [JsonPropertyName("posfuncAuthAt")]
	public string? AUTH_YN { get; set; }
	/// <summary>
	/// 이미지명
	/// </summary>
	[Comment("이미지명")]
	public string? IMG_FILE_NAME { get; set; }
	/// <summary>
	/// 위치조정 가능여부 결제메뉴에 한하여 사용한다
	/// </summary>
	[Comment("위치조정 가능여부 결제메뉴에 한하여 사용한다")]
    [JsonPropertyName("positionAt")]
	public string? POSITION_YN { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
    [JsonPropertyName("storeGcode")]
	public string? PRD_CODE { get; set; }
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
		return "SHOP_CODE_FK_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/funckey";
    }    
}