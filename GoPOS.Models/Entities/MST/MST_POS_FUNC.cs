using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_POS_FUNC")]
public class MST_POS_FUNC : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달
	/// </summary>
	[Comment("사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달")]
	[Key, Column(Order = 2)]
	[Required]
	public string? EMP_FLAG { get; set; }
	/// <summary>
	/// 포스기능번호 구성:포스기능구분(1)+NO(2)
	/// </summary>
	[Comment("포스기능번호 구성:포스기능구분(1)+NO(2)")]
	[Key, Column(Order = 3)]
	[Required]
	public string? POS_FN_NO { get; set; }
	/// <summary>
	/// 포스기능명
	/// </summary>
	[Comment("포스기능명")]
	public string? POS_FN_NAME { get; set; }
	/// <summary>
	/// 포스기능구분 (CCD_CODEM_T : 061) 1:영업관리 2:일반-판매처리 3:매출현황 4:수발주 5:환경설정 6:외식-테이블메인 7:외식-주문판매
	/// </summary>
	[Comment("포스기능구분 (CCD_CODEM_T : 061) 1:영업관리 2:일반-판매처리 3:매출현황 4:수발주 5:환경설정 6:외식-테이블메인 7:외식-주문판매")]
	public string? POS_FN_FLAG { get; set; }
	/// <summary>
	/// 포스기능위치
	/// </summary>
	[Comment("포스기능위치")]
	public Int16 POS_FN_LOC { get; set; }
	/// <summary>
	/// 권한여부
	/// </summary>
	[Comment("권한여부")]
	public string? AUTH_YN { get; set; }
	/// <summary>
	/// 원천포스기능번호 POS BUTTON CLICK RETURN VALUE
	/// </summary>
	[Comment("원천포스기능번호 POS BUTTON CLICK RETURN VALUE")]
	public string? ORG_POS_FN_NO { get; set; }
	/// <summary>
	/// 일반-사용여부
	/// </summary>
	[Comment("일반-사용여부")]
	public string? FN_USE_YN_0 { get; set; }
	/// <summary>
	/// 외식-사용여부
	/// </summary>
	[Comment("외식-사용여부")]
	public string? FN_USE_YN_1 { get; set; }
	/// <summary>
	/// 의류-사용여부
	/// </summary>
	[Comment("의류-사용여부")]
	public string? FN_USE_YN_2 { get; set; }
	/// <summary>
	/// 일반/의류 이미지파일명
	/// </summary>
	[Comment("일반/의류 이미지파일명")]
	public string? IMG_FILE_NAME_1 { get; set; }
	/// <summary>
	/// 외식 이미지파일명
	/// </summary>
	[Comment("외식 이미지파일명")]
	public string? IMG_FILE_NAME_2 { get; set; }
	/// <summary>
	/// 판매처리-메인화면표기구분
	/// </summary>
	[Comment("판매처리-메인화면표기구분")]
	public string? MAIN_DISP_FLAG { get; set; }
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
		return "SHOP_CODE_EMP_FLAG_POS_FN_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}