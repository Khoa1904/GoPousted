using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.SalesMng.ViewModels;
using GoPOS.Service.Service;
using GoPOS.Services;
using static GoPOS.Function;
using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.DirectoryServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;


/*
 영업관리 > 마감정산
 */

namespace GoPOS.ViewModels
{

    public class SalesClosingSettleSumViewModel : OrderPayChildViewModel
    {
        private readonly IPOSInitService pOSInitService;
        private readonly ISettAccountService settAccountService;
        private SETT_POSACCOUNT MiddleSettAccount = null;
        private IPOSPrintService posPrintService;
        private ISalesGiftSaleService saleService;

        public SalesClosingSettleSumViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISalesGiftSaleService saleService,
            IPOSInitService pOSInitService, ISettAccountService settAccountService, IPOSPrintService pOSPrintService) : base(windowManager, eventAggregator)
        {
            this.pOSInitService = pOSInitService;
            this.settAccountService = settAccountService;
            this.posPrintService = pOSPrintService;
            this.saleService = saleService;
            this.ViewLoaded += SalesClosingSettleSumViewModel_ViewLoaded;
        }

        private void SalesClosingSettleSumViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            //LastSettAccount = settAccountService.GetSingleAsync(DataLocals.PosStatus.SHOP_CODE, DataLocals.PosStatus.POS_NO,
            //            DataLocals.PosStatus.SALE_DATE, DataLocals.PosStatus.REGI_SEQ).Result;
            LoadData();
        }

        public string InitDateTime
        {
            get
            {
                var ci = Thread.CurrentThread.CurrentCulture;
                string[] names = ci.DateTimeFormat.DayNames;
                var openDt = DateTime.ParseExact(MiddleSettAccount.OPEN_DT, "yyyyMMddHHmmss",
                    Thread.CurrentThread.CurrentCulture);
                string dn = names[(int)ci.Calendar.GetDayOfWeek(openDt)];
                return string.Format("개시시간:  {0:yyyy-MM-dd HH:mm:ss} {1}", openDt, dn);
            }
        }

        private bool createNewMiddle = false;
        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            MiddleSettAccount = (SETT_POSACCOUNT)datas[1]; // 중간정산 middle data
            createNewMiddle = (bool)datas[2];
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(button =>
        {
            switch (button.Tag.ToString())
            {
                case "SalesClose":
                    var res = DialogHelper.MessageBox("마감정산 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }

                    pOSInitService.DoCloseSettAccount(MiddleSettAccount, createNewMiddle, false);
                    DialogHelper.MessageBox("마감정산 완료 되었습니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.MainWindow.Close();
                    break;
                case "PrevPage":
                    _eventAggregator.PublishOnUIThreadAsync(new SalesMngMainViewEventArgs()
                    {
                        EventType = "OpenVM",
                        EventData = "SalesMiddleExcClcViewModel-FCLOSE"
                    });
                    this.DeactivateClose(true);
                    break;
                case "PrintSalesClose":
                    var closeSettAccount = pOSInitService.DoCloseSettAccount(MiddleSettAccount, createNewMiddle, true);

                    bool printSettle = true;
                    if (DataLocals.AppConfig.PosOption.SettlePrintFlag == "2")
                    {
                        var printRes = DialogHelper.MessageBox("정산지 출력하시곘습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                        printSettle = printRes == MessageBoxResult.OK;
                    }

                    if (!printSettle || DataLocals.AppConfig.PosOption.SettlePrintFlag == "1")
                    {
                        break;
                    }

                    posPrintService.PrintCloseSettle(Common.PrinterLib.PrintOptions.Normal, closeSettAccount);
                    posPrintService.EndPrinting();
                    break;
                default:
                    break;
            }
        });

        private List<FINAL_SETT> finalss = null;
        private List<CLOSE_DETAILS1> close1 = null;
        private List<CLOSE_DETAILS2> close2 = null;
        private List<SALE_BY_TYPE> salebyType = null;
        private List<SALE_DISCOUNT> saleDis = null;
        private List<NSALE_RECORD> noSale = null;
        private List<MEMBER_SALE> memberS = null;
        private List<CR_CARD_SALE> crCardSale = null;
        private decimal totalSale1 = 0;
        private decimal totalSaleType = 0;
        private decimal totalDis = 0;

        public List<FINAL_SETT>? FINAL
        {
            get => finalss;
            set
            {
                finalss = value;
                NotifyOfPropertyChange(() => FINAL);
            }
        }
        public List<CLOSE_DETAILS1>? CLOSE_DTL1
        {
            get => close1;
            set
            {
                close1 = value;
                NotifyOfPropertyChange(() => CLOSE_DTL1);
            }
        }
        public List<CLOSE_DETAILS2>? CLOSE_DTL2
        {
            get => close2;
            set
            {
                close2 = value;
                NotifyOfPropertyChange(() => CLOSE_DTL2);
            }
        }
        public List<SALE_BY_TYPE>? SALEBTYPE
        {
            get => salebyType;
            set
            {
                salebyType = value;
                NotifyOfPropertyChange(() => SALEBTYPE);
            }
        }
        public List<SALE_DISCOUNT>? SALE_DIS
        {
            get => saleDis;
            set
            {
                saleDis = value;
                NotifyOfPropertyChange(() => SALE_DIS);
            }
        }
        public List<NSALE_RECORD>? NO_SALE
        {
            get => noSale;
            set
            {
                noSale = value;
                NotifyOfPropertyChange(() => NO_SALE);
            }
        }
        public List<MEMBER_SALE>? MEMBER
        {
            get => memberS;
            set
            {
                memberS = value;
                NotifyOfPropertyChange(() => MEMBER);
            }
        }
        public List<CR_CARD_SALE>? CR_SALE
        {
            get => crCardSale;
            set
            {
                crCardSale = value;
                NotifyOfPropertyChange(() => CR_SALE);
            }
        }
        public Decimal TOTAL_SALE1
        {
            get => totalSale1;
            set
            {
                totalSale1 = value;
                NotifyOfPropertyChange(() => TOTAL_SALE1);
            }
        }
        public Decimal TOTAL_SALE_TYPE
        {
            get => totalSaleType;
            set
            {
                totalSaleType = value;
                NotifyOfPropertyChange(() => TOTAL_SALE_TYPE);
            }
        }
        public Decimal TOTAL_DIS
        {
            get => totalDis;
            set
            {
                totalDis = value;
                NotifyOfPropertyChange(() => TOTAL_DIS);
            }
        }
        private void InitData()
        {

        }

        private void LoadData()
        {
            FINAL = saleService.GetFinalClosing(MiddleSettAccount.REGI_SEQ, /*MiddleSettAccount.CLOSE_FLAG,*/ MiddleSettAccount.SALE_DATE).Result.Item1;
   
            CLOSE_DETAILS1 grossSale   = new () { Item = "총판매액",  value = FINAL[0].GROSS_SALE };
            CLOSE_DETAILS1 retSale     = new () { Item = "취소매출액",  value = FINAL[0].RET_BILL_AMT };
            CLOSE_DETAILS1 netSale     = new () { Item = "총매출액",    value = FINAL[0].TOT_SALE_AMT };
            CLOSE_DETAILS1 disAmt      = new () { Item = "총할인액",     value = FINAL[0].TOT_DC_AMT };
            CLOSE_DETAILS1 actSale     = new () { Item = "실매출액",    value = FINAL[0].DCM_SALE_AMT };
            CLOSE_DETAILS1 notaxedSale = new () { Item = "면세매출액", value = FINAL[0].NO_VAT_SALE_AMT }; //면세매출액추가
            CLOSE_DETAILS1 taxedSale   = new () { Item = "과세매출액", value = FINAL[0].VAT_SALE_AMT };
            CLOSE_DETAILS1 taxAmt      = new () { Item = "부가세액",     value = FINAL[0].VAT_AMT };           

            CLOSE_DETAILS2 saleCount    = new () { Item = "매출건수   ", value = FINAL[0].TOT_BILL_CNT };
            CLOSE_DETAILS2 retCount     = new () { Item = "취소매출건수 ", value = FINAL[0].RET_BILL_CNT };
            CLOSE_DETAILS2 vstCount     = new () { Item = "총방문객수 ", value = FINAL[0].VISIT_CST_CNT };
            CLOSE_DETAILS2 netsaleCount = new () { Item = "실매출건수 ", value = FINAL[0].NET_BILL_CNT };

            SALE_BY_TYPE cashSale    = new () { Item = "현금      ", value = FINAL[0].CASH_AMT};
            SALE_BY_TYPE cardSale    = new () { Item = "신용카드  ", value = FINAL[0].CRD_CARD_AMT };
            SALE_BY_TYPE wesSale     = new () { Item = "외상      ", value = FINAL[0].WES_AMT };
            SALE_BY_TYPE voucherSale = new () { Item = "상품권    ", value = FINAL[0].TK_GFT_AMT };
            SALE_BY_TYPE onlSale     = new () { Item = "온라인매출 ", value = FINAL[0].O2O_AMT };
            SALE_BY_TYPE joincSale   = new () { Item = "제휴카드   ", value = FINAL[0].JCD_CARD_AMT };
            SALE_BY_TYPE memPtsSale  = new () { Item = "회원포인트 ", value = FINAL[0].CST_POINT_AMT };
            SALE_BY_TYPE mealTkSale  = new () { Item = "식권      ", value = FINAL[0].TK_FOD_AMT };
            SALE_BY_TYPE rfcSale     = new () { Item = "사원카드   ", value = FINAL[0].RFC_AMT };
            SALE_BY_TYPE simpleSale  = new () { Item = "간편결제   ", value = FINAL[0].SP_PAY_AMT };
            SALE_BY_TYPE egiftSale   = new () { Item = "전자상품권 ", value = FINAL[0].EGIFT_AMT };
            SALE_BY_TYPE pPcCard     = new () { Item = "선결제     ", value = FINAL[0].PPC_CARD_AMT }; //선결제추가

            SALE_DISCOUNT genDC     = new () { Item = "일반     ", value = FINAL[0].DC_GEN_AMT };
            SALE_DISCOUNT serviceDC = new () { Item = "서비스  ", value = FINAL[0].DC_SVC_AMT };
            SALE_DISCOUNT joincDC   = new () { Item = "제휴카드  ", value = FINAL[0].DC_JCD_AMT };
            SALE_DISCOUNT couponDC  = new () { Item = "쿠폰     ", value = FINAL[0].DC_CPN_AMT };
            SALE_DISCOUNT memberDC  = new () { Item = "회원     ", value = FINAL[0].DC_CST_AMT };
            SALE_DISCOUNT tkDC      = new () { Item = "식권     ", value = FINAL[0].DC_TFD_AMT };
            SALE_DISCOUNT promoDC   = new () { Item = "프로모션  ", value = FINAL[0].DC_PRM_AMT };
            SALE_DISCOUNT CrcDC     = new () { Item = "신용카드 ", value = FINAL[0].DC_CRD_AMT };
            SALE_DISCOUNT packDC    = new () { Item = "포장할인 ", value = FINAL[0].DC_PACK_AMT };

            NSALE_RECORD saleAmt = null; // new () { Item = "실매출금액", value = Comma(FINAL[0].TOT_DC_AMT) }; 비매출주석 2023.11.13
            NSALE_RECORD payType = null; // new () { Item = "결제수단구분", value = FINAL[0].PAY_TYPE_NAME }; 비매출주석 2023.11.13
            NSALE_RECORD preSaleCard = new () { Item = "션결제충전", value = FINAL[0].PRE_PNT_SALE_CRD_AMT }; //선결제카드충전 추가

            MEMBER_SALE msaleCount  = new () { Item = "총영수건수", value = FINAL[0].TOT_BILL_CNT };
            MEMBER_SALE mvstCount   = new () { Item = "방문손님수", value = FINAL[0].VISIT_CST_CNT };
            MEMBER_SALE memPtsCount = new () { Item = "결제건수 - 회원포인트", value = FINAL[0].CST_POINT_CNT };
            MEMBER_SALE memPtsAmt   = new () { Item = "결제액 - 회원포인트", value = FINAL[0].CST_POINT_AMT };
            MEMBER_SALE accmCount   = new () { Item = "적립건수 - 회원포인트", value = 0 };
            MEMBER_SALE accmPts     = new () { Item = "적립액 - 회원포인트", value = 0 };

            CLOSE_DTL1 = new List<CLOSE_DETAILS1>();
            CLOSE_DTL1.Add(grossSale);
            CLOSE_DTL1.Add(retSale);
            CLOSE_DTL1.Add(netSale);
            CLOSE_DTL1.Add(disAmt);
            CLOSE_DTL1.Add(actSale);
            CLOSE_DTL1.Add(notaxedSale); //면세매출액 추가
            CLOSE_DTL1.Add(taxedSale);
            CLOSE_DTL1.Add(taxAmt);
            //TOTAL_SALE1 = CLOSE_DTL1[2].value; //순매출액 계산수정 총매출액 - 부가세 - 할인액 : 여긴주석처리

            CLOSE_DTL2 = new List<CLOSE_DETAILS2>();
            CLOSE_DTL2.Add(saleCount);
            CLOSE_DTL2.Add(retCount);
            CLOSE_DTL2.Add(vstCount);
            CLOSE_DTL2.Add(netsaleCount);

            SALEBTYPE = new List<SALE_BY_TYPE>();
            SALEBTYPE.Add(cashSale);
            SALEBTYPE.Add(cardSale);
            SALEBTYPE.Add(wesSale);
            SALEBTYPE.Add(voucherSale);
            SALEBTYPE.Add(onlSale);
            SALEBTYPE.Add(joincSale);
            SALEBTYPE.Add(memPtsSale);
            SALEBTYPE.Add(mealTkSale);
            SALEBTYPE.Add(rfcSale);
            SALEBTYPE.Add(simpleSale);
            SALEBTYPE.Add(egiftSale); 
            SALEBTYPE.Add(pPcCard); //선결제추가
            TOTAL_SALE_TYPE = 0;
            for (int i = 0; i < SALEBTYPE.Count; i++)
            {
                TOTAL_SALE_TYPE += SALEBTYPE[i].value;
            }

            SALE_DIS = new List<SALE_DISCOUNT>();
            SALE_DIS.Add(genDC);
            SALE_DIS.Add(serviceDC);
            SALE_DIS.Add(joincDC);
            SALE_DIS.Add(couponDC);
            SALE_DIS.Add(memberDC);
            SALE_DIS.Add(tkDC);
            SALE_DIS.Add(promoDC);
            SALE_DIS.Add(CrcDC);
            SALE_DIS.Add(packDC);
            TOTAL_DIS = 0;
            for (int i=0;  i < SALE_DIS.Count; i++)
            {
                TOTAL_DIS += SALE_DIS[i].value;
            }
            //순매출액 계산수정 총매출액 - 부가세 - 할인액 : 여기서 계산
            TOTAL_SALE1 = CLOSE_DTL1[2].value - CLOSE_DTL1[7].value - TOTAL_DIS;

            NO_SALE = new List<NSALE_RECORD>();
            //NO_SALE.Add(saleAmt); 비매출주석 2023.11.13
            //NO_SALE.Add(payType); 비매출주석 2023.11.13
            NO_SALE.Add(preSaleCard); //선결제카드충전추가

            MEMBER = new List<MEMBER_SALE>();
            MEMBER.Add(msaleCount);
            MEMBER.Add(mvstCount);
            MEMBER.Add(memPtsCount);
            MEMBER.Add(memPtsAmt);
            MEMBER.Add(accmCount);
            MEMBER.Add(accmPts);

            CR_SALE = new List<CR_CARD_SALE>();
            CR_SALE = saleService.GetCardDistinct(MiddleSettAccount.REGI_SEQ, /*MiddleSettAccount.CLOSE_FLAG,*/ MiddleSettAccount.SALE_DATE).Result.Item1;
        }
    }
}