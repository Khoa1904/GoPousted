using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("POS_STATUS")]
public class POS_STATUS : IIdentifyEntity
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
	public string? POS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CLOSE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? BILL_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? ORDER_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SALE_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CREDIT_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? EMP_NO { get; set; }

	public string Base_PrimaryName()
	{
		return "";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "";
	}    
}