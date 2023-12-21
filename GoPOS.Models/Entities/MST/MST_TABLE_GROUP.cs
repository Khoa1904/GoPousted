using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_TABLE_GROUP")]
public class MST_TABLE_GROUP : IIdentifyEntity
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
    /// 테이블그룹코드
    /// </summary>
    [Comment("테이블그룹코드")]
    [Key, Column(Order = 2)]
    [Required]
    [JsonPropertyName("tableGrpCode")]
    public string? TG_CODE { get; set; }

    /// <summary>
    /// 테이블그룹명
    /// </summary>
    [Comment("테이블그룹명")]
    [Key, Column(Order = 3)]
    [Required]
    [JsonPropertyName("tableGrpNm")]
    public string? TG_NAME { get; set; }

    /// <summary>
    /// 그룹구분 (1:일반 2:포장 3:배달) 코드(200)
    /// </summary>
    [Comment("그룹구분 (1:일반 2:포장 3:배달) 코드(200)")]
    [Key, Column(Order = 4)]
    [Required]
    [JsonPropertyName("tableGrpSeCode")]
    public string? TG_FLAG { get; set; }

    /// <summary>
    /// 배경이미지
    /// </summary>
    [Comment("배경이미지")]
    [Key, Column(Order = 5)]
    [JsonPropertyName("tableGrpImg")]
    public string TG_BGIMAGE { get; set; }

    /// <summary>
    /// 정렬
    /// </summary>
    [Comment("정렬")]
    [Key, Column(Order = 6)]
    [Required]
    [JsonPropertyName("tableGrpSeq")]
    public Int16? TG_SORT { get; set; }

    /// <summary>
    /// 터치분류코드
    /// </summary>
    [Comment("터치분류코드")]
    [Key, Column(Order = 7)]
    [JsonPropertyName("tchkeyGrpCode")]
    public string TU_CLASS_CODE { get; set; }

    /// <summary>
    /// 사용여부 Menu display switched YN (Y: possible, N: impossible)
    /// </summary>
    [Comment("사용여부 Menu display switched YN (Y: possible, N: impossible)")]
    [Key, Column(Order = 8)]
    [Required]
    [JsonPropertyName("menuAt")]
    public string? MENU_YN { get; set; }

    /// <summary>
    /// 사용여부
    /// </summary>
    [Comment("사용여부")]
    [Key, Column(Order = 9)]
    [Required]
    [JsonPropertyName("useAt")]
    public string? USE_YN { get; set; }

    /// <summary>
    /// 등록일
    /// </summary>
    [Comment("등록일")]
    [Key, Column(Order = 10)]
    [JsonPropertyName("createdAt")]
    public string INSERT_DT { get; set; }

    /// <summary>
    /// 수정일
    /// </summary>
    [Comment("수정일")]
    [Key, Column(Order = 11)]
    [JsonPropertyName("updatedAt")]
    public string UPDATE_DT { get; set; }


    public string Base_PrimaryName()
    {
        return "SHOP_CODE_TG_CODE";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        return "/client/master/table/group";
    }
}