using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;

namespace GoPOS.Models;

[Table("MST_INFO_VAN_POS")]
public class MST_INFO_VAN_POS : IIdentifyEntity
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
	/// 포스번호
	/// </summary>
	[Comment("포스번호")]
	[Key, Column(Order = 2)]
	[Required]
	[JsonPropertyName("posNo")]
	public string? POS_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? POS_REMARK { get; set; } = "";
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[DefaultValue("N")]
	public string? LOCAL_DB_YN { get; set; } = "N";
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	[DefaultValue("Y")]
	public string? USE_YN { get; set; } = "Y";
	/// <summary>
	/// 신용카드-승인일련번호
	/// </summary>
	[Comment("신용카드-승인일련번호")]
	[JsonPropertyName("crdtCardApproPid")]
	public string? CRD_VAN_SEQ { get; set; }
	/// <summary>
	/// 현금영수증-승인일련번호
	/// </summary>
	[Comment("현금영수증-승인일련번호")]
	[JsonPropertyName("cashRecptApproPid")]
	public string? CASH_VAN_SEQ { get; set; }
	/// <summary>
	/// 접속IP주소
	/// </summary>
	[Comment("접속IP주소")]
	[JsonPropertyName("posIp")]
	public string? IP_ADDR { get; set; }
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
	[JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }
	/// <summary>
	/// 밴사코드
	/// </summary>
	[Comment("밴사코드")]
	[JsonPropertyName("mgmtVanCode")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 사업자번호
	/// </summary>
	[Comment("사업자번호")]
	[JsonPropertyName("storeCorpno")]
	public string? BIZ_NO { get; set; }
	/// <summary>
	/// 단말기번호
	/// </summary>
	[Comment("단말기번호")]
	[JsonPropertyName("vanTermNo")]
	public string? VAN_TERM_NO { get; set; }
	/// <summary>
	/// 단말기시리얼번호
	/// </summary>
	[Comment("단말기시리얼번호")]
	[JsonPropertyName("vanSerialNo")]
	public string? VAN_SER_NO { get; set; }
	/// <summary>
	/// 인증여부
	/// </summary>
	[Comment("인증여부")]
	[JsonPropertyName("confrmAt")]
	public string? VAN_CERT_YN { get; set; }
	/// <summary>
	/// 최초인증일시
	/// </summary>
	[Comment("최초인증일시")]
	[JsonPropertyName("initConfrmDt")]
	public string? VAN_CERT_SDT { get; set; }
	/// <summary>
	/// 최종인증일시
	/// </summary>
	[Comment("최종인증일시")]
	[JsonPropertyName("lastConfrmDt")]
	public string? VAN_CERT_EDT { get; set; }
	/// <summary>
	/// 인증횟수
	/// </summary>
	[Comment("인증횟수")]
	[JsonPropertyName("confrmCnt")]
	public int VAN_CERT_CNT { get; set; }
	/// <summary>
	/// 서버송신구분
	/// </summary>
	[Comment("서버송신구분")]
	[JsonPropertyName("posSendSeCode")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 서버송신일시
	/// </summary>
	[Comment("서버송신일시")]
	[JsonPropertyName("posLastSendDt")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// 모바일 버전체크시 사용
	/// </summary>
	[Comment("모바일 버전체크시 사용")]
	[JsonPropertyName("posVer")]
	public string? POS_VER { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? W_KEY { get; set; }
	/// <summary>
	/// Work Index Key
	/// </summary>
	[Comment("Work Index Key")]
	[JsonPropertyName("workIndexKey")]
	public string? WORK_INDEX { get; set; }
	/// <summary>
	/// Work Key
	/// </summary>
	[Comment("Work Key")]
	[JsonPropertyName("workKey")]
	public string? WORK_KEY { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_POS_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/pos/van";
    }    
}