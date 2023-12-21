﻿using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("TRN_FOODCPN")]
public class TRN_FOODCPN : IIdentifyEntity
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
	/// 상품권/식권코드 상품권 MASTER CODE
	/// </summary>
	[Comment("상품권/식권코드 상품권 MASTER CODE")]
	public string? TK_GFT_CODE { get; set; }
	/// <summary>
	/// 상품권/식권 BARCODE
	/// </summary>
	[Comment("상품권/식권 BARCODE")]
	public string? TK_GTF_BARCODE { get; set; }
	/// <summary>
	/// 식권-액면가액
	/// </summary>
	[Comment("식권-액면가액")]
	[Precision(12, 2)]
	public decimal TK_FOD_UAMT { get; set; } = 0;
	/// <summary>
	/// 식권-결제액
	/// </summary>
	[Comment("식권-결제액")]
	[Precision(12, 2)]
	public decimal TK_FOD_AMT { get; set; }
	/// <summary>
	/// 식권-할인액
	/// </summary>
	[Comment("식권-할인액")]
	[Precision(12, 2)]
	public decimal TK_FOD_DC_AMT { get; set; }
	/// <summary>
	/// 식권-짜투리액
	/// </summary>
	[Comment("식권-짜투리액")]
	[Precision(12, 2)]
	public decimal ETC_AMT { get; set; }
	/// <summary>
	/// 식권-거스름돈
	/// </summary>
	[Comment("식권-거스름돈")]
	[Precision(12, 2)]
	public decimal REPAY_CASH_AMT { get; set; }
	/// <summary>
	/// 등록일시
	/// </summary>
	[Comment("등록일시")]
	public string? INSERT_DT { get; set; }
	/// <summary>
	/// 판매여부 Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매여부 Y:정상 N:반품(취소)")]
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