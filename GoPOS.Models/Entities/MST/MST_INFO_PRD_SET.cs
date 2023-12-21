using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_PRD_SET")]
public class MST_INFO_PRD_SET : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 상품코드
	/// </summary>
	[Comment("상품코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? PRD_CODE { get; set; }
	/// <summary>
	/// 세트상품구분 (CCD_CODEM_T : 012) 0:일반상품 1:세트상품 2:코스상품
	/// </summary>
	[Comment("세트상품구분 (CCD_CODEM_T : 012) 0:일반상품 1:세트상품 2:코스상품")]
	public string? SET_PRD_FLAG { get; set; }
	/// <summary>
	/// 구성상품코드
	/// </summary>
	[Comment("구성상품코드")]
	public string? UNIT_PRD_CODE { get; set; }
	/// <summary>
	/// 구성상품수량
	/// </summary>
	[Comment("구성상품수량")]
	public int UNIT_PRD_QTY { get; set; }
	/// <summary>
	/// 구성상품순서
	/// </summary>
	[Comment("구성상품순서")]
	public Int16 UNIT_PRD_SEQ { get; set; }
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
		return "SHOP_CODE_PRD_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}