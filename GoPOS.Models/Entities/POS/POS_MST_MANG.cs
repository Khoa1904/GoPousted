using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("POS_MST_MANG")]
public class POS_MST_MANG : IIdentifyEntity
{
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 1)]
	[Required]
	public string? RECV_SEQ { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Key, Column(Order = 2)]
	[Required]
	public string? MST_ID { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? MST_TPNAME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? MST_TLNAME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(10, 0)]
	public decimal DATA_COUNT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? LASTVERSION { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "RECV_SEQ_MST_ID";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}