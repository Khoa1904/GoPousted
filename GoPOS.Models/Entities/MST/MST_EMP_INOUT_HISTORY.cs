using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_EMP_INOUT_HISTORY")]
public class MST_EMP_INOUT_HISTORY : IIdentifyEntity
{
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? EMP_IO_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key]
	[Required]
	public string? EMP_IO_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? EMP_NAME { get; set; }
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
	public string? EMP_IO_REMARK { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_POS_NO_SALE_DATE_EMP_NO_EMP_IO_DT_EMP_IO_FLAG";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}