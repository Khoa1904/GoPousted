using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_MEMB_CLASS")]
public class MST_MEMB_CLASS : IIdentifyEntity
{
	/// <summary>
	/// 회원관리주체-소속코드
	/// </summary>
	[Comment("회원관리주체-소속코드")]
	[Key, Column(Order = 1)]
	[Required]
	public string? CST_OGN_CODE { get; set; }
	/// <summary>
	/// 회원등급분류코드
	/// </summary>
	[Comment("회원등급분류코드")]
	[Key, Column(Order = 2)]
	[Required]
	public string? CST_CLASS_CODE { get; set; }
	/// <summary>
	/// 회원등급순번
	/// </summary>
	[Comment("회원등급순번")]
	public Int16 CST_SEQ { get; set; }
	/// <summary>
	/// 회원등급분류명
	/// </summary>
	[Comment("회원등급분류명")]
	public string? CST_CLASS_NAME { get; set; }
	/// <summary>
	/// 할인율
	/// </summary>
	[Comment("할인율")]
	public Int16 DC_RATE { get; set; }
	/// <summary>
	/// 적립단위구분 (0: % 1:금액기준)
	/// </summary>
	[Comment("적립단위구분 (0: % 1:금액기준)")]
	public string? ACC_UNIT_FLAG { get; set; }
	/// <summary>
	/// 현금-적립기준
	/// </summary>
	[Comment("현금-적립기준")]
	public int ACC_BASE_CSH { get; set; }
	/// <summary>
	/// 신용카드-적립기준
	/// </summary>
	[Comment("신용카드-적립기준")]
	public int ACC_BASE_CRD { get; set; }
	/// <summary>
	/// 상품권-적립기준
	/// </summary>
	[Comment("상품권-적립기준")]
	public int ACC_BASE_GFT { get; set; }
	/// <summary>
	/// 식권-적립기준
	/// </summary>
	[Comment("식권-적립기준")]
	public int ACC_BASE_FOD { get; set; }
	/// <summary>
	/// 제휴카드사용-적립기준
	/// </summary>
	[Comment("제휴카드사용-적립기준")]
	public int ACC_BASE_JCD { get; set; }
	/// <summary>
	/// 외상-적립기준
	/// </summary>
	[Comment("외상-적립기준")]
	public int ACC_BASE_WES { get; set; }
	/// <summary>
	/// 회원포인트사용-적립기준
	/// </summary>
	[Comment("회원포인트사용-적립기준")]
	public int ACC_BASE_CST { get; set; }
	/// <summary>
	/// 사원카드-적립기준
	/// </summary>
	[Comment("사원카드-적립기준")]
	public int ACC_BASE_RFC { get; set; }
	/// <summary>
	/// 신규가입포인트
	/// </summary>
	[Comment("신규가입포인트")]
	public int NEW_ENTRY_POINT { get; set; }
	/// <summary>
	/// 최소사용포인트
	/// </summary>
	[Comment("최소사용포인트")]
	public int MIN_USE_POINT { get; set; }
	/// <summary>
	/// 고객등록기본등급
	/// </summary>
	[Comment("고객등록기본등급")]
	public string? BASE_CLASS_YN { get; set; }
	/// <summary>
	/// 모바일쿠폰-적립기준
	/// </summary>
	[Comment("모바일쿠폰-적립기준")]
	public int ACC_BASE_MCP { get; set; }
	/// <summary>
	/// 교통선불카드-적립기준
	/// </summary>
	[Comment("교통선불카드-적립기준")]
	public int ACC_BASE_PCD { get; set; }
	/// <summary>
	/// 회원-기념일 적립여부 (0 : 미사용 1 : 생일, 2 : 결혼기념일)
	/// </summary>
	[Comment("회원-기념일 적립여부 (0 : 미사용 1 : 생일, 2 : 결혼기념일)")]
	public string? ACC_ANN_FLAG { get; set; }
	/// <summary>
	/// 회원-기념일 적립기준
	/// </summary>
	[Comment("회원-기념일 적립기준")]
	public int ACC_BASE_ANN { get; set; }
	/// <summary>
	/// 회원-첫구매 적립기준
	/// </summary>
	[Comment("회원-첫구매 적립기준")]
	public int ACC_BASE_NEW { get; set; }
	/// <summary>
	/// 현금영수증-적립기준
	/// </summary>
	[Comment("현금영수증-적립기준")]
	public int ACC_BASE_CSB { get; set; }
	/// <summary>
	/// 스탬프 적립 정책 P:상품 C:금액
	/// </summary>
	[Comment("스탬프 적립 정책 P:상품 C:금액")]
	public string? STAMP_ACC_FLAG { get; set; }
	/// <summary>
	/// 스탬프 적립 단위 금액
	/// </summary>
	[Comment("스탬프 적립 단위 금액")]
	public int STAMP_ACC_COST { get; set; }
	/// <summary>
	/// 스탬프 적립 단위 수량
	/// </summary>
	[Comment("스탬프 적립 단위 수량")]
	public int STAMP_ACC_QTY { get; set; }
	/// <summary>
	/// 스탬프 사용 정책 P:상품 C:금액
	/// </summary>
	[Comment("스탬프 사용 정책 P:상품 C:금액")]
	public string? STAMP_USE_FLAG { get; set; }
	/// <summary>
	/// 스탬프 사용 단위 금액
	/// </summary>
	[Comment("스탬프 사용 단위 금액")]
	public int STAMP_USE_COST { get; set; }
	/// <summary>
	/// 스탬프 사용 단위 수량
	/// </summary>
	[Comment("스탬프 사용 단위 수량")]
	public int STAMP_USE_QTY { get; set; }
	/// <summary>
	/// 할인시 스탬프 적용여부
	/// </summary>
	[Comment("할인시 스탬프 적용여부")]
	public string? DC_ACC_YN { get; set; }
	/// <summary>
	/// 포인트 사용시 적립여부
	/// </summary>
	[Comment("포인트 사용시 적립여부")]
	public string? USE_ACC_YN { get; set; }
	/// <summary>
	/// 최대사용 마일리지/스탬프
	/// </summary>
	[Comment("최대사용 마일리지/스탬프")]
	public int MAX_USE_POINT { get; set; }
	/// <summary>
	/// 스탬프 0원상품 적립여부
	/// </summary>
	[Comment("스탬프 0원상품 적립여부")]
	public string? STAMP_ZERO_ACC_YN { get; set; }
	/// <summary>
	/// 일조정횟수
	/// </summary>
	[Comment("일조정횟수")]
	public int DAY_ADJ_CNT { get; set; }
	/// <summary>
	/// 조정금액상한선
	/// </summary>
	[Comment("조정금액상한선")]
	public int LIMIT_ADJ_AMT { get; set; }
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
		return "CST_OGN_CODE_CST_CLASS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		throw new NotImplementedException();
	}    
}