using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table("MST_INFO_SHOP")]
public class MST_INFO_SHOP : IIdentifyEntity
{
	/// <summary>
	/// 매장코드
	/// </summary>
	[Comment("매장코드")]
	[Key, Column(Order = 2)]
	[Required]
    [JsonPropertyName("storeCode")]
	public string? SHOP_CODE { get; set; }
	/// <summary>
	/// 매장명
	/// </summary>
	[Comment("매장명")]
    [JsonPropertyName("storeNm")]
	public string? SHOP_NAME { get; set; }
	/// <summary>
	/// 본사코드
	/// </summary>
	[Comment("본사코드")]
	[Key, Column(Order = 1)]
	[Required]
    [JsonPropertyName("fchqCode")]
	public string? HD_SHOP_CODE { get; set; }
	/// <summary>
	/// 본사-매장그룹코드  본사별설정 (H/SCD_CODEM_T : 101)
	/// </summary>
	[Comment("본사-매장그룹코드  본사별설정 (H/SCD_CODEM_T : 101)")]
    [JsonPropertyName("groupCode")]
	public string? SHOP_GROUP_CODE { get; set; }
	/// <summary>
	/// 본사-매장형태구분 본사별설정 (H/SCD_CODEM_T : 102)
	/// </summary>
	[Comment("본사-매장형태구분 본사별설정 (H/SCD_CODEM_T : 102)")]
    [JsonPropertyName("formCode")]
	public string? SHOP_TYPE_FLAG { get; set; }
	/// <summary>
	/// ASP-매장분류구분 (CCD_CODEM_T : 001)
	/// </summary>
	[Comment("ASP-매장분류구분 (CCD_CODEM_T : 001)")]
    [JsonPropertyName("storeCtgCode")]
	public string? SHOP_CLASS_FLAG { get; set; }
	/// <summary>
	/// 프로그램용도구분 0:일반 1:외식전용 2:의류잡화전용 (CCD_CODEM_T : 004)
	/// </summary>
	[Comment("프로그램용도구분 0:일반 1:외식전용 2:의류잡화전용 (CCD_CODEM_T : 004)")]
    [JsonPropertyName("useSeCode")]
	public string? PGM_TYPE_FLAG { get; set; }
	/// <summary>
	/// 대표자명
	/// </summary>
	[Comment("대표자명")]
    [JsonPropertyName("repNm")]
	public string? OWNER_NAME { get; set; }
	/// <summary>
	/// 사업자번호
	/// </summary>
	[Comment("사업자번호")]
    [JsonPropertyName("storeCorpno")]
	public string? BIZ_NO { get; set; }
	/// <summary>
	/// 업태명
	/// </summary>
	[Comment("업태명")]
    [JsonPropertyName("storeTypeNm")]
	public string? BIZ_TYPE_NAME { get; set; }
	/// <summary>
	/// 업종명
	/// </summary>
	[Comment("업종명")]
    [JsonPropertyName("storeItemNm")]
	public string? BIZ_KIND_NAME { get; set; }
	/// <summary>
	/// 상호명
	/// </summary>
	[Comment("상호명")]
    [JsonPropertyName("storeCompNm")]
	public string? BIZ_SHOP_NAME { get; set; }
	/// <summary>
	/// 전화번호
	/// </summary>
	[Comment("전화번호")]
    [JsonPropertyName("storeTelno")]
	public string? TEL_NO { get; set; }
	/// <summary>
	/// 휴대폰번호
	/// </summary>
	[Comment("휴대폰번호")]
    [JsonPropertyName("storeCelno")]
	public string? HP_NO { get; set; }
	/// <summary>
	/// 팩스번호
	/// </summary>
	[Comment("팩스번호")]
    [JsonPropertyName("storeFaxno")]
	public string? FAX_NO { get; set; }
	/// <summary>
	/// 이메일주소
	/// </summary>
	[Comment("이메일주소")]
    [JsonPropertyName("storeEmail")]
	public string? EMAIL_ADDR { get; set; }
	/// <summary>
	/// 우편번호
	/// </summary>
	[Comment("우편번호")]
    [JsonPropertyName("storePostno")]
	public string? POST_NO { get; set; }
	/// <summary>
	/// 주소
	/// </summary>
	[Comment("주소")]
    [JsonPropertyName("roadnmAdres")]
	public string? ADDR { get; set; }
	/// <summary>
	/// 주소상세
	/// </summary>
	[Comment("주소상세")]
    [JsonPropertyName("detailAdres")]
	public string? ADDR_DTL { get; set; }
	/// <summary>
	/// 매장상태구분 0:등록 1:오픈 2:폐점 3:보류 4:출고보류 (CCD_CODEM_T : 010)
	/// </summary>
	[Comment("매장상태구분 0:등록 1:오픈 2:폐점 3:보류 4:출고보류 (CCD_CODEM_T : 010)")]
    [JsonPropertyName("sttSeCode")]
	public string? SHOP_STAT_FLAG { get; set; }
	/// <summary>
	/// 매장오픈일자
	/// </summary>
	[Comment("매장오픈일자")]
    [JsonPropertyName("storeOpenDe")]
	public string? SHOP_OPEN_DATE { get; set; }
	/// <summary>
	/// 시스템오픈일자
	/// </summary>
	[Comment("시스템오픈일자")]
    [JsonPropertyName("posOpenDe")]
	public string? SYS_OPEN_DATE { get; set; }
	/// <summary>
	/// 시스템폐점일자
	/// </summary>
	[Comment("시스템폐점일자")]
    [JsonPropertyName("posCloseDe")]
	public string? SYS_CLOSE_DATE { get; set; }
	/// <summary>
	/// 상품분류관리레벨구분 1:대 2:대중 3:대중소 (CCD_CODEM_T : 048)
	/// </summary>
	[Comment("상품분류관리레벨구분 1:대 2:대중 3:대중소 (CCD_CODEM_T : 048)")]
    [JsonPropertyName("goodsSeCode")]
	public string? CLASS_MGR_LEVEL_FLAG { get; set; }
	/// <summary>
	/// 창고여부
	/// </summary>
	[Comment("창고여부")]
    [JsonPropertyName("wareAt")]
	public string? WARE_YN { get; set; }
	/// <summary>
	/// 브랜드관리여부
	/// </summary>
	[Comment("브랜드관리여부")]
    [JsonPropertyName("brandAt")]
	public string? BRAND_MGR_YN { get; set; }
	/// <summary>
	/// 판매마진관리여부
	/// </summary>
	[Comment("판매마진관리여부")]
    [JsonPropertyName("marginMgmtAt")]
	public string? SALEMG_MGR_YN { get; set; }
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
	/// 관리밴사
	/// </summary>
	[Comment("관리밴사")]
    [JsonPropertyName("mgmtVanCode")]
	public string? VAN_CORP_CODE { get; set; }
	/// <summary>
	/// 로컬 포스 구분 Y:로컬, N:ASP
	/// </summary>
	[Comment("로컬 포스 구분 Y:로컬, N:ASP")]
    [JsonPropertyName("localPosAt")]
	public string? LOCAL_POS_YN { get; set; }
	/// <summary>
	/// 담당대리점명
	/// </summary>
	[Comment("담당대리점명")]
    [JsonPropertyName("mgmtCorpNm")]
	public string? CORP_NAME { get; set; }
	/// <summary>
	/// 담당대리점연락처
	/// </summary>
	[Comment("담당대리점연락처")]
    [JsonPropertyName("mgmtCorpTelno")]
	public string? CORP_TEL_NO { get; set; }
	/// <summary>
	/// S/W사용동의여부
	/// </summary>
	[Comment("S/W사용동의여부")]
    [JsonPropertyName("swAgreeAt")]
	public string? AGREE_YN { get; set; }
    /// <summary>
    /// S/W사용동의일시
    /// </summary>
    [Comment("S/W사용동의일시")]
    [JsonPropertyName("swAgreeDe")]
	public string? AGREE_DT { get; set; }
	/// <summary>
	/// S/W사용동의전송여부
	/// </summary>
	[Comment("S/W사용동의전송여부")]
    [JsonPropertyName("swAgreeSendAt")]
	public string? AGREE_SEND_FLAG { get; set; }

	public string Base_PrimaryName()
	{
		return "SHOP_CODE_HD_SHOP_CODE";
	}

	public void EntityDefValue(EEditType eEdit, string userID, object data)
	{
	}
    
	public string Resource()
	{
		return "/client/master/store";

    }    
}