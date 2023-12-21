using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoPOS.Views;
using GoPOS.ViewModels;
using System.Windows.Controls;
using System.Windows;
using GoPOS.Helpers;

namespace GoPOS
{
    public static class Menu
    {


        //확장 메뉴
        public static void GoExtMenu(string No)
        {
            //임시 테스트 버튼 모음
            if (No == "202")
            {
                orderPayMainViewModel.ActiveForm("OrderPayTestViewModel");
                return;
            }

            if (No == "206")
            {
                //	206	배달처리 전체화면
                orderPayMainViewModel.ActiveItemAllPop = IoC.Get<OrderPayDlvrProcessViewModel>();
            }
            else if (No == "221")
            {
                //	221	예약조회 중간팝업화면
                orderPayMainViewModel.ActiveItemMidPop = IoC.Get<OrderPayResveInqireViewModel>();
            }
            else if (No == "209")
            {
                //	209	출력관리 왼쪽하단 확장메뉴화면에 바인딩
                IoC.Get<OrderPayLeftViewModel>().ActiveItem2 = IoC.Get<OrderPayPrintMngViewModel>();
                IoC.Get<OrderPayLeftView>().stackExt.Visibility = Visibility.Visible;
            }
            else if (No == "212")
            {
                //	212	확장메뉴 왼쪽하단 확장메뉴화면에 바인딩
                IoC.Get<OrderPayLeftViewModel>().ActiveItem2 = IoC.Get<OrderPayExtMenuViewModel>();
                IoC.Get<OrderPayLeftView>().stackExt.Visibility = Visibility.Visible;
            }
            else if (No == "253")
            {
                //	253	판매현황 전체화면
                IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Visible;
                IoC.Get<OrderPayMainViewModel>().ActiveItemAllPop = IoC.Get<OrderPaySaleStatusViewModel>();
            }
            else if (No == "267")
            {
                //	267	대기표발급(대기접수) 중간팝업화면
                IoC.Get<OrderPayMainView>().DockMidPop2.Visibility = Visibility.Visible;
                IoC.Get<OrderPayMainViewModel>().ActiveItemMidPop979x529 = IoC.Get<OrderPayWaitTicketIssuViewModel>();
            }
            else if (No == "281")
            {
                //	206	배달처리 전체화면
                IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Visible;
                IoC.Get<OrderPayMainViewModel>().ActiveItemAllPop = IoC.Get<OrderPayDlvrSttusViewModel>();
            }
            else if (No == "291")
            {
                //	291	전화이력
                IoC.Get<OrderPayMainView>().DockMidPop.Visibility = Visibility.Visible;
                IoC.Get<OrderPayMainViewModel>().ActiveItemMidPop = IoC.Get<OrderPayTlphonHistViewModel>();
            }
            else
            {
                IoC.Get<OrderPayMainViewModel>().ActiveForm(GetModel(No));
            }
        }

        //영업관리 메뉴
        public static void GoSalesMngMenu(string No)
        {
            IoC.Get<SalesMngMainViewModel>().ActiveForm(GetModel(No)); 
        }

        //매출현황 메뉴
        public static void GoSellingStatusMenu(string No)
        {
            IoC.Get<SellingStatusMainViewModel>().ActiveForm(GetModel(No));
        }

        //매출현황 메뉴
        public static void GoConfigSetupMenu(string No)
        {
            IoC.Get<ConfigSetupMainViewModel>().ActiveForm(GetModel(No));
        }


        public static string GetModel(string No)
        {
            string retValue = "";

            switch (No)
            {

                /*

            FK_NO	FK_NAME	AUTH_YN	POSITION_NO	COL_NUM	ROW_NUM	WIDTH_NUM	HEIGHT_NUM	USE_YN	FK_FLAG
            101	공지사항	    N	1	0	0	0	0	N	1
            115	매출 I/F	    N	2	0	0	0	0	N	1
            103	마감취소	    N	3	0	0	0	0	N	1
            104	시재입출금	    N	4	0	0	0	0	N	1
            105	시재점검	    N	5	0	0	0	0	N	1
            106	외상입금	    N	6	0	0	0	0	N	1
            108	판매원변경	    N	7	0	0	0	0	N	1
            109	마감정산	    N	8	0	0	0	0	N	1
            112	중간정산	    N	9	0	0	0	0	N	1
            113	준비금등록	    N	10	0	0	0	0	N	1
            107	사원근태	    N	11	0	0	0	0	N	1
            110	상품권판매	    N	12	0	0	0	0	N	1
            114	기사출금	    N	13	0	0	0	0	N	1
            117	전자상품권	    N	14	0	0	0	0	N	1
            118	알차고경영비서	N	15	0	0	0	0	N	1

            ClSelngSttusViewModel            분류별 매출현황	301
            GoodsSelngSttusViewModel         상품별 매출현황	302
            PaymntSelngSttusViewModel        결제유형별 매출현황	303
            DscntSelngSttusViewModel         할인유형별 매출현황	304
            MtSelngSttusViewModel            월별 매출현황	305	
            ExcclcSttusViewModel             정산현황	306	
            TimeSelngSttusViewModel          시간대별 매출현황	307
            RciptSelngSttusViewModel         영수증별 매출현황	308
            GoodsOrderCanclSttusViewModel    상품별주문취소현황	309
            CrnSelngSttusViewModel           코너별 매출현황	310
            TmPrRcppaySttusViewModel         시재입출금현황	311
            TableGroupSelngSttusViewModel    테이블그룹 매출현황	312

            502 포스별 환경설정              ConfigPosConfigSetupViewModel
            503 자료보관(백업)
            504 마스터수신설정               ConfigMstrRecptnSetupViewModel
            505 단말기인증                   ConfigTrmnlCrtfcViewModel
            506 매출자료수신                 ConfigSellingDataRecptnViewModel
            507 원격제어
            508 시스템로그전송
            509 포스데이터관리               ConfigPosDataMngViewModel
            510 매출자료송신                 ConfigSellingDataTrnsmisViewModel
            511 보안리더기 무결성 점검       ConfigScrtyRdrIntgrtyChckViewModel


                 */


                //101 공지사항
                case "101":
                    retValue = "SalesNoticeViewModel";
                    break;
                //103 마감취소
                case "103":
                    retValue = "SalesClosingCancelViewModel";
                    break;
                //104 시재입출금
                case "104":
                    retValue = "SalesTmPrRcppayViewModel";
                    break;
                //105 시재점검
                case "105":
                    retValue = "SalesTmPrCheckViewModel";
                    break;
                //106 외상입금
                case "106":
                    retValue = "SalesCreditRcpMnyViewModel";
                    break;
                //108 판매원변경
                case "108":
                    retValue = "SalesSellerChangeViewModel";
                    break;
                //109 마감정산
                case "109":
                    retValue = "SalesClosingSettleViewModel";
                    break;
                //112 중간정산
                case "112":
                    retValue = "SalesMiddleExcClcViewModel";
                    break;
                //113 준비금등록
                case "113":
                    retValue = "SalesResvMneyRegistViewModel";
                    break;
                //107 사원근태
                case "107":
                    retValue = "SalesEmpDclzViewModel";
                    break;
                //110 상품권판매
                case "110":
                    retValue = "SalesGiftSaleViewModel";
                    break;
                //117 전자상품권
                case "117":
                    retValue = "SalesElctrnGcctViewModel";
                    break;

                //	201	상품조회
                case "201":
                    retValue = "OrderPayGoodSearchViewModel";
                    break;
                //	202	매출요약
                case "202":
                    retValue = "";
                    break;
                //	203	재고조회
                case "203":
                    retValue = "";
                    break;
                //	204	타매장재고
                case "204":
                    retValue = "OrderPayOtherStoreInvViewModel";
                    break;
                //	206	배달처리
                case "206":
                    retValue = "OrderPayDlvrProcessViewModel";
                    break;
                //	207	수표조회
                case "207":
                    retValue = "";
                    break;
                //	208	대기
                case "208":
                    retValue = "OrderPayWaitingViewModel";
                    break;
                //	209	출력관리
                case "209":
                    retValue = "OrderPayPrintMngViewModel";
                    break;
                //	210	웹정보
                case "210":
                    retValue = "";
                    break;
                //	211	환전
                case "211":
                    retValue = "";
                    break;
                //	212	확장메뉴
                case "212":
                    retValue = "";
                    break;
                //	213	주문접수
                case "213":
                    retValue = "OrderPayReceiptViewModel";
                    break;
                //	214	결제추가
                case "214":
                    retValue = "OrderPaySetleAditViewModel";
                    break;
                //	215	포장
                case "215":
                    retValue = "";
                    break;
                //	216	회원등록
                case "216":
                    retValue = "OrderPayMemberRegistViewModel";
                    break;
                //	217	단가변경
                case "217":
                    retValue = "";
                    break;
                //	218	곱배기
                case "218":
                    retValue = "";
                    break;
                //	219	반찬만
                case "219":
                    retValue = "";
                    break;
                //	220	여행사조회
                case "220":
                    retValue = "";
                    break;
                //	221	예약조회
                case "221":
                    retValue = "OrderPayResveInqireViewModel";
                    break;
                //	222	발권(수량)
                case "222":
                    retValue = "";
                    break;
                //	223	배달주문접수
                case "223":
                    retValue = "OrderPayDlvrOrderRceptViewModel";
                    break;
                //	224	중간계산서
                case "224":
                    retValue = "";
                    break;
                //	225	로열티쿠폰
                case "225":
                    retValue = "";
                    break;
                //	226	전체취소
                case "226":
                    retValue = "";
                    break;
                //	227	선택취소
                case "227":
                    retValue = "";
                    break;
                //	228	수량감소
                case "228":
                    retValue = "";
                    break;
                //	229	수량변경
                case "229":
                    retValue = "";
                    break;
                //	230	퇴실
                case "230":
                    retValue = "";
                    break;
                //	231	보관함출력
                case "231":
                    retValue = "OrderPayLockerPrintViewModel";
                    break;
                //	232	다음단골
                case "232":
                    retValue = "";
                    break;
                //	233	반품
                case "233":
                    retValue = "";
                    break;
                //	234	결제변경
                case "234":
                    retValue = "";
                    break;
                //	235	입장(바코드)
                case "235":
                    retValue = "OrderPayEntncBrcdViewModel";
                    break;
                //	236	퇴장(바코드)
                case "236":
                    retValue = "OrderPayLvBrcdViewModel";
                    break;
                //	238	TRS
                case "238":
                    retValue = "OrderPayTrsViewModel";
                    break;
                //	239	외상결제
                case "239":
                    retValue = "OrderPayCreditSetleViewModel";
                    break;
                //	240	주차권
                case "240":
                    retValue = "";
                    break;
                //	241	예술의전당
                case "241":
                    retValue = "";
                    break;
                //	242	1mile
                case "242":
                    retValue = "";
                    break;
                //	243	NH비타민
                case "243":
                    retValue = "";
                    break;
                //	244	계열사할인
                case "244":
                    retValue = "";
                    break;
                //	245	회원검색
                case "245":
                    retValue = "OrderPayMemberSearchViewModel";
                    break;
                //	246	재고폐기
                case "246":
                    retValue = "";
                    break;
                //	247	기부
                case "247":
                    retValue = "";
                    break;
                //	248	할인
                case "248":
                    retValue = "";
                    break;
                //	249	칸투칸쿠폰
                case "249":
                    retValue = "";
                    break;
                //	250	칸투칸 AS
                case "250":
                    retValue = "";
                    break;
                //	251	전체포장
                case "251":
                    retValue = "";
                    break;
                //	252	입금/출금
                case "252":
                    retValue = "";
                    break;
                //	253	판매현황
                case "253":
                    retValue = "OrderPaySaleStatusViewModel";
                    break;
                //	254	SALT
                case "254":
                    retValue = "";
                    break;
                //	255	PAYCO
                case "255":
                    retValue = "";
                    break;
                //	257	입장/퇴장 내역
                case "257":
                    retValue = "OrderPayEntncLvDtlsViewModel";
                    break;
                //	258	교환
                case "258":
                    retValue = "";
                    break;
                //	259	영수증쿠폰
                case "259":
                    retValue = "";
                    break;
                //	260	SURF
                case "260":
                    retValue = "";
                    break;
                //	261	회원적립
                case "261":
                    retValue = "";
                    break;
                //	262	OK캐쉬백
                case "262":
                    retValue = "";
                    break;
                //	263	재출력
                case "263":
                    retValue = "";
                    break;
                //	267	대기표발급
                case "267":
                    retValue = "OrderPayWaitTicketIssuViewModel";
                    break;
                //	268	온라인발권
                case "268":
                    retValue = "";
                    break;
                //	269	레저큐
                case "269":
                    retValue = "";
                    break;
                //	270	웹사이트접속
                case "270":
                    retValue = "";
                    break;
                //	271	스탬프쿠폰
                case "271":
                    retValue = "";
                    break;
                //	272	직전영수증
                case "272":
                    retValue = "";
                    break;
                //	273	커피에반하다
                case "273":
                    retValue = "";
                    break;
                //	274	MTOUCH
                case "274":
                    retValue = "";
                    break;
                //	275	쥬씨멤버십
                case "275":
                    retValue = "";
                    break;
                //	276	모바일오더
                case "276":
                    retValue = "";
                    break;
                //	277	판매종료
                case "277":
                    retValue = "";
                    break;
                //	279	배달접수
                case "279":
                    retValue = "";
                    break;
                //	280	배달주문내역
                case "280":
                    retValue = "OrderPayDlvrOrderDtlsViewModel";
                    break;
                //	281	배달현황
                case "281":
                    retValue = "OrderPayDlvrSttusViewModel";
                    break;
                //	283	모바일바코드
                case "283":
                    retValue = "";
                    break;
                //	285	PAYG
                case "285":
                    retValue = "";
                    break;
                //	287	품절
                case "287":
                    retValue = "OrderPaySoldOutViewModel";
                    break;
                //	288	코코넛적립(발트)
                case "288":
                    retValue = "";
                    break;
                //	290	픽업현황
                case "290":
                    retValue = "";
                    break;
                //	291	전화이력
                case "291":
                    retValue = "OrderPayTlphonHistViewModel";
                    break;
                //	292	모바일운전면허조회
                case "292":
                    retValue = "OrderPayMobileDrvlsInqireViewModel";
                    break;
                //	293	직전카드전표
                case "293":
                    retValue = "";
                    break;
                //	294	두끼멤버십
                case "294":
                    retValue = "";
                    break;
                //	296	샐러디
                case "296":
                    retValue = "";
                    break;
                //	297	선불카드 충전
                case "297":
                    retValue = "";
                    break;
                //	299	e코오롱
                case "299":
                    retValue = "";
                    break;
                //301   분류별 매출현황	          
                case "301":
                    retValue = "ClSelngSttusViewModel";
                    break;
                //302   상품별 매출현황	          
                case "302":
                    retValue = "GoodsSelngSttusViewModel";
                    break;
                //303   결제유형별 매출현황	    
                case "303":
                    retValue = "PaymntSelngSttusViewModel";
                    break;
                //304   할인유형별 매출현황	   
                case "304":
                    retValue = "DscntSelngSttusViewModel";
                    break;
                //305   월별 매출현황	           
                case "305":
                    retValue = "MtSelngSttusViewModel";
                    break;
                //306   정산현황	                 
                case "306":
                    retValue = "ExcclcSttusViewModel";
                    break;
                //307   시간대별 매출현황	         
                case "307":
                    retValue = "TimeSelngSttusViewModel";
                    break;
                //308   영수증별 매출현황	      
                case "308":
                    retValue = "RciptSelngSttusViewModel";
                    break;
                //309   상품별주문취소현황	     
                case "309":
                    retValue = "GoodsOrderCanclSttusViewModel";
                    break;
                //310   코너별 매출현황	           
                case "310":
                    retValue = "CrnSelngSttusViewModel";
                    break;
                //311   시재입출금현황            
                case "311":
                    retValue = "TmPrRcppaySttusViewModel";
                    break;
                //312   테이블그룹 매출현황	    
                case "312":
                    retValue = "TableGroupSelngSttusViewModel";
                    break;

                //502 포스별 환경설정
                case "502":
                    retValue = "ConfigPosConfigSetupViewModel";
                    break;
                //503 자료보관(백업)
                case "503":
                    retValue = "";
                    //DialogHelper.Show("", true);
                    break;
                //504 마스터수신설정
                case "504":
                    retValue = "ConfigMstrRecptnSetupViewModel";
                    break;
                //505 단말기인증
                case "505":
                    retValue = "ConfigTrmnlCrtfcViewModel";
                    break;
                //506 매출자료수신
                case "506":
                    retValue = "ConfigSellingDataRecptnViewModel";
                    break;
                //507 원격제어
                case "507":
                    retValue = "";
                    break;
                //508 시스템로그전송
                case "508":
                    retValue = "";
                    break;
                //509 포스데이터관리
                case "509":
                    retValue = "ConfigPosDataMngViewModel";
                    break;
                //510 매출자료송신
                case "510":
                    retValue = "ConfigSellingDataTrnsmisViewModel";
                    break;
                //511 보안리더기 무결성 점검
                case "511":
                    retValue = "ConfigScrtyRdrIntgrtyChckViewModel";
                    break;

                //	601	현금
                case "601":
                    retValue = "OrderPayCashViewModel";
                    break;
                //	602	신용카드
                case "602":
                    retValue = "OrderPayCardViewModel";
                    break;
                //	603	복합결제
                case "603":
                    retValue = "OrderPayCompPayViewModel";                    
                    break;
                //	604	제휴할인
                case "604":
                    retValue = "OrderPayCoprtnDscntViewModel";
                    break;
                //	605	쿠폰
                case "605":
                    retValue = "OrderPayCouponViewModel";
                    break;
                //	606	서비스
                case "606":
                    retValue = "";
                    break;
                //	607	부분반품
                case "607":
                    retValue = "";
                    break;
                //	608	영수증관리
                case "608":
                    retValue = "OrderPayReceiptMngViewModel";
                    break;
                //	609	곱빼기
                case "609":
                    retValue = "";
                    break;
                //	610	봉사료
                case "610":
                    retValue = "";
                    break;
                //	612	단순현금
                case "612":
                    retValue = "";
                    break;
                //	613	은련카드
                case "613":
                    retValue = "OrderPayUnionViewModel";
                    break;
                //	614	모바일바코드
                case "614":
                    retValue = "";
                    break;
                //	615	자사카드
                case "615":
                    retValue = "";
                    break;
                //	616	모바일쿠폰
                case "616":
                    retValue = "OrderPayMoblieCouponViewModel";
                    break;
                //	617	캐시비
                case "617":
                    retValue = "";
                    break;
                //	618	포인트/스탬프
                case "618":
                    retValue = "OrderPayMemberPointUseViewModel";
                    break;
                //	619	페이코
                case "619":
                    retValue = "";
                    break;
                //	622	모바일결제
                case "622":
                    retValue = "OrderPayMobileSetleViewModel";
                    break;
                //	623	환급조회
                case "623":
                    retValue = "";
                    break;
                //	624	더치페이
                case "624":
                    retValue = "OrderPayDutchPayTab1ViewModel";
                    break;
                //	625	위쳇페이
                case "625":
                    retValue = "";
                    break;
                //	626	알리페이
                case "626":
                    retValue = "";
                    break;
                //	627	선불카드
                case "627":
                    retValue = "OrderPayPrepaidCardTab1ViewModel";
                    break;
                //	628	MTOUCH PAY
                case "628":
                    retValue = "";
                    break;
                //	629	전자상품권
                case "629":
                    retValue = "OrderPayElctrnGcctViewModel";
                    break;
                //	630	카카오페이
                case "630":
                    retValue = "";
                    break;
                //	631	SSG PAY
                case "631":
                    retValue = "";
                    break;
                //	632	PAYG PAY
                case "632":
                    retValue = "";
                    break;
                //	633	QR 결제
                case "633":
                    retValue = "";
                    break;
                //	634	카카오페이&머니
                case "634":
                    retValue = "";
                    break;
                //	635	썸패스
                case "635":
                    break;
                //	636	KIS선불카드
                case "636":
                    retValue = "";
                    break;
                //	637	제로페이
                case "637":
                    retValue = "";
                    break;
                //	638	EMV-QR
                case "638":
                    retValue = "";
                    break;
                //	640	간편결제
                case "640":
                    retValue = "";
                    break;
                //	699	주문
                case "699":
                    retValue = "";
                    break;


                default:
                    break;
            }

            if (retValue == "")
            {
                if (No == "503")
                {
                    if (DialogHelper.Show("데이터베이스 자료보관을 실행하시겠습니까?", true))
                    {

                    }
                }
                else if (No == "508")
                {
                    if (DialogHelper.Show("POS 시스템 로그파일을 전송하시겠습니까?", true))
                    {

                    }
                }
                else
                    DialogHelper.Show("없는 화면이거나 작업중인 화면입니다.");
            }
            return retValue;
        }

        //메뉴이름 가져오기-띄어쓰기 
        public static string GetMenuName(string No)
        {
            string retValue = "";

            switch (No)
            {

                /*

ClSelngSttusViewModel            분류별 매출현황	301
GoodsSelngSttusViewModel         상품별 매출현황	302
PaymntSelngSttusViewModel        결제유형별 매출현황	303
DscntSelngSttusViewModel         할인유형별 매출현황	304
MtSelngSttusViewModel            월별 매출현황	305	
ExcclcSttusViewModel             정산현황	306	
TimeSelngSttusViewModel          시간대별 매출현황	307
RciptSelngSttusViewModel         영수증별 매출현황	308
GoodsOrderCanclSttusViewModel    상품별주문취소현황	309
CrnSelngSttusViewModel           코너별 매출현황	310
TmPrRcppaySttusViewModel         시재입출금현황	311
TableGroupSelngSttusViewModel    테이블그룹 매출현황	312

                 */


                //101 공지사항
                case "101":
                    retValue = "";
                    break;
                //103 마감취소
                case "103":
                    retValue = "";
                    break;
                //104 시재입출금
                case "104":
                    retValue = "";
                    break;
                //105 시재점검
                case "105":
                    retValue = "";
                    break;
                //106 외상입금
                case "106":
                    retValue = "";
                    break;
                //108 판매원변경
                case "108":
                    retValue = "";
                    break;
                //109 마감정산
                case "109":
                    retValue = "";
                    break;
                //112 중간정산
                case "112":
                    retValue = "";
                    break;
                //113 준비금등록
                case "113":
                    retValue = "";
                    break;
                //107 사원근태
                case "107":
                    retValue = "";
                    break;
                //110 상품권판매
                case "110":
                    retValue = "";
                    break;
                //117 전자상품권
                case "117":
                    retValue = "";
                    break;

                //	201	상품조회
                case "201":
                    retValue = "";
                    break;
                //	202	매출요약
                case "202":
                    retValue = "";
                    break;
                //	203	재고조회
                case "203":
                    retValue = "";
                    break;
                //	204	타매장재고
                case "204":
                    retValue = "";
                    break;
                //	206	배달처리
                case "206":
                    retValue = "";
                    break;
                //	207	수표조회
                case "207":
                    retValue = "";
                    break;
                //	208	대기
                case "208":
                    retValue = "";
                    break;
                //	209	출력관리
                case "209":
                    retValue = "";
                    break;
                //	210	웹정보
                case "210":
                    retValue = "";
                    break;
                //	211	환전
                case "211":
                    retValue = "";
                    break;
                //	212	확장메뉴
                case "212":
                    retValue = "";
                    break;
                //	213	주문접수
                case "213":
                    retValue = "";
                    break;
                //	214	결제추가
                case "214":
                    retValue = "";
                    break;
                //	215	포장
                case "215":
                    retValue = "";
                    break;
                //	216	회원등록
                case "216":
                    retValue = "";
                    break;
                //	217	단가변경
                case "217":
                    retValue = "";
                    break;
                //	218	곱배기
                case "218":
                    retValue = "";
                    break;
                //	219	반찬만
                case "219":
                    retValue = "";
                    break;
                //	220	여행사조회
                case "220":
                    retValue = "";
                    break;
                //	221	예약조회
                case "221":
                    retValue = "";
                    break;
                //	222	발권(수량)
                case "222":
                    retValue = "";
                    break;
                //	223	배달주문접수
                case "223":
                    retValue = "";
                    break;
                //	224	중간계산서
                case "224":
                    retValue = "";
                    break;
                //	225	로열티쿠폰
                case "225":
                    retValue = "";
                    break;
                //	226	전체취소
                case "226":
                    retValue = "";
                    break;
                //	227	선택취소
                case "227":
                    retValue = "";
                    break;
                //	228	수량감소
                case "228":
                    retValue = "";
                    break;
                //	229	수량변경
                case "229":
                    retValue = "";
                    break;
                //	230	퇴실
                case "230":
                    retValue = "";
                    break;
                //	231	보관함출력
                case "231":
                    retValue = "";
                    break;
                //	232	다음단골
                case "232":
                    retValue = "";
                    break;
                //	233	반품
                case "233":
                    retValue = "";
                    break;
                //	234	결제변경
                case "234":
                    retValue = "";
                    break;
                //	235	입장(바코드)
                case "235":
                    retValue = "";
                    break;
                //	236	퇴장(바코드)
                case "236":
                    retValue = "";
                    break;
                //	238	TRS
                case "238":
                    retValue = "";
                    break;
                //	239	외상결제
                case "239":
                    retValue = "";
                    break;
                //	240	주차권
                case "240":
                    retValue = "";
                    break;
                //	241	예술의전당
                case "241":
                    retValue = "";
                    break;
                //	242	1mile
                case "242":
                    retValue = "";
                    break;
                //	243	NH비타민
                case "243":
                    retValue = "";
                    break;
                //	244	계열사할인
                case "244":
                    retValue = "";
                    break;
                //	245	회원검색
                case "245":
                    retValue = "";
                    break;
                //	246	재고폐기
                case "246":
                    retValue = "";
                    break;
                //	247	기부
                case "247":
                    retValue = "";
                    break;
                //	248	할인
                case "248":
                    retValue = "";
                    break;
                //	249	칸투칸쿠폰
                case "249":
                    retValue = "";
                    break;
                //	250	칸투칸 AS
                case "250":
                    retValue = "";
                    break;
                //	251	전체포장
                case "251":
                    retValue = "";
                    break;
                //	252	입금/출금
                case "252":
                    retValue = "";
                    break;
                //	253	판매현황
                case "253":
                    retValue = "";
                    break;
                //	254	SALT
                case "254":
                    retValue = "";
                    break;
                //	255	PAYCO
                case "255":
                    retValue = "";
                    break;
                //	257	입장/퇴장 내역
                case "257":
                    retValue = "";
                    break;
                //	258	교환
                case "258":
                    retValue = "";
                    break;
                //	259	영수증쿠폰
                case "259":
                    retValue = "";
                    break;
                //	260	SURF
                case "260":
                    retValue = "";
                    break;
                //	261	회원적립
                case "261":
                    retValue = "";
                    break;
                //	262	OK캐쉬백
                case "262":
                    retValue = "";
                    break;
                //	263	재출력
                case "263":
                    retValue = "";
                    break;
                //	267	대기표발급
                case "267":
                    retValue = "";
                    break;
                //	268	온라인발권
                case "268":
                    retValue = "";
                    break;
                //	269	레저큐
                case "269":
                    retValue = "";
                    break;
                //	270	웹사이트접속
                case "270":
                    retValue = "";
                    break;
                //	271	스탬프쿠폰
                case "271":
                    retValue = "";
                    break;
                //	272	직전영수증
                case "272":
                    retValue = "";
                    break;
                //	273	커피에반하다
                case "273":
                    retValue = "";
                    break;
                //	274	MTOUCH
                case "274":
                    retValue = "";
                    break;
                //	275	쥬씨멤버십
                case "275":
                    retValue = "";
                    break;
                //	276	모바일오더
                case "276":
                    retValue = "";
                    break;
                //	277	판매종료
                case "277":
                    retValue = "";
                    break;
                //	279	배달접수
                case "279":
                    retValue = "";
                    break;
                //	280	배달주문내역
                case "280":
                    retValue = "";
                    break;
                //	281	배달현황
                case "281":
                    retValue = "";
                    break;
                //	283	모바일바코드
                case "283":
                    retValue = "";
                    break;
                //	285	PAYG
                case "285":
                    retValue = "";
                    break;
                //	287	품절
                case "287":
                    retValue = "";
                    break;
                //	288	코코넛적립(발트)
                case "288":
                    retValue = "";
                    break;
                //	290	픽업현황
                case "290":
                    retValue = "";
                    break;
                //	291	전화이력
                case "291":
                    retValue = "";
                    break;
                //	292	모바일운전면허조회
                case "292":
                    retValue = "";
                    break;
                //	293	직전카드전표
                case "293":
                    retValue = "";
                    break;
                //	294	두끼멤버십
                case "294":
                    retValue = "";
                    break;
                //	296	샐러디
                case "296":
                    retValue = "";
                    break;
                //	297	선불카드 충전
                case "297":
                    retValue = "";
                    break;
                //	299	e코오롱
                case "299":
                    retValue = "";
                    break;
                //301   분류별 매출현황	          
                case "301":
                    retValue = "분류별\r\n매출현황";
                    break;
                //302   상품별 매출현황	          
                case "302":
                    retValue = "상품별\r\n매출현황";
                    break;
                //303   결제유형별 매출현황	    
                case "303":
                    retValue = "결제유형별\r\n매출현황";
                    break;
                //304   할인유형별 매출현황	   
                case "304":
                    retValue = "할인유형별\r\n매출현황";
                    break;
                //305   월별 매출현황	           
                case "305":
                    retValue = "월별\r\n매출현황";
                    break;
                //306   정산현황	                 
                case "306":
                    retValue = "";
                    break;
                //307   시간대별 매출현황	         
                case "307":
                    retValue = "시간대별\r\n매출현황";
                    break;
                //308   영수증별 매출현황	      
                case "308":
                    retValue = "영수증별\r\n매출현황";
                    break;
                //309   상품별주문취소현황	     
                case "309":
                    retValue = "상품별주문\r\n취소현황";
                    break;
                //310   코너별 매출현황	           
                case "310":
                    retValue = "코너별\r\n매출현황";
                    break;
                //311   시재입출금현황            
                case "311":
                    retValue = "시재입출금\r\n현황";
                    break;
                //312   테이블그룹 매출현황	    
                case "312":
                    retValue = "테이블그룹\r\n매출현황";
                    break;

                //502 포스별 환경설정
                case "502":
                    retValue = "포스별\r\n환경설정";
                    break;
                //503 자료보관(백업)
                case "503":
                    retValue = "자료보관\r\n(백업)";
                    break;
                //504 마스터수신설정
                case "504":
                    retValue = "마스터수신\r\n설정";
                    break;
                //505 단말기인증
                case "505":
                    retValue = "";
                    break;
                //506 매출자료수신
                case "506":
                    retValue = "매출자료\r\n수신";
                    break;
                //507 원격제어
                case "507":
                    retValue = "";
                    break;
                //508 시스템로그전송
                case "508":
                    retValue = "시스템\r\n로그전송";
                    break;
                //509 포스데이터관리
                case "509":
                    retValue = "포스\r\n데이터관리";
                    break;
                //510 매출자료송신
                case "510":
                    retValue = "매출자료\r\n전송";
                    break;
                //511 보안리더기 무결성 점검
                case "511":
                    retValue = "보안리더기\r\n무결성점검";
                    break;

                //	601	현금
                case "601":
                    retValue = "";
                    break;
                //	602	신용카드
                case "602":
                    retValue = "";
                    break;
                //	603	복합결제
                case "603":
                    retValue = "";
                    break;
                //	604	제휴할인
                case "604":
                    retValue = "";
                    break;
                //	605	쿠폰
                case "605":
                    retValue = "";
                    break;
                //	606	서비스
                case "606":
                    retValue = "";
                    break;
                //	607	부분반품
                case "607":
                    retValue = "";
                    break;
                //	608	영수증관리
                case "608":
                    retValue = "";
                    break;
                //	609	곱빼기
                case "609":
                    retValue = "";
                    break;
                //	610	봉사료
                case "610":
                    retValue = "";
                    break;
                //	612	단순현금
                case "612":
                    retValue = "";
                    break;
                //	613	은련카드
                case "613":
                    retValue = "";
                    break;
                //	614	모바일바코드
                case "614":
                    retValue = "";
                    break;
                //	615	자사카드
                case "615":
                    retValue = "";
                    break;
                //	616	모바일쿠폰
                case "616":
                    retValue = "";
                    break;
                //	617	캐시비
                case "617":
                    retValue = "";
                    break;
                //	618	포인트/스탬프
                case "618":
                    retValue = "";
                    break;
                //	619	페이코
                case "619":
                    retValue = "";
                    break;
                //	622	모바일결제
                case "622":
                    retValue = "";
                    break;
                //	623	환급조회
                case "623":
                    retValue = "";
                    break;
                //	624	더치페이
                case "624":
                    retValue = "";
                    break;
                //	625	위쳇페이
                case "625":
                    retValue = "";
                    break;
                //	626	알리페이
                case "626":
                    retValue = "";
                    break;
                //	627	선불카드
                case "627":
                    retValue = "";
                    break;
                //	628	MTOUCH PAY
                case "628":
                    retValue = "";
                    break;
                //	629	전자상품권
                case "629":
                    retValue = "";
                    break;
                //	630	카카오페이
                case "630":
                    retValue = "";
                    break;
                //	631	SSG PAY
                case "631":
                    retValue = "";
                    break;
                //	632	PAYG PAY
                case "632":
                    retValue = "";
                    break;
                //	633	QR 결제
                case "633":
                    retValue = "";
                    break;
                //	634	카카오페이&머니
                case "634":
                    retValue = "";
                    break;
                //	635	썸패스
                case "635":
                    break;
                //	636	KIS선불카드
                case "636":
                    retValue = "";
                    break;
                //	637	제로페이
                case "637":
                    retValue = "";
                    break;
                //	638	EMV-QR
                case "638":
                    retValue = "";
                    break;
                //	640	간편결제
                case "640":
                    retValue = "";
                    break;
                //	699	주문
                case "699":
                    retValue = "";
                    break;


                default:
                    break;
            }

            return retValue;
        }

    }

    public static class IocHelper
    {
    }

}
