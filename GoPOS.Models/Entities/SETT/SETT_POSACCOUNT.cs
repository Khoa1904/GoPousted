using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("SETT_POSACCOUNT")]
public class SETT_POSACCOUNT : IIdentifyEntity
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
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CLOSE_FLAG { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? OPEN_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? CLOSE_DT { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int TOT_BILL_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TOT_SALE_AMT { get; set; } = 0; //nail  (gross sale)
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TOT_DC_AMT { get; set; } = 0;
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
	public decimal TOT_ETC_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DCM_SALE_AMT { get; set; } = 0; //nail  (net sale)
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal VAT_SALE_AMT { get; set; } = 0;
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
	public decimal NO_VAT_SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal NO_TAX_SALE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int RET_BILL_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal RET_BILL_AMT { get; set; } = 0;         //nailed
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int VISIT_CST_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal POS_READY_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal POS_CSH_IN_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal POS_CSH_OUT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_GEN_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_GEN_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_SVC_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_SVC_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_JCD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_JCD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_CPN_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_CPN_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_CST_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_CST_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_TFD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_TFD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_PRM_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_PRM_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_CRD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DC_PACK_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_PACK_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal WEA_IN_CSH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal WEA_IN_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TK_GFT_SALE_CSH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TK_GFT_SALE_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TK_FOD_SALE_CSH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TK_FOD_SALE_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal PPC_CARD_SALE_CSH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal PPC_CARD_SALE_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal PRE_PNT_SALE_CSH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal PRE_PNT_SALE_CRD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CASH_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CASH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int CASH_BILL_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CASH_BILL_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int CRD_CARD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CRD_CARD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int UIONPAY_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal UIONPAY_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int SP_PAY_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal SP_PAY_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int JCD_CARD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal JCD_CARD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int TK_GFT_CNT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    [Precision(12, 2)]
    public decimal TK_GFT_UAMT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	[Precision(12, 2)]
	public decimal TK_GFT_AMT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public int TK_FOD_CNT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
    [Precision(12, 2)]
    public decimal TK_FOD_UAMT { get; set; } = 0;
		/// <summary>
    /// 
    /// </summary>
    [Comment("")]
	[Precision(12, 2)]
	public decimal TK_FOD_AMT { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    [Comment("")]
	public int WES_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal WES_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int PCD_CARD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal PCD_CARD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public decimal PPC_CARD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public int PPC_CARD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int EGIFT_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal EGIFT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int MCP_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal MCP_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int CST_POINTSAVE_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CST_POINTSAVE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int CST_POINTUSE_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal CST_POINTUSE_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int RFC_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal RFC_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int DEPOSIT_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DEPOSIT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REFUND_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REFUND_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_CHECK_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REM_CHECK_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W100000_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W50000_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W10000_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W5000_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W1000_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W500_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W100_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W50_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_W10_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REM_CASH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public decimal REM_TK_GFT_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REM_TK_GFT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REM_TK_FOD_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REM_TK_FOD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal ETC_TK_FOD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal LOSS_CASH_AMT { get; set; } = 0;
    /// <summary>
    /// 상품권과부족
    /// </summary>
    [Comment("상품권과부족")]
	[Precision(12, 2)]
	public decimal LOSS_TK_GFT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal LOSS_TK_FOD_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REPAY_CASH_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REPAY_CASH_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int REPAY_TK_GFT_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal REPAY_TK_GFT_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public int TAX_RFND_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TAX_RFND_AMT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal TAX_RFND_FEE { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_TAX_CNT { get; set; } = 0;
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[Precision(12, 2)]
	public decimal DC_TAX_AMT { get; set; } = 0;
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
		return "SHOP_CODE_SALE_DATE_POS_NO_REGI_SEQ";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}
}