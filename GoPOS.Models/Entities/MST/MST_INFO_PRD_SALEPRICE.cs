using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_PRD_SALEPRICE")]
public class MST_INFO_PRD_SALEPRICE : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 단가변경코드
	/// </summary>
	[Comment("단가변경코드")]
	[Key, Column(Order = 3)]
	[Required]
	public string? PRC_MG_CODE { get; set; }
	/// <summary>
	/// 판매단가변경명칭
	/// </summary>
	[Comment("판매단가변경명칭")]
	public string? PRC_MG_NAME { get; set; }
	/// <summary>
	/// 사용코드 PGM_TYPE_FG=0,1 -> PROD_CD , PGM_TYPE_FG=2 => STYLE_CD
	/// </summary>
	[Comment("사용코드 PGM_TYPE_FG=0,1 -> PROD_CD , PGM_TYPE_FG=2 => STYLE_CD")]
	[Key, Column(Order = 2)]
	[Required]
	public string? STYLE_PRD_CODE { get; set; }
	/// <summary>
	/// 변경시작일
	/// </summary>
	[Comment("변경시작일")]
	public string? S_DATE { get; set; }
	/// <summary>
	/// 변경종료일
	/// </summary>
	[Comment("변경종료일")]
	public string? E_DATE { get; set; }
	/// <summary>
	/// 가격변동구분 인상/하:0 SALE:1
	/// </summary>
	[Comment("가격변동구분 인상/하:0 SALE:1")]
	public string? SALE_PRICE_FLAG { get; set; }
	/// <summary>
	/// 판매마진코드
	/// </summary>
	[Comment("판매마진코드")]
	public string? SALE_MG_CODE { get; set; }
	/// <summary>
	/// 정상가
	/// </summary>
	[Comment("정상가")]
	[Precision(10, 2)]
	public decimal NORMAL_UPRC { get; set; }
	/// <summary>
	/// 판매단가
	/// </summary>
	[Comment("판매단가")]
	[Precision(10, 2)]
	public decimal SALE_UPRC { get; set; }
	/// <summary>
	/// 판매형태 (H/SCD_CODEM_T=301)
	/// </summary>
	[Comment("판매형태 (H/SCD_CODEM_T=301)")]
	public string? SALE_MG_FLAG { get; set; }
	/// <summary>
	/// 판매할인율
	/// </summary>
	[Comment("판매할인율")]
	public Int16 SALE_DC_RATE { get; set; }
	/// <summary>
	/// 마진율
	/// </summary>
	[Comment("마진율")]
	public Int16 MG_RATE { get; set; }
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
		return "SHOP_CODE_PRC_MG_CODE_STYLE_PRD_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}