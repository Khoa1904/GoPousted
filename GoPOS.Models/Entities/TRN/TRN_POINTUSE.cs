﻿using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using log4net.Core;

namespace GoPOS.Models;

[Table("TRN_POINTUSE")]
public class TRN_POINTUSE : IIdentifyEntity
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
	/// 판매여부 Y:정상 N:반품(취소)
	/// </summary>
	[Comment("판매여부 Y:정상 N:반품(취소)")]
	public string?	SALE_YN			{get;set;}
    public string	CST_NO			{get;set;}	= string.Empty ;
	public string	CARD_NO			{get;set;}	= string.Empty ;
	public string	LEVEL			{get;set;}	= string.Empty ;
	public decimal	TOT_SALE_AMT	{get;set;}	= 0 ;
	public decimal	TOT_DC_AMT		{get;set;}	= 0 ;
	public decimal	TOT_PNT			{get;set;}	= 0 ;
	public decimal	TOT_USE_PNT		{get;set;}	= 0 ;
	public decimal	USE_PNT			{get;set;}	= 0 ;
	public decimal	LAST_PNT		{get;set;}	= 0 ;


	/// <summary>
	/// USE_STAMP 사용 스탬프 개수
	/// </summary>
	public decimal USE_STAMP { get; set; } = 0;

    /// <summary>
    /// USE_TYPE 사용포인트 구분
	/// 01:포인트사용, 02:스탬프사용, 03:스탬프쿠폰
    /// </summary>
    public string USE_TYPE { get; set; } = string.Empty ;


    public string	INSERT_DT		{get;set;}	= string.Empty;

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