using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("POS_KEY_MANG")]
public class POS_KEY_MANG : IIdentifyEntity
{
    /// <summary>
    /// 본사코드
    /// </summary>
    [Comment("본사코드")]
    [Key, Column(Order = 1)]
    [Required]
    public string? HD_SHOP_CODE { get; set; }
    /// <summary>
    /// 매장코드
    /// </summary>
    [Comment("매장코드")]
    [Key, Column(Order = 2)]
    [Required]
    public string? SHOP_CODE { get; set; }
    /// <summary>
    /// 포스번호
    /// </summary>
    [Comment("포스번호")]
    [Key, Column(Order = 3)]
    [Required]
    public string? POS_NO { get; set; }
    /// <summary>
    /// 라이센스키
    /// </summary>
    [Comment("라이센스")]
    public string? LICENSE_ID { get; set; }
    /// <summary>
    /// 라이센스키
    /// </summary>
    [Comment("라이센스키")]
    public string? LICENSE_KEY { get; set; }
    /// <summary>
    /// 인증토큰
    /// </summary>
    [Comment("인증토큰")]
    public string? TOKEN { get; set; }
    /// <summary>
    /// 토큰유효기간
    /// </summary>
    [Comment("토큰유효기간")]
    public string? VALIDDT { get; set; }

    public DateTime ExpiredDate
    {
        get
        {
            return DateTime.ParseExact(this.VALIDDT, "yyyyMMddHHmms", Thread.CurrentThread.CurrentUICulture);
        }
    }


    public string Base_PrimaryName()
    {
        return "HD_SHOP_CODE_SHOP_CODE_POS_NO";
    }

    public void EntityDefValue(EEditType eEdit, string userID, object data)
    {
    }

    public string Resource()
    {
        throw new NotImplementedException();
    }
}