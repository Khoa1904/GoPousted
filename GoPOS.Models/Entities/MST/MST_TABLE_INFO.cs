using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_TABLE_INFO")]
public class MST_TABLE_INFO : IIdentifyEntity
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
    /// 테이블코드
    /// </summary>
    [Comment("테이블코드")]
    [Key, Column(Order = 2)]
    [Required]
    [JsonPropertyName("tableCode")]
    public string? TABLE_CODE { get; set; }

    /// <summary>
    /// 테이블명
    /// </summary>
    [Comment("테이블명")]
    [Key, Column(Order = 3)]
    [Required]
    [JsonPropertyName("tableNm")]
    public string? TABLE_NAME { get; set; }

    /// <summary>
    /// 테이블그룹코드
    /// </summary>
    [Comment("테이블그룹코드")]
    [Key, Column(Order = 4)]
    [Required]
    [JsonPropertyName("tableGrpCode")]
    public string? TG_CODE { get; set; }

    /// <summary>
    /// 단체코드
    /// </summary>
    [Comment("단체코드")]
    [Key, Column(Order = 5)]
    [JsonPropertyName("tablePartyCode")]
    public string? GROUP_CODE { get; set; }

    /// <summary>
    /// 테이블 자석수
    /// </summary>
    [Comment("테이블 자석수")]
    [Key, Column(Order = 6)]
    [Required]
    [JsonPropertyName("tableSeatCnt")]
    public Int32? SEAT_NUM { get; set; } = 0;

    /// <summary>t
    /// 위치X
    /// </summary>
    [Comment("위치X")]
    [Key, Column(Order = 7)]
    [Required]
    [JsonPropertyName("tableXCoor")]
    public Int16 X { get; set; } = 0;
    /// <summary>
    /// 위치Y
    /// </summary>
    [Comment("위치Y")]
    [Key, Column(Order = 8)]
    [Required]
    [JsonPropertyName("tableYCoor")]
    public Int16 Y { get; set; } = 0;
    /// <summary>
    /// 폭
    /// </summary>
    [Comment("폭")]
    [Key, Column(Order = 9)]
    [Required]
    [JsonPropertyName("tableWidth")]
    public Int16 WIDTH { get; set; } = 0;
    /// <summary>
    /// 높이
    /// </summary>
    [Comment("높이")]
    [Key, Column(Order = 10)]
    [Required]
    [JsonPropertyName("tableHeight")]
    public Int16 HEIGHT { get; set; } = 0;

    /// <summary>
    /// 테이블 모양 (1: square 2: round table 3. wall) Code (203)
    /// </summary>
    [Comment("테이블 모양 (1: square 2: round table 3. wall) Code (203)")]
    [Key, Column(Order = 11)]
    [Required]
    [JsonPropertyName("tableShapeCode")]
    public string? SHAPE_FLAG { get; set; }

    /// <summary>
    /// 테이블 형태구분 (1.사각 2.원탁 3:포장 4:배달 5.최소) 코드(201)
    /// </summary>
    //[Comment("테이블 형태구분 (1.사각 2.원탁 3:포장 4:배달 5.최소) 코드(201)")]
    //[Key, Column(Order = 12)]
    //[JsonPropertyName("tableFormCode")]
    //public string TABLE_FLAG { get; set; }

    /// <summary>
    /// 사용여부
    /// </summary>
    [Comment("사용여부")]
    [Key, Column(Order = 12)]
    [Required]
    [JsonPropertyName("useAt")]
    public string? USE_YN { get; set; }

    /// <summary>
    /// 테이블상태 (1:Basic 2:Use 3:Order 4:CID CALL)
    /// </summary>
    [Comment("테이블상태 (1:Basic 2:Use 3:Order 4:CID CALL)")]
    [Key, Column(Order = 13)]
    [Required]
    [JsonPropertyName("tableSttCode")]
    public string? STATUS_FLAG { get; set; }

    /// <summary>
    /// 테이블LOCK포스번호
    /// </summary>
    [Comment("테이블LOCK포스번호")]
    [Key, Column(Order = 14)]
    [JsonPropertyName("lockPosNo")]
    public string LOCK_POS_NO { get; set; }

    /// <summary>
    /// 테이블LOCK상태
    /// </summary>
    [Comment("테이블LOCK상태")]
    [Key, Column(Order = 15)]
    [JsonPropertyName("lockPosSttCode")]
    public string LOCK_FLAG { get; set; }

    /// <summary>
    /// 등록일
    /// </summary>
    [Comment("등록일")]
    [Key, Column(Order = 16)]
    [JsonPropertyName("createdAt")]
    public string INSERT_DT { get; set; }

    /// <summary>
    /// 수정일
    /// </summary>
    [Comment("수정일")]
    [Key, Column(Order = 17)]
    [JsonPropertyName("updatedAt")]
    public string UPDATE_DT { get; set; }

    /// <summary>
    /// CID-전화번호
    /// </summary>
    [Comment("CID-전화번호")]
    [Key, Column(Order = 18)]
    [JsonPropertyName("cidTelno")]
    public string CID_TEL_NO { get; set; }

    /// <summary>
    /// CID-라인번호
    /// </summary>
    [Comment("CID-라인번호")]
    [Key, Column(Order = 19)]
    [JsonPropertyName("cidLineNo")]
    public string CID_LINE_NO { get; set; }

    /// <summary>
    /// 신규고객구분 0:General 1:Existing customer 2:New customer
    /// </summary>
    [Comment("신규고객구분 0:General 1:Existing customer 2:New customer")]
    [Key, Column(Order = 20)]
    [Required]
    [JsonPropertyName("newCstCode")]
    public string? NEW_CST_FLAG { get; set; }

    /// <summary>
    /// 회원번호
    /// </summary>
    [Comment("회원번호")]
    [Key, Column(Order = 21)]
    [JsonPropertyName("mbrCode")]
    public string CST_NO { get; set; }

    /// <summary>
    /// 회원명
    /// </summary>
    [Comment("회원명")]
    [Key, Column(Order = 22)]
    [JsonPropertyName("koMbrNm")]
    public string CST_NAME { get; set; }

    /// <summary>
    /// 배달지-주소
    /// </summary>
    [Comment("배달지-주소")]
    [Key, Column(Order = 23)]
    [JsonPropertyName("dlvryAdres")]
    public string DLV_ADDR { get; set; }

    /// <summary>
    /// 배달지-상세주소
    /// </summary>
    [Comment("배달지-상세주소")]
    [Key, Column(Order = 24)]
    [JsonPropertyName("dlvryDetailAdres")]
    public string DLV_ADDR_DTL { get; set; }

    /// <summary>
    /// 배달처리 선택구분 0:General 1:Delivery selection
    /// </summary>
    [Comment("배달처리 선택구분 0:General 1:Delivery selection")]
    [Key, Column(Order = 25)]
    [Required]
    [JsonPropertyName("dlvryCode")]
    public Int16? DLV_PROC_FLAG { get; set; }

    /// <summary>
    /// 배달주문번호
    /// </summary>
    [Comment("배달주문번호")]
    [Key, Column(Order = 26)]
    [JsonPropertyName("dlvryOrderNo")]
    public string DLV_ORDER_NO { get; set; }

    /// <summary>
    /// 배달구역-대분류코드
    /// </summary>
    [Comment("배달구역-대분류코드")]
    [Key, Column(Order = 27)]
    [JsonPropertyName("dlvryMgmtGrpCode")]
    public string DLV_CL_CODE { get; set; }

    /// <summary>
    /// 배달구역-중분류코드
    /// </summary>
    [Comment("배달구역-중분류코드")]
    [Key, Column(Order = 28)]
    [JsonPropertyName("dlvryMgmtCode")]
    public string DLV_CM_CODE { get; set; }

    /// <summary>
    /// CID-수신시각
    /// </summary>
    [Comment("CID-수신시각")]
    [Key, Column(Order = 29)]
    [JsonPropertyName("cidRcvDt")]
    public string CID_CALL_DT { get; set; }

    /// <summary>
    /// 배달IF전송용주소
    /// </summary>
    [Comment("배달IF전송용주소")]
    [Key, Column(Order = 30)]
    [JsonPropertyName("dlvryIfAdres")]
    public string DLV_IF_ADDR { get; set; }

    /// <summary>
    /// 테이블 색상코드 RGB - 255,255,255
    /// </summary>
    [Comment("테이블 색상코드 RGB - 255,255,255")]
    [Key, Column(Order = 31)]
    [JsonPropertyName("tableColorCode")]
    public string TABLE_COLOR_CODE { get; set; }

    public string Base_PrimaryName()
    {
        return "SHOP_CODE_TABLE_FLAG_PROPERTY_CODE";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        return "/client/master/table/info";
    }
}

public class TABLE_THR 
{
    public string? SHOP_CODE { get; set; }
    public string? TABLE_FLAG { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double WIDTH { get; set; }
    public double HEIGHT { get; set; }
    public string? USE_YN { get; set; }
    public string? INSERT_DT { get; set; }
    public string? UPDATE_DT { get; set; }
    public string TABLE_CODE { get; set; }
    public string TABLE_NAME { get; set; }
    public string SHAPE_FLAG { get; set; }
    public string TG_CODE { get; set; }
    public List<ORDER_GRID_ITEM> ORDER_ITEMS { get; set;}
}