using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System;

namespace GoPOS.Models;

[Table("TRN_PRDT")]
public class TRN_PRDT : IIdentifyEntity
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
	public string? PRD_CODE { get; set; }

	[NotMapped]
    public string? PRD_NAME { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? PRD_TYPE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CORNER_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
    [Precision(12, 0)]
    public int SALE_QTY { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal SALE_UPRC { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal ETC_AMT { get; set; } = 0;
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
	[Precision(12, 2)]
	public decimal DCM_SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CHG_BILL_NO { get; set; }
    /// <summary>
	/// 과세여부
	/// N: 면세상품
	/// </summary>
	[Comment("과세여부")]
    public string? TAX_YN { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? DLV_PACK_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SDS_CLASS_CODE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? SDS_PARENT_CODE { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[NotMapped]
    public string? SIDE_MENU_YN { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_GEN { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_SVC { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_JCD { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_CPN { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_CST { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_FOD { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_PRM { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_CRD { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_AMT_PACK { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    public string? TK_CPN_CODE { get; set; }
    /// <summary>
    /// REMARK
    /// </summary>
    [Comment("Remark")]
    public string? REMARK { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public string? INSERT_DT { get; set; }
	

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
    public override string ToString()
    {
        return SHOP_CODE + "-"+ POS_NO + "-" + BILL_NO + "-" + SEQ_NO;
    }
}