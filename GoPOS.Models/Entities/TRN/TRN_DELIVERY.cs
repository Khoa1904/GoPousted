using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_DELIVERY")]
public class TRN_DELIVERY : IIdentifyEntity
{
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 2)]
	[Required]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 3)]
	[Required]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 4)]
	[Required]
	public string? BILL_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SEQ_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SALE_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? NEW_DLV_ADDR_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_ADDR { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_ADDR_DTL { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? NEW_DLV_TEL_NO_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_TEL_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_PAY_TYPE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_IDT_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_REMARK { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_CL_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_CM_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_START_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_PAYIN_EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_PAYIN_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_BOWLIN_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_BOWLIN_EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DLV_BOWLIN_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}