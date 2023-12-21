using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.PrinterLib
{
    public static class FieldMapping
    {
        public static readonly Dictionary<string, string> dicField = new Dictionary<string, string>
        {
           { "상품리스트1" , "PrintItemList1"},
           { "상품목록1" , "PrintItemList1"},
           { "상품리스트2" , "PrintItemList2"},
           { "상품목록2" , "PrintItemList2"},
           { "결제리스트" , "PrintPayment"},
           { "회원정보" , "PrintMemberInfo"},
           { "현금영수증" , "PrintCashBill"},
           { "신용카드" , "PrintCreditCard"},
           { "상품권정보" , "PrintGiftInfo"},
           { "식권정보" , "PrintTicketFood"},
           { "매출상품권내역" , "PrintSaleGift"},
           { "비매출정산내역" , "PrintNonSaleSettlement"},
           { "상품권시재내역" , "PrintGiftCard"},
           { "회원포인트내역" , "PrintMemberPointInfo"},
           { "카드사별매출내역" , "PrintSaleByCard"},
           { "신용카드정산내역" , "PrintCreditCardSettlement"},
           { "현금정산액" , "PrintCashSettlement"},
           { "현금시재입력내역" , "PrintCashBalance"},
           { "할인매출내역" , "PrintDiscountSales"},
           { "결제수단별매출내역" , "PaymentMethod"},
           { "매출합계" , "PrintSaleTotal"},
           { "date" , "DATE" },
           { "영수증번호" , "BILL_NUMBER"},
           { "타이틀" , "TITLE"},
           { "상호" , "SHOP_NAME"},
           { "전화번호" , "TEL_NO"},
           { "주소" , "ADDRESS"},
           { "대표자" , "OWNER_NAME"},
           { "POSNO" , "POS_NO"},
           { "포스번호" , "POS_NO"},
           { "DATETIME" , "DATETIME"},
           { "시간정보" , "DATETIME"},
           { "계산원" , "EMP_NO_NAME"},
           { "합계금액" , "TOT_SALE_AMT"},
           { "할인금액" , "TOT_DC_AMT"},
           { "받을금액" , "EXP_PAY_AMT"},
           { "받을금액1" , "EXP_PAY_AMT"},
           { "받은금액" , "GST_PAY_AMT"},
           { "거스름" , "RET_PAY_AMT"},
           { "sign" , "SIGN"},
           { "사업자" , "BIZ_NO"},
           { "세액" , "PrintVAT"},
           { "간편결제" , "SIMPLE_CARD"},
           { "전자상품권" , "PURCHASE_RECEIPT"},
           { "모바일쿠폰" , "MOBILE_COUPON"},
           { "별도-신용승인전표-내역" , "SPE_CREDIT_CARD"},
           { "매출시간" , "SALE_TIME"},
           { "영업일자" , "SALE_DATE"},
           { "정산원" , "EMP_NO_NAME"},
           { "정산차수" , "REGI_SEQ" },
           { "개점시간" , "OPEN_DT"},
           { "정산시간" , "REGI_TIME"},
           { "비매출상품권내역" , ""},
           { "비매출합계금액" , ""},
           { "비매출받을금액" , ""},
           { "비매출받은금액" , ""},
           { "비매출거스름돈" , ""},
           { "주방메모" , ""},
           { "주문서-메뉴출력형태3" , "Menu3"},
           { "주문서 상품목록3" , "Menu3"},
           { "주문번호" , "ORDER_ID"},
           { "교환번호" , "EX_NUM" },
           { "시재점검" , "PrintTrialCheck" },
           { "기타출력" , "" },
           { "코너별-메뉴목록" , "PrintCornerItems" },
           { "코너별 상품목록" , "PrintCornerItems" },
           { "이전차수-메뉴목록", "CustOrderMenuList"},
           { "주문서-메뉴출력형태1","CustOrderMenuList"},
           { "배달전화번호",""},
           { "배달지주소",""},
           {"선결제충전상품목록1","RechareProducts"},
           {"선결제합계금액","TotalPrechangreAmount"},
           {"선결제받을금액","AmountInAdvance"},
           {"선결제받은금액","PrerpaidAmout"},
           {"선결제거스름돈","PrepaidChange"},
           {"재출력","Reprint"},
           {"재발행여부","ReprintYN"}
        };
    }

    public enum PrintOptions
    {
        Normal = 0,
        JournalOnly = 1,
        PrinterOnly = 2
    }

    public enum ReceiptPrintReturns
    {
        CanPrint,
        Offline,
        DontPrint,
        Errored
    }

    public class PAYMENT
    {
        public PAYMENT()
        {
            PAY_AMT = 0;
            WES_AMT = -1;
            RFC_AMT = -1;
            CST_POINT_AMT = 0;
        }

        [Comment("현금")]
        public decimal PAY_AMT { get; set; }
        [Comment("신용카드")]
        public decimal CRD_CARD_AMT { get; set; }
        [Comment("외상")]
        public decimal WES_AMT { get; set; }
        [Comment("상품권")]
        public decimal TK_GFT_AMT { get; set; }
        [Comment("식권")]
        public decimal TK_FOD_AMT { get; set; }
        [Comment("제휴카드")]
        public decimal JCD_CARD_AMT { get; set; }
        [Comment("포인트")]
        public decimal RFC_AMT { get; set; }
        [Comment("포인트")]
        public decimal CST_POINT_AMT { get; set; }
        public decimal CST_STAMP_AMT { get; set; }
        /// <summary>
        /// Easy pay sum amt
        /// 간편결제 총금액
        /// </summary>
        public decimal EASY_PAY_AMT { get; set; }
        public decimal PPC_PAY_AMT { get; set; }

        [Comment("간편결제")]
        public PAY_PRINT_ITEM[] EASY_PAY_AMTs { get; set; }
    }

    public class PAY_PRINT_ITEM
    {
        public string PAY_NAME { get; set; }
        public decimal PAY_AMT { get; set; }
    }

    public enum PRINT_TITLES
    {
        RECEIPT,
        MIDDLESETTLE,
        CLOSESETTLE,
        KITCHEN,
        TRIALCHECK,
        ORDERLIST,
        PREPAYMENTINPUT,
    }

}
