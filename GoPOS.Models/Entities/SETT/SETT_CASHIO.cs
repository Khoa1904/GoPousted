using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("SETT_CASHIO")]
public class SETT_CASHIO : IIdentifyEntity
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
	/// 입출금일련번호
	/// </summary>
	[Comment("입출금일련번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? CSH_IO_SEQ { get; set; }
	/// <summary>
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 입출금계정구분 0:입금 1:출금
	/// </summary>
	[Comment("입출금계정구분 0:입금 1:출금")]
	public string? ACCNT_FLAG { get; set; }
	/// <summary>
	/// 입출금계정코드
	/// </summary>
	[Comment("입출금계정코드")]
	public string? ACCNT_CODE { get; set; }
	/// <summary>
	/// 입출금계정금액
	/// </summary>
	[Comment("입출금계정금액")]
	[Precision(12, 2)]
	public decimal ACCNT_AMT { get; set; } = 0;
	/// <summary>
	/// 입출금비고
	/// </summary>
	[Comment("입출금비고")]
	public string? REMARK { get; set; }
	/// <summary>
	/// 사용구분 Y:사용 N:삭제
	/// </summary>
	[Comment("사용구분 Y:사용 N:삭제")]
	public string? USE_YN { get; set; }
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
		return "SHOP_CODE_SALE_DATE_POS_NO_CSH_IO_SEQ";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}