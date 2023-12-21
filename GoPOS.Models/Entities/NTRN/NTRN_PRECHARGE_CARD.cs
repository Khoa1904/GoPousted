using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_PRECHARGE_CARD")]
public class NTRN_PRECHARGE_CARD : IIdentifyEntity
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
	[Key, Column(Order = 6)]
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
	public string? CHARGE_YN { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CRD_CARD_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? VALID_TERM { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal APPR_REQ_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal APPR_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal APPR_DC_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal SVC_TIP_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal VAT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INST_MM_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public Int16 INST_MM_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SIGN_PAD_YN { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CARD_IN_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? APPR_FLAG { get; set; }
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
	public string? APPR_MSG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CRDCP_CODE { get; set; }
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
	public string? VAN_TERM_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? VAN_SLIP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CRDCP_TERM_NO { get; set; }
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
	public string? APPR_LOG_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_CST_NO_SALE_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}