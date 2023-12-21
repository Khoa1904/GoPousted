using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_PARTCARD")]
public class TRN_PARTCARD : IIdentifyEntity
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
	/// 영수번호
	/// </summary>
	[Comment("영수번호")]
	[Key, Column(Order = 4)]
	[Required]
	public string? BILL_NO { get; set; }
	/// <summary>
	/// 순번
	/// </summary>
	[Comment("순번")]
	[Key, Column(Order = 5)]
	[Required]
	public string? SEQ_NO { get; set; }
	/// <summary>
	/// 정산차수
	/// </summary>
	[Comment("정산차수")]
	public string? REGI_SEQ { get; set; }
	/// <summary>
	/// 승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인
	/// </summary>
	[Comment("승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인")]
	public string? APPR_PROC_FLAG { get; set; }
	/// <summary>
	/// 제휴카드-코드
	/// </summary>
	[Comment("제휴카드-코드")]
	public string? JCD_CODE { get; set; }
	/// <summary>
	/// 제휴카드-유형구분 (CCD_CODEM_T : 035) 1:OCBS 2:SKT 3:KTF 4:LGT
	/// </summary>
	[Comment("제휴카드-유형구분 (CCD_CODEM_T : 035) 1:OCBS 2:SKT 3:KTF 4:LGT")]
	public string? JCD_TYPE_FLAG { get; set; }
	/// <summary>
	/// 제휴카드-처리구분 (CCD_CODEM_T : 036) 0:할인 1:적립 2:사용 3:조회
	/// </summary>
	[Comment("제휴카드-처리구분 (CCD_CODEM_T : 036) 0:할인 1:적립 2:사용 3:조회")]
	public string? JCD_PROC_FLAG { get; set; }
	/// <summary>
	/// 제휴카드-번호
	/// </summary>
	[Comment("제휴카드-번호")]
	public string? JCD_CARD_NO { get; set; }
	/// <summary>
	/// 유효기간
	/// </summary>
	[Comment("유효기간")]
	public string? VALID_TERM { get; set; }
	/// <summary>
	/// 승인금액
	/// </summary>
	[Comment("승인금액")]
	[Precision(12, 2)]
	public decimal APPR_AMT { get; set; }
	/// <summary>
	/// 카드입력구분 (CCD_CODEM_T : 032) S:Swapping K:KeyIn, I : IC - IC 추가
	/// </summary>
	[Comment("카드입력구분 (CCD_CODEM_T : 032) S:Swapping K:KeyIn, I : IC - IC 추가")]
	public string? CARD_IN_FLAG { get; set; }
	/// <summary>
	/// 사인패드사용여부 Y/N 확인필요
	/// </summary>
	[Comment("사인패드사용여부 Y/N 확인필요")]
	public string? SIGN_PAD_YN { get; set; }
	/// <summary>
	/// 승인일자
	/// </summary>
	[Comment("승인일자")]
	public string? APPR_DATE { get; set; }
	/// <summary>
	/// 승인시각
	/// </summary>
	[Comment("승인시각")]
	public string? APPR_TIME { get; set; }
	/// <summary>
	/// 승인번호
	/// </summary>
	[Comment("승인번호")]
	public string? APPR_NO { get; set; }
	/// <summary>
	/// 제휴카드-할인금액
	/// </summary>
	[Comment("제휴카드-할인금액")]
	[Precision(12, 2)]
	public decimal JCD_DC_AMT { get; set; }
	/// <summary>
	/// 제휴카드-발생포인트
	/// </summary>
	[Comment("제휴카드-발생포인트")]
	[Precision(12, 2)]
	public decimal JCD_OCC_POINT { get; set; }
	/// <summary>
	/// 제휴카드-가용포인트
	/// </summary>
	[Comment("제휴카드-가용포인트")]
	[Precision(12, 2)]
	public decimal JCD_AVL_POINT { get; set; }
	/// <summary>
	/// 제휴카드-사용포인트
	/// </summary>
	[Comment("제휴카드-사용포인트")]
	[Precision(12, 2)]
	public decimal JCD_USE_POINT { get; set; }
	/// <summary>
	/// 제휴카드-잔여포인트 OCB-누적포인트
	/// </summary>
	[Comment("제휴카드-잔여포인트 OCB-누적포인트")]
	[Precision(12, 2)]
	public decimal JCD_REM_POINT { get; set; }
	/// <summary>
	/// 밴사코드
	/// </summary>
	[Comment("밴사코드")]
	public string? VAN_CODE { get; set; }
	/// <summary>
	/// 승인응답메세지
	/// </summary>
	[Comment("승인응답메세지")]
	public string? APPR_MSG { get; set; }
	/// <summary>
	/// 밴사터미널번호
	/// </summary>
	[Comment("밴사터미널번호")]
	public string? VAN_TERM_NO { get; set; }
	/// <summary>
	/// 밴사전표번호
	/// </summary>
	[Comment("밴사전표번호")]
	public string? VAN_SLIP_NO { get; set; }
	/// <summary>
	/// 카드사가맹번호
	/// </summary>
	[Comment("카드사가맹번호")]
	public string? CRDCP_TERM_NO { get; set; }
	/// <summary>
	/// 원거래승인일자
	/// </summary>
	[Comment("원거래승인일자")]
	public string? ORG_APPR_DATE { get; set; }
	/// <summary>
	/// 원거래승인번호
	/// </summary>
	[Comment("원거래승인번호")]
	public string? ORG_APPR_NO { get; set; }
	/// <summary>
	/// 승인로그번호
	/// </summary>
	[Comment("승인로그번호")]
	public string? APPR_LOG_NO { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 판매여부  Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매여부  Y:정상 N:반품(취소)")]
	public string? SALE_YN { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_SALE_DATE_POS_NO_BILL_NO_SEQ_NO";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}