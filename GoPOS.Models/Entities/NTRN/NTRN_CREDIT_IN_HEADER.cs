using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("NTRN_CREDIT_IN_HEADER")]
public class NTRN_CREDIT_IN_HEADER : IIdentifyEntity
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
	/// 회원번호
	/// </summary>
	[Comment("회원번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? CST_NO { get; set; }
	/// <summary>
	/// 외상입금번호
	/// </summary>
	[Comment("외상입금번호")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SALE_NO { get; set; }
	/// <summary>
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 외상입금여부 Y:정상 N:취소
	/// </summary>
	[Comment("외상입금여부 Y:정상 N:취소")]
	public string? WES_IN_YN { get; set; }
	/// <summary>
	/// 결제수단구분 (CCD_CODEM_T : 038) 0:현금 1:신용카드
	/// </summary>
	[Comment("결제수단구분 (CCD_CODEM_T : 038) 0:현금 1:신용카드")]
	public string? PAY_TYPE_FLAG { get; set; }
	/// <summary>
	/// 계좌이체 입금구분 0:일반 1:계좌이체
	/// </summary>
	[Comment("계좌이체 입금구분 0:일반 1:계좌이체")]
	public string? BANK_IN_FLAG { get; set; }
	/// <summary>
	/// 외상입금금액
	/// </summary>
	[Comment("외상입금금액")]
	[Precision(12, 2)]
	public decimal WES_IN_AMT { get; set; } = 0;
	/// <summary>
	/// 잔액-외상입금전
	/// </summary>
	[Comment("잔액-외상입금전")]
	[Precision(12, 2)]
	public decimal WES_BF_REM_AMT { get; set; } = 0;
	/// <summary>
	/// 잔액-외상입금후
	/// </summary>
	[Comment("잔액-외상입금후")]
	[Precision(12, 2)]
	public decimal WES_AF_REM_AMT { get; set; } = 0;
	/// <summary>
	/// 외상입금비고
	/// </summary>
	[Comment("외상입금비고")]
	public string? REMARK { get; set; }
	/// <summary>
	/// 원천외상입금번호 영업일자+외상입금번호
	/// </summary>
	[Comment("원천외상입금번호 영업일자+외상입금번호")]
	public string? ORG_WES_IN_NO { get; set; }
	/// <summary>
	/// 외상구분(0:회원 1:여행사)
	/// </summary>
	[Comment("외상구분(0:회원 1:여행사)")]
	public string? WES_FLAG { get; set; }
	/// <summary>
	/// 판매원번호
	/// </summary>
	[Comment("판매원번호")]
	public string? EMP_NO { get; set; }
	/// <summary>
	/// 서버전송구분 0:미전송 1:전송 2:오류
	/// </summary>
	[Comment("서버전송구분 0:미전송 1:전송 2:오류")]
	public string? SEND_FLAG { get; set; }
	/// <summary>
	/// 서버전송일시
	/// </summary>
	[Comment("서버전송일시")]
	public string? SEND_DT { get; set; }
	/// <summary>
	/// 등록일시 System Date(8) + System Time(6)
	/// </summary>
	[Comment("등록일시 System Date(8) + System Time(6)")]
	public string? INSERT_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_CST_NO_SALE_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}