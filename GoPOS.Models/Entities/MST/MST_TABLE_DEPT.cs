using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_TABLE_DEPT")]
public class MST_TABLE_DEPT : IIdentifyEntity
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
    /// 테이블 형태구분 Table shape classification (1: square 2: round table 3: packaging 4: delivery 5: the smallest 6: wall)
    /// </summary>
    [Comment("테이블 형태구분 Table shape classification (1: square 2: round table 3: packaging 4: delivery 5: the smallest 6: wall)")]
    [Key, Column(Order = 2)]
    [Required]
    [JsonPropertyName("tableFormCode")]
    public string? TABLE_FLAG { get; set; }

    /// <summary>
    /// 속성코드
    /// </summary>
    [Comment("속성코드")]
    [Key, Column(Order = 3)]
    [Required]
    [JsonPropertyName("attrCode")]
    public string? PROPERTY_CODE { get; set; }

    /// <summary>
    /// 속성명
    /// </summary>
    [Comment("속성명")]
    [Key, Column(Order = 4)]
    [Required]
    [JsonPropertyName("attrNm")]
    public string? PROPERTY_NAME { get; set; }

    /// <summary>
    /// 위치 X
    /// </summary>
    [Comment("위치 X")]
    [Key, Column(Order = 5)]
    [Required]
    [JsonPropertyName("attrXCoor")]
    public Int16? X { get; set; }

    /// <summary>
    /// 위치Y
    /// </summary>
    [Comment("위치Y")]
    [Key, Column(Order = 6)]
    [Required]
    [JsonPropertyName("attrYCoor")]
    public Int16? Y { get; set; }

    /// <summary>
    /// 폭
    /// </summary>
    [Comment("폭")]
    [Key, Column(Order = 7)]
    [Required]
    [JsonPropertyName("fontWidth")]
    public Int16? WIDTH { get; set; } = 0;

    /// <summary>
    /// 높이
    /// </summary>
    [Comment("높이")]
    [Key, Column(Order = 8)]
    [Required]
    [JsonPropertyName("fontHeight")]
    public Int16? HEIGHT { get; set; }=0;

    /// <summary>
    /// 텍스트 정렬구분 코드 "1:좌측상단 2:상단 3:우측상단 4:좌측 5:중앙 6:우측 7:좌측하단 8:하단 9:우측하단
	/// 1: Top left 2: Top 3: Top right 4: Left 5: Center 6: Right 7: Bottom left 8: Bottom 9: Bottom right"
    /// </summary>
    [Comment("텍스트 정렬구분 코드 1:좌측상단 2:상단 3:우측상단 4:좌측 5:중앙 6:우측 7:좌측하단 8:하단 9:우측하단\r\n1: Top left 2: Top 3: Top right 4: Left 5: Center 6: Right 7: Bottom left 8: Bottom 9: Bottom right")]
    [Key, Column(Order = 9)]
    [Required]
    [JsonPropertyName("txtSortCode")]
    public string? TEXTALIGN_FLAG { get; set; }

    /// <summary>
    /// 폰트명(굴림) "후불제 첫 OPEN시에는 폰트명(굴림) 지정해서 가야 한다.
    /// 추 후 폰트 종류를 Code로 변경 해야 한다.
    /// When opening the postpaid system for the first time, must specify the font name (굴림).
	/// Need to change the font type to Code later."
    /// </summary>
    [Comment("\"후불제 첫 OPEN시에는 폰트명(굴림) 지정해서 가야 한다.\r\n추 후 폰트 종류를 Code로 변경 해야 한다.\r\nWhen opening the postpaid system for the first time, must specify the font name (굴림).\r\nNeed to change the font type to Code later.\"")]
    [Key, Column(Order = 10)]
    [Required]
    [JsonPropertyName("fontCode")]
    public string? FONT { get; set; }

    /// <summary>
    /// 글자크기
    /// </summary>
    [Comment("글자크기")]
    [Key, Column(Order = 11)]
    [Required]
    [JsonPropertyName("fontSize")]
    public Int16? FONT_SIZE { get; set; }

    /// <summary>
    /// 글자형태 "0:일반 1:일반기울림 2:일반기울림밑줄 3:일반밑줄 4:굵게 5:굵게기울림
	/// 6:굴게기울기밑줄 7: 굵게밑줄
	/// 0: Normal 1: Normal italics 2: Normal italics Underline 3: Normal underline 4: Bold 5: Bold italics 6: Slanted underline 7: Bold underline"
    /// </summary>
    [Comment("글자형태 \"0:일반 1:일반기울림 2:일반기울림밑줄 3:일반밑줄 4:굵게 5:굵게기울림\r\n6:굴게기울기밑줄 7: 굵게밑줄\r\n0: Normal 1: Normal italics 2: Normal italics Underline 3: Normal underline 4: Bold 5: Bold italics 6: Slanted underline 7: Bold underline\"")]
    [Key, Column(Order = 12)]
    [Required]
    [JsonPropertyName("fontStyleCode")]
    public string? FONT_STYLE_FLAG { get; set; }

    /// <summary>
    /// 글자색(RGB) (255, 255, 255)
    /// </summary>
    [Comment("글자색(RGB) (255, 255, 255)")]
    [Key, Column(Order = 13)]
    [Required]
    [JsonPropertyName("fontColorCode")]
    public string? FONT_COLOR { get; set; }

    /// <summary>
    /// 사용여부
    /// </summary>
    [Comment("사용여부")]
    [Key, Column(Order = 14)]
    [Required]
    [JsonPropertyName("useAt")]
    public string? USE_YN { get; set; }

    /// <summary>
    /// 사용여부 Menu display switched YN (Y: possible, N: impossible)
    /// </summary>
    [Comment("사용여부 Menu display switched YN (Y: possible, N: impossible)")]
    [Key, Column(Order = 15)]
    [Required]
    [JsonPropertyName("menuAt")]
    public string? MENU_YN { get; set; }

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

    public string Base_PrimaryName()
    {
        return "SHOP_CODE_TABLE_CODE";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        return "/client/master/table/attribute";
    }
}