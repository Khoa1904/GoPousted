using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("ORD_EASYPAY")]
public class ORD_EASYPAY : IIdentifyEntity
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
	public string? ORDER_NO { get; set; }
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
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PAY_CARD_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal APPR_REQ_AMT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INST_MM_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public Int16 INST_MM_CNT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PAY_METHOD_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? APPR_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? APPR_TIME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? APPR_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PAYCP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? VAN_PAYCP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? ISS_CRDCP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? ISS_CRDCP_NAME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PUR_CRDCP_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? PUR_CRDCP_NAME { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CRDCP_TERM_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? APPR_MSG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? ORG_APPR_DATE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? ORG_APPR_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_ORDER_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}