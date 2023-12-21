using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_PRECHARGE_HEADER")]
public class NTRN_PRECHARGE_HEADER : IIdentifyEntity
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
	public string? CST_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SALE_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? REGI_SEQ { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? CHARGE_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PAY_TYPE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CHARGE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CHARGE_REM_AMT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    public string? ORG_SALE_NO { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? RTN_MSG_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? RTN_MSG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_CST_NO_SALE_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}