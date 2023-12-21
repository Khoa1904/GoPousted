﻿using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_MCLASS")]
public class MST_INFO_MCLASS : IIdentifyEntity
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
	/// 상품-중분류코드
	/// </summary>
	[Comment("상품-중분류코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("midCtgCode")]
	public string? MCLASS_CODE { get; set; }
	/// <summary>
	/// 상품-중분류명
	/// </summary>
	[Comment("상품-중분류명")]
    [JsonPropertyName("midCtgNm")]
	public string? MCLASS_NAME { get; set; }
	/// <summary>
	/// 상품-대분류코드
	/// </summary>
	[Comment("상품-대분류코드")]
    [JsonPropertyName("highCtgCode")]
	public string? LCLASS_CODE { get; set; }
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
    [JsonPropertyName("updatedAt")]
	public string? UPDATE_DT { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_MCLASS_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/goods/category/mid";
    }    
}