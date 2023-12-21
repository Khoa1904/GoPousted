using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("POS_POST_MANG")]
public class POS_POST_MANG : IIdentifyEntity
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
	[Key, Column(Order = 3)]
	[Required]
	public string? SALE_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 4)]
	[Required]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 5)]
	[Required]
	public string? POST_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 6)]
	[Required]
	public string? BILL_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 7)]
	[Required]
	public string? BILL_NO { get; set; }
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
	public string? ERR_CODE { get; set; }
	/// <summary>
	/// 오류 메시지
	/// </summary>
	[Comment("오류 메시지")]
	public string? ERR_MSG { get; set; }
    /// <summary>
    /// send times
    /// </summary>
    [Comment("send times")]
	public int? ERR_CNT { get; set; } = 0;
    
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
public enum ESend_Flag { Yes ='Y', No = 'N', Error = 'E' }