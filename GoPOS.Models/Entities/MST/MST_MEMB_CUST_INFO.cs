using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_MEMB_CUST_INFO")]
public class MST_MEMB_CUST_INFO : IIdentifyEntity
{
	/// <summary>
	/// 회원관리주체-소속코드
	/// </summary>
	[Comment("회원관리주체-소속코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? CST_OGN_CODE { get; set; }
	/// <summary>
	/// 고객번호
	/// </summary>
	[Comment("고객번호")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 고객명-한글
	/// </summary>
	[Comment("고객명-한글")]
	public string? CST_NAME { get; set; }
	/// <summary>
	/// 고객명-영문
	/// </summary>
	[Comment("고객명-영문")]
	public string? CST_NAME_ENG { get; set; }
	/// <summary>
	/// 고객등급분류코드
	/// </summary>
	[Comment("고객등급분류코드")]
	public string? CST_CLASS_CODE { get; set; }
	/// <summary>
	/// 등록매장코드
	/// </summary>
	[Comment("등록매장코드")]
	public string? INSERT_SHOP_CODE { get; set; }
	/// <summary>
	/// 고객카드번호
	/// </summary>
	[Comment("고객카드번호")]
	public string? CST_CARD_NO { get; set; }
	/// <summary>
	/// 우편번호
	/// </summary>
	[Comment("우편번호")]
	public string? POST_NO { get; set; }
	/// <summary>
	/// 주소
	/// </summary>
	[Comment("주소")]
	public string? ADDR { get; set; }
	/// <summary>
	/// 주소상세
	/// </summary>
	[Comment("주소상세")]
	public string? ADDR_DTL { get; set; }
	/// <summary>
	/// 주민번호
	/// </summary>
	[Comment("주민번호")]
	public string? RESI_NO { get; set; }
	/// <summary>
	/// 생일자-월일년
	/// </summary>
	[Comment("생일자-월일년")]
	public string? BIRTH_DATE { get; set; }
	/// <summary>
	/// 양력여부
	/// </summary>
	[Comment("양력여부")]
	public string? SOLAR_YN { get; set; }
	/// <summary>
	/// 성별구분
	/// </summary>
	[Comment("성별구분")]
	public string? SEX_FLAG { get; set; }
	/// <summary>
	/// 이메일주소
	/// </summary>
	[Comment("이메일주소")]
	public string? EMAIL_ADDR { get; set; }
	/// <summary>
	/// 휴대폰번호
	/// </summary>
	[Comment("휴대폰번호")]
	public string? HP_NO { get; set; }
	/// <summary>
	/// 전화번호
	/// </summary>
	[Comment("전화번호")]
	public string? TEL_NO { get; set; }
	/// <summary>
	/// 단축전화번호(뒷자리4자리)
	/// </summary>
	[Comment("단축전화번호(뒷자리4자리)")]
	public string? S_TEL_NO { get; set; }
	/// <summary>
	/// 회원-현금영수증인식번호
	/// </summary>
	[Comment("회원-현금영수증인식번호")]
	public string? CSH_IDT_NO { get; set; }
	/// <summary>
	/// 결혼여부
	/// </summary>
	[Comment("결혼여부")]
	public string? WEDDING_YN { get; set; }
	/// <summary>
	/// 결혼일자-월일년
	/// </summary>
	[Comment("결혼일자-월일년")]
	public string? WEDDING_DATE { get; set; }
	/// <summary>
	/// 이메일수신여부
	/// </summary>
	[Comment("이메일수신여부")]
	public string? EMAIL_RECV_YN { get; set; }
	/// <summary>
	/// SMS수신여부
	/// </summary>
	[Comment("SMS수신여부")]
	public string? SMS_RECV_YN { get; set; }
	/// <summary>
	/// DM수신여부
	/// </summary>
	[Comment("DM수신여부")]
	public string? DM_RECV_YN { get; set; }
	/// <summary>
	/// 임시고객등록여부
	/// </summary>
	[Comment("임시고객등록여부")]
	public string? TMP_INSERT_YN { get; set; }
	/// <summary>
	/// 고객카드사용구분
	/// </summary>
	[Comment("고객카드사용구분")]
	public string? CST_CARD_USE_FLAG { get; set; }
	/// <summary>
	/// 고객카드발급횟수
	/// </summary>
	[Comment("고객카드발급횟수")]
	public Int16 CST_CARD_ISS_CNT { get; set; }
	/// <summary>
	/// 원천고객카드번호
	/// </summary>
	[Comment("원천고객카드번호")]
	public string? ORG_CST_CARD_NO { get; set; }
	/// <summary>
	/// 고객비고
	/// </summary>
	[Comment("고객비고")]
	public string? CST_REMARK { get; set; }
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

	public string Base_PrimaryName()
	{
		return "CST_OGN_CODE_CST_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}