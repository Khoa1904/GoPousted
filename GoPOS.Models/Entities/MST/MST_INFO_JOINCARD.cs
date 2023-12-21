using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_JOINCARD")]
public class MST_INFO_JOINCARD : IIdentifyEntity
{
    /// <summary>
    /// 매장코드
    /// </summary>
    [Comment("매장코드")]
    [Key, Column(Order = 1)]
    [Required]
    [JsonPropertyName("storeCode")]
    public string? SHOP_CODE { get; set; }
    /// <summary>
    /// 제휴카드코드
    /// </summary>
    [Comment("제휴카드코드")]
    [Key, Column(Order = 2)]
    [Required]
    [JsonPropertyName("partnerCode")]
    public string? JCD_CODE { get; set; }
    /// <summary>
    /// 제휴카드명
    /// </summary>
    [Comment("제휴카드명")]
    [JsonPropertyName("partnerNm")]
    public string? JCD_NAME { get; set; }
    /// <summary>
    /// 제휴카드-유형구분 (CCD_CODEM_T : 035) 1:OCBS 2:SKT 3:KTF 4:LGT 5:신용제휴 9:기타제휴
    /// </summary>
    [Comment("제휴카드-유형구분 (CCD_CODEM_T : 035) 1:OCBS 2:SKT 3:KTF 4:LGT 5:신용제휴 9:기타제휴")]
    [JsonPropertyName("partnerTypeCode")]
    public string? JCD_TYPE_FLAG { get; set; }
    /// <summary>
    /// 유효기간-시작일자
    /// </summary>
    [Comment("유효기간-시작일자")]
    [JsonPropertyName("startExpireDe")]
    public string? VALID_F_DATE { get; set; }
    /// <summary>
    /// 유효기간-종료일자
    /// </summary>
    [Comment("유효기간-종료일자")]
    [JsonPropertyName("endExpireDe")]
    public string? VALID_T_DATE { get; set; }
    /// <summary>
    /// 할인상품구분 (0:전체상품, 1:특정상품)
    /// </summary>
    [Comment("할인상품구분 (0:전체상품, 1:특정상품)")]
    [JsonPropertyName("dscSeCode")]
    public string? DC_PRD_FLAG { get; set; }
    /// <summary>
    /// 할인율 (전체상품의 경우 세팅)
    /// </summary>
    [Comment("할인율 (전체상품의 경우 세팅)")]
    [JsonPropertyName("dscRt")]
    public Int16 DC_RATE { get; set; }
    /// <summary>
    /// 할인한도구분 (0:한도없음, 1:한도설정)
    /// </summary>
    [Comment("할인한도구분 (0:한도없음, 1:한도설정)")]
    [JsonPropertyName("dscLmtSeCode")]
    public string? DC_LIMIT_FLAG { get; set; }
    /// <summary>
    /// 할인한도금액 (한도설정의 경우 세팅)
    /// </summary>
    [Comment("할인한도금액 (한도설정의 경우 세팅)")]
    [JsonPropertyName("dscLmtPrice")]
    public int DC_LIMIT_AMT { get; set; }

    [Comment("할인한도금액 (한도설정의 경우 세팅)")]
    [NotMapped]
    public int DC_LIMIT_AMT_APPLY
    {
        get
        {
            return "0".Equals(DC_LIMIT_FLAG) ? 0 : DC_LIMIT_AMT;
        }
    }
    /// <summary>
    /// 승인처리구분 (0:승인안함, 1:승인처리)
    /// </summary>
    [Comment("승인처리구분 (0:승인안함, 1:승인처리)")]
    [JsonPropertyName("approSeCode")]
    public string? APPR_PROC_FLAG { get; set; }

    [NotMapped]
    public string? APPR_PROC_FLAG_NAME
    {
        get
        {
            return "0".Equals(APPR_PROC_FLAG) ? "승인안함" : "승인처리";
        }
    }
    /// <summary>
    /// 비고
    /// </summary>
    [Comment("비고")]
    [JsonPropertyName("partnerNote")]
    public string? REMARK { get; set; }
    /// <summary>
    /// 등록일시
    /// </summary>
    [Comment("등록일시")]
    [JsonPropertyName("createdAt")]
    public string? INSERT_DT { get; set; }
    /// <summary>
    /// 수정일시
    /// </summary>
    [Comment("수정일시")]
    [JsonPropertyName("updatedAt")]
    public string? UPDATE_DT { get; set; }

    public string Base_PrimaryName()
    {
        return "SHOP_CODE_JCD_CODE";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        return "/client/master/payment/partner-card";
    }
}