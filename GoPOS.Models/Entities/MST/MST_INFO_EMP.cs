using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace GoPOS.Models;

[Table("MST_INFO_EMP")]
public class MST_INFO_EMP : IIdentifyEntity, INotifyPropertyChanged
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
	/// 사원번호
	/// </summary>
	[Comment("사원번호")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("staffNo")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 사원명
	/// </summary>
	[Comment("사원명")]
    [JsonPropertyName("staffNm")]
	public string? EMP_NAME { get; set; }
	/// <summary>
	/// 사원비밀번호
	/// </summary>
	[Comment("사원비밀번호")]
    [JsonPropertyName("staffPosPw")]
	public string? EMP_PWD { get; set; }
	/// <summary>
	/// 사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달
	/// </summary>
	[Comment("사원구분 (CCD_CODEM_T : 008) 0:점주 1:판매원 2:주문 3:배달")]
    [JsonPropertyName("staffSeCode")]
	public string? EMP_FLAG { get; set; }
	/// <summary>
	/// 사원직책 (SCD_CODEM_T : 203) 매장별설정
	/// </summary>
	[Comment("사원직책 (SCD_CODEM_T : 203) 매장별설정")]
    [JsonPropertyName("positionCode")]
	public string? EMP_CLASS_CODE { get; set; }
	/// <summary>
	/// 웹사용여부
	/// </summary>
	[Comment("웹사용여부")]
    [JsonPropertyName("webAt")]
	public string? WEB_USE_YN { get; set; }
	/// <summary>
	/// 웹사용자ID
	/// </summary>
	[Comment("웹사용자ID")]
    [JsonPropertyName("staffId")]
	public string? USER_ID { get; set; }
	/// <summary>
	/// 웹사용자비밀번호
	/// </summary>
	[Comment("웹사용자비밀번호")]
    [JsonPropertyName("staffPw")]
	public string? USER_PWD { get; set; }
	/// <summary>
	/// 포스팅가능여부
	/// </summary>
	[Comment("포스팅가능여부")]
    [JsonPropertyName("postingAt")]
	public string? POSTING_YN { get; set; }
	/// <summary>
	/// 주문가능여부
	/// </summary>
	[Comment("주문가능여부")]
    [JsonPropertyName("orderAt")]
	public string? ORDER_FLAG { get; set; }
	/// <summary>
	/// 사원카드번호
	/// </summary>
	[Comment("사원카드번호")]
    [JsonPropertyName("staffCardNo")]
	public string? EMP_CARD_NO { get; set; }
	/// <summary>
	/// 전화번호
	/// </summary>
	[Comment("전화번호")]
    [JsonPropertyName("staffTelNo")]
	public string? TEL_NO { get; set; }
	/// <summary>
	/// 우편번호
	/// </summary>
	[Comment("우편번호")]
    [JsonPropertyName("postNo")]
	public string? POST_NO { get; set; }
	/// <summary>
	/// 주소
	/// </summary>
	[Comment("주소")]
    [JsonPropertyName("roadnmAdres")]
	public string? ADDR { get; set; }
	/// <summary>
	/// 주소상세
	/// </summary>
	[Comment("주소상세")]
    [JsonPropertyName("detailAdres")]
	public string? ADDR_DTL { get; set; }
	/// <summary>
	/// 퇴직구분 (CCD_CODEM_T : 009 ) 0:재직 1:퇴직 2:휴직
	/// </summary>
	[Comment("퇴직구분 (CCD_CODEM_T : 009 ) 0:재직 1:퇴직 2:휴직")]
    [JsonPropertyName("retireSeCode")]
	public string? RETIRE_FLAG { get; set; }
	/// <summary>
	/// SMS수신여부
	/// </summary>
	[Comment("SMS수신여부")]
    [JsonPropertyName("smsAt")]
	public string? SMS_RECV_YN { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
    [JsonPropertyName("useAt")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
    [JsonPropertyName("createdAt")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
    [JsonPropertyName("updatedat")]
	public string? UPDATE_DT { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Base_PrimaryName()
	{
		return "SHOP_CODE_EMP_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/staff/store";

    }    
}