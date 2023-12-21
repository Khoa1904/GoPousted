using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_SHOP_CORNER")]
public class MST_INFO_SHOP_CORNER : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 코너코드
	/// </summary>
	[Comment("코너코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CORNER_CODE { get; set; }
	/// <summary>
	/// 코너명
	/// </summary>
	[Comment("코너명")]
	public string? CORNER_NAME { get; set; }
	/// <summary>
	/// 대표자명
	/// </summary>
	[Comment("대표자명")]
	public string? OWNER_NAME { get; set; }
	/// <summary>
	/// 사업자번호
	/// </summary>
	[Comment("사업자번호")]
	public string? BIZ_NO { get; set; }
	/// <summary>
	/// 전화번호
	/// </summary>
	[Comment("전화번호")]
	public string? TEL_NO { get; set; }
	/// <summary>
	/// 현금임대수수료율
	/// </summary>
	[Comment("현금임대수수료율")]
	public int CSH_FEE_RATE { get; set; }
	/// <summary>
	/// 카드임대수수료율
	/// </summary>
	[Comment("카드임대수수료율")]
	public int CRD_FEE_RATE { get; set; }
	/// <summary>
	/// 기타임대수수료율
	/// </summary>
	[Comment("기타임대수수료율")]
	public int ETC_FEE_RATE { get; set; }
	/// <summary>
	/// 신용카드-밴사코드
	/// </summary>
	[Comment("신용카드-밴사코드")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 신용카드-밴사계약번호
	/// </summary>
	[Comment("신용카드-밴사계약번호")]
	public string? VAN_TERM_NO { get; set; }
	/// <summary>
	/// 현금영수-밴사코드
	/// </summary>
	[Comment("현금영수-밴사코드")]
	public string? CASH_VAN_CODE { get; set; }
	/// <summary>
	/// 현금영수-밴사계약번호
	/// </summary>
	[Comment("현금영수-밴사계약번호")]
	public string? CASH_VAN_TERM_NO { get; set; }
	/// <summary>
	/// 사용여부
	/// </summary>
	[Comment("사용여부")]
	public string? USE_YN { get; set; }
	/// <summary>
	/// 최초등록일시
	/// </summary>
	[Comment("최초등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 최종수정일시
	/// </summary>
	[Comment("최종수정일시")]
	public string? UPDATE_DT { get; set; }
	/// <summary>
	/// 신용카드-KOCES 용 SERIAL_NO
	/// </summary>
	[Comment("신용카드-KOCES 용 SERIAL_NO")]
	public string? VAN_SER_NO { get; set; }
	/// <summary>
	/// 현금영수-KOCES 용 SERIAL_NO
	/// </summary>
	[Comment("현금영수-KOCES 용 SERIAL_NO")]
	public string? CASH_VAN_SER_NO { get; set; }
	/// <summary>
	/// 인증여부
	/// </summary>
	[Comment("인증여부")]
	public string? VAN_CERT_YN { get; set; }
	/// <summary>
	/// 최초인증일시
	/// </summary>
	[Comment("최초인증일시")]
	public string? VAN_CERT_SDT { get; set; }
	/// <summary>
	/// 최종인증일시
	/// </summary>
	[Comment("최종인증일시")]
	public string? VAN_CERT_EDT { get; set; }
	/// <summary>
	/// 인증횟수
	/// </summary>
	[Comment("인증횟수")]
	public int VAN_CERT_CNT { get; set; }
	/// <summary>
	/// 서버송신구분
	/// </summary>
	[Comment("서버송신구분")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 서버송신일시
	/// </summary>
	[Comment("서버송신일시")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// Work Index Key
	/// </summary>
	[Comment("Work Index Key")]
	public string? WORK_INDEX { get; set; }
	/// <summary>
	/// Work Key
	/// </summary>
	[Comment("Work Key")]
	public string? WORK_KEY { get; set; }
	/// <summary>
	/// OK-CASHBAG 가맹점번호
	/// </summary>
	[Comment("OK-CASHBAG 가맹점번호")]
	public string? OKCBG_TERM_NO { get; set; }
	/// <summary>
	/// OK-CASHBAG 가맹점비밀번호
	/// </summary>
	[Comment("OK-CASHBAG 가맹점비밀번호")]
	public string? OKCBG_SER_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? W_KEY { get; set; }
	/// <summary>
	/// 다음터미널번호
	/// </summary>
	[Comment("다음터미널번호")]
	public string? DAUMMSP_TERM_NO { get; set; }
	/// <summary>
	/// 
	/// </summary>
	[Comment("")]
	public string? DAUMMSP_SER_NO { get; set; }
	/// <summary>
	/// 동글SAM-NO
	/// </summary>
	[Comment("동글SAM-NO")]
	public string? VAN_SAM_ID { get; set; }
	/// <summary>
	/// 동글 단말기번호
	/// </summary>
	[Comment("동글 단말기번호")]
	public string? VAN_SAM_NO { get; set; }
	/// <summary>
	/// 발행사정보수신유무
	/// </summary>
	[Comment("발행사정보수신유무")]
	public string? VAN_SAM_RECV_FLAG { get; set; }
	/// <summary>
	/// 모바일 쿠폰 터미널번호
	/// </summary>
	[Comment("모바일 쿠폰 터미널번호")]
	public string? MCP_TERM_NO { get; set; }
	/// <summary>
	/// 모바일 쿠폰 SERIAL 번호
	/// </summary>
	[Comment("모바일 쿠폰 SERIAL 번호")]
	public string? MCP_SER_NO { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_CORNER_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}