using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using GoPOS.Models.Config;

namespace GoPOS.Models;

[Table("TRN_POINTSAVE")]
public class TRN_POINTSAVE : IIdentifyEntity
{
    /// <summary>
    /// 매장코드
    /// </summary>
    [Comment("매장코드")]
    [Key, Column(Order = 1)]
    [Required]
    public string? SHOP_CODE { get; set; }
    /// <summary>
    /// 영업일자
    /// </summary>
    [Comment("영업일자")]
    [Key, Column(Order = 2)]
    [Required]
    public string? SALE_DATE { get; set; }
    /// <summary>
    /// 포스번호
    /// </summary>
    [Comment("포스번호")]
    [Key, Column(Order = 3)]
    [Required]
    public string? POS_NO { get; set; }
    /// <summary>
    /// 영수번호
    /// </summary>
    [Comment("영수번호")]
    [Key, Column(Order = 4)]
    [Required]
    public string? BILL_NO { get; set; }
    /// <summary>
    /// 순번
    /// </summary>
    [Comment("순번")]
    [Key, Column(Order = 5)]
    [Required]
    public string? SEQ_NO { get; set; }
    /// <summary>
    /// 정산차수
    /// </summary>
    [Comment("정산차수")]
    public string? REGI_SEQ { get; set; }
    /// <summary>
    /// 판매여부 Y:정상 N:반품(취소)
    /// </summary>
    [Comment("판매여부 Y:정상 N:반품(취소)")]
    public string? SALE_YN { get; set; }

    public string Base_PrimaryName()
    {
        return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO_SEQ_NO";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 회원번호
    /// </summary>
    public string CST_NO { get; set; } = string.Empty;

    /// <summary>
    /// 카드번호
    /// </summary>
    public string CARD_NO { get; set; } = string.Empty;

    /// <summary>
    /// 등급
    /// </summary>
    public string LEVEL { get; set; } = string.Empty;

    /// <summary>
    /// 총매출액
    /// </summary>
    public decimal TOT_SALE_AMT { get; set; } = 0;

    /// <summary>
    /// 총할인액
    /// </summary>
    public decimal TOT_DC_AMT { get; set; } = 0;

    /// <summary>
    /// 적립대상총액/적립 스탬프 수량
    /// </summary>
    public decimal SAVE_AMT { get; set; } = 0;

    /// <summary>
    /// 적립제외금액/적립제외 스탬프 수량
    /// </summary>
    public decimal NO_SAVE_AMT { get; set; } = 0;

    /// <summary>
    /// 누적 포인트 / 누적 스탬프
    /// </summary>
    public decimal TOT_PNT { get; set; } = 0;

    /// <summary>
    /// 누적 사용 포인트 / 누적 사용 스탬프
    /// </summary>
    public decimal TOT_USE_PNT { get; set; } = 0;

    /// <summary>
    /// 최종 포인트 / 최종 스탬프
    /// </summary>
    public decimal LAST_PNT { get; set; } = 0;

    /// <summary>
    /// 직전 포인트 / 직전 스탬프
    /// </summary>
    public decimal PRE_PNT { get; set; } = 0;

    /// <summary>
    /// 적립 포인트 / 적립 스탬프
    /// </summary>
    public decimal SAVE_PNT { get; set; } = 0;

    /// <summary>
    /// 사용포인트 구분
    /// </summary>
    public string USE_TYPE { get; set; }

    /// <summary>
    /// 등록일시
    /// </summary>
    public string INSERT_DT { get; set; } = string.Empty;





















}