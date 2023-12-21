using FirebirdSql.Data.FirebirdClient;
using Google.Protobuf.WellKnownTypes;
using GoPOS.Common.Helpers;
using GoPOS.Common.Service;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Service;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Printing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Resources;
using static GoPosVanAPI.Api.VanAPI;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace GoPOS.Common.PrinterLib
{
    public class PrintTemplateProcess
    {
        #region Declare Properties
        private bool _isPrintItem { get; set; } = false;
        private bool _isRePrint { get; set; } = false;
        private bool _isPrintReturn { set { if (value) _isReturn = "-"; } }
        private string _isReturn { get; set; }
        private TRN_PRDT[] _arrPrdt { get; set; }
        private PAYMENT _payMent { get; set; }
        private TRN_HEADER _trnHeader { get; set; }
        private NTRN_PRECHARGE_HEADER _ntrnPreCHARGE { get; set; }
        private NTRN_PRECHARGE_CARD[] _ntrnPrechargeCards { get; set; }
        private TRN_CASHREC[] _trnCashRecs { get; set; }
        private TRN_CARD[] _arrTrnCards { get; set; }
        private TRN_GIFT[] _arrTrnGift { get; set; }
        private TRN_CASH[] _arrTrnCash { get; set; }
        private TRN_TENDERSEQ[] _arrTenderSeq { get; set; }
        private TRN_EASYPAY[] _arrTrnEasyPay { get; set; }
        private TRN_FOODCPN[] _arrTrnFoodCpn { get; set; }
        private TRN_PARTCARD[] _arrTrnPartCard { get; set; }
        private SETT_POSACCOUNT _setPosAccount { get; set; }
        private TRN_POINTUSE[] _trnPointUse { get; set; }
        private TRN_POINTSAVE _trnPointSave { get; set; }
        private TRN_PPCARD[] _arrPpCards { get; set; }
        private MEMBER_CLASH _MemberInfo { get; set; }

        private PRINT_TITLES _printTitle = PRINT_TITLES.RECEIPT;
        #endregion
        public PrintTemplateProcess()
        {
        }

        #region Print Form Handling Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptForm"></param>
        /// <param name="isPrintItem"></param>
        /// <param name="isRePrint"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string GetReceiptText(string rptForm, bool isPrintItem, bool isRePrint, object[] datas)
        {
            _trnHeader = (TRN_HEADER)datas[0];
            //_orderSumInfo = (ORDER_SUM_INFO)datas[1];
            _arrPrdt = (TRN_PRDT[])datas[2];
            _arrTenderSeq = (TRN_TENDERSEQ[])datas[3];
            _arrTrnCash = (TRN_CASH[])datas[4];
            _trnCashRecs = (TRN_CASHREC[])datas[5];
            _arrTrnCards = (TRN_CARD[])datas[6];
            _arrTrnGift = (TRN_GIFT[])datas[7];
            _arrTrnFoodCpn = (TRN_FOODCPN[])datas[8];
            _arrTrnEasyPay = (TRN_EASYPAY[])datas[9];
            _arrTrnPartCard = (TRN_PARTCARD[])datas[10];
            _setPosAccount = (SETT_POSACCOUNT)datas[11];
            _trnPointUse = datas.Length > 12 ? (TRN_POINTUSE[])datas[12] : null;
            _trnPointSave = datas.Length > 13 ? (TRN_POINTSAVE)datas[13] : null;
            _arrPpCards = datas.Length > 14 ? (TRN_PPCARD[])datas[14] : null;
            _MemberInfo = datas.Length > 15 ? (MEMBER_CLASH)datas[15] : null;
            _payMent = GetPayMent(_arrTrnCash, _trnCashRecs, _arrTrnCards, _arrTrnGift, _arrTrnFoodCpn, _arrTrnEasyPay, _arrTrnPartCard, _trnPointUse, _trnPointSave, _arrPpCards);
            _isPrintItem = isPrintItem;
            _isRePrint = isRePrint;

            if (_trnHeader != null)
                _isPrintReturn = _trnHeader.SALE_YN == "N" ? true : false;

            return ProcessTemplate(rptForm);
        }

        public string GetFoodCouponText(string rptForm, object[] datas)
        {
            _trnHeader = (TRN_HEADER)datas[0];
            //_orderSumInfo = (ORDER_SUM_INFO)datas[1];
            _arrPrdt = (TRN_PRDT[])datas[2];

            if (_trnHeader != null)
                _isPrintReturn = _trnHeader.SALE_YN == "N" ? true : false;

            return ProcessTemplate(rptForm);
        }

        // 고객주문서 추가
        public string GetCustomerOrderText(string rptForm, object[] datas)
        {
            _printTitle = PRINT_TITLES.ORDERLIST;
            _trnHeader = (TRN_HEADER)datas[0];
            //_orderSumInfo = (ORDER_SUM_INFO)datas[1];
            _arrPrdt = (TRN_PRDT[])datas[2];

            if (_trnHeader != null)
                _isPrintReturn = _trnHeader.SALE_YN == "N" ? true : false;

            return ProcessTemplate(rptForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMiddle"></param>
        /// <param name="rptForm"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string GetSettleText(bool isMiddle, string rptForm, object[] datas)
        {
            _printTitle = isMiddle ? PRINT_TITLES.MIDDLESETTLE : PRINT_TITLES.CLOSESETTLE;

            _trnHeader = (TRN_HEADER)datas[0];
            //_orderSumInfo = (ORDER_SUM_INFO)datas[1];
            _arrPrdt = (TRN_PRDT[])datas[2];
            _arrTenderSeq = (TRN_TENDERSEQ[])datas[3];
            _arrTrnCash = (TRN_CASH[])datas[4];
            _trnCashRecs = (TRN_CASHREC[])datas[5];
            _arrTrnCards = (TRN_CARD[])datas[6];
            _arrTrnGift = (TRN_GIFT[])datas[7];
            _arrTrnFoodCpn = (TRN_FOODCPN[])datas[8];
            _arrTrnEasyPay = (TRN_EASYPAY[])datas[9];
            _arrTrnPartCard = (TRN_PARTCARD[])datas[10];
            _setPosAccount = (SETT_POSACCOUNT)datas[11];
            _trnPointUse = datas.Length > 12 ? (TRN_POINTUSE[])datas[12] : null;
            _trnPointSave = datas.Length > 13 ? (TRN_POINTSAVE)datas[13] : null;
            _arrPpCards = datas.Length > 14 ? (TRN_PPCARD[])datas[14] : null;
            _MemberInfo = datas.Length > 15 ? (MEMBER_CLASH)datas[15] : null;
            _payMent = GetPayMent(_arrTrnCash, _trnCashRecs, _arrTrnCards, _arrTrnGift, _arrTrnFoodCpn, _arrTrnEasyPay, _arrTrnPartCard, _trnPointUse, _trnPointSave, _arrPpCards);
            if (_trnHeader != null)
                _isPrintReturn = _trnHeader.SALE_YN == "N" ? true : false;

            return ProcessTemplate(rptForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptForm"></param>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public string GetKitchenPrintText(string rptForm, string shopCode, string posNo, string saleDate, string billNo)
        {
            _printTitle = PRINT_TITLES.KITCHEN;

            // get trnHeader and product list of bill
            using (var db = new DataContext())
            {
                _trnHeader = db.tRN_HEADERs.FirstOrDefault(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                p.SALE_DATE == saleDate && p.BILL_NO == billNo);
                _arrPrdt = db.tRN_PRDTs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                p.SALE_DATE == saleDate && p.BILL_NO == billNo).OrderBy(p => p.SEQ_NO).ToArray();

                /// (from device in _deviceInfos?.Where(t => t.KDS_TYPE == ((char)EKDS_Type.KDS).ToString()).ToList()
                //join product in _productDevices on device.KDS_NO equals product.KDS_NO
                //select product).ToList();
                //_arrPrdt = (from trnPrdt in db.tRN_PRDTs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                //                p.SALE_DATE == saleDate && p.BILL_NO == billNo).OrderBy(p => p.SEQ_NO).ToList()
                //           join mstPrdt in db.mST_INFO_PRODUCTs on trnPrdt.PRD_CODE equals mstPrdt.PRD_CODE
                //           select trnPrdt).ToArray();
                if (_arrPrdt != null && _arrPrdt.Any())
                {
                    //string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, "GetProducts");
                    //var _productInfo = DapperORM.ReturnListAsync<Models.Common.ProductInfo>(sql, null).Result;
                    //if (_productInfo != null)

                    foreach (var item in _arrPrdt)
                    {
                        var pro = db.mST_INFO_PRODUCTs.FirstOrDefault(t => t.SHOP_CODE == shopCode && t.PRD_CODE == item.PRD_CODE);
                        if (pro != null)
                        {
                            item.PRD_NAME = pro.PRD_NAME;
                        }
                    }
                }
            }

            return ProcessTemplate(rptForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trHeader"></param>
        /// <param name="trProducts"></param>
        /// <returns></returns>
        public string GetKitchenPrintText(string rptForm, TRN_HEADER trHeader, TRN_PRDT[] trProducts)
        {
            _printTitle = PRINT_TITLES.KITCHEN;
            _trnHeader = trHeader;
            _arrPrdt = trProducts;
            if (_arrPrdt != null && _arrPrdt.Any())
            {
                using (var db = new DataContext())
                {
                    //string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, "GetProducts");
                    //var _productInfo = DapperORM.ReturnListAsync<Models.Common.ProductInfo>(sql, null).Result;
                    //if (_productInfo != null)
                    //{
                    //    foreach (var item in _arrPrdt)
                    //    {
                    //        var pro = _productInfo.FirstOrDefault(t => t.SHOP_CODE == trHeader.SHOP_CODE && t.PRD_CODE == item.PRD_CODE);
                    //        if (pro != null)
                    //        {
                    //            item.PRD_NAME = pro.PRD_NAME;
                    //        }
                    //    }
                    //}
                    foreach (var item in _arrPrdt)
                    {
                        var pro = db.mST_INFO_PRODUCTs.FirstOrDefault(t => t.SHOP_CODE == trHeader.SHOP_CODE && t.PRD_CODE == item.PRD_CODE);
                        if (pro != null)
                        {
                            item.PRD_NAME = pro.PRD_NAME;
                        }
                    }
                }
            }

            return ProcessTemplate(rptForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trHeader"></param>
        /// <param name="trProducts"></param>
        /// <returns></returns>
        public string GetPrePaymentInputPrintText(string rptForm, NTRN_PRECHARGE_HEADER trHeader, NTRN_PRECHARGE_CARD[] trProducts, bool isRePrint)
        {
            _printTitle = PRINT_TITLES.PREPAYMENTINPUT;
            _ntrnPreCHARGE = trHeader;
            _ntrnPrechargeCards = trProducts;
            _isRePrint = isRePrint;

            if (_arrPrdt != null && _arrPrdt.Any())
            {
                using (var db = new DataContext())
                {
                    //string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, "GetProducts");
                    //var _productInfo = DapperORM.ReturnListAsync<Models.Common.ProductInfo>(sql, null).Result;
                    //if (_productInfo != null)
                    //{
                    //    foreach (var item in _arrPrdt)
                    //    {
                    //        var pro = _productInfo.FirstOrDefault(t => t.SHOP_CODE == trHeader.SHOP_CODE && t.PRD_CODE == item.PRD_CODE);
                    //        if (pro != null)
                    //        {
                    //            item.PRD_NAME = pro.PRD_NAME;
                    //        }
                    //    }
                    //}
                    //foreach (var item in _arrPrdt)
                    //{
                    //    var pro = db.nTRN_PRECHARGE_CARDs.FirstOrDefault(t => t.SHOP_CODE == trHeader.SHOP_CODE && t.PRD_CODE == item.PRD_CODE);
                    //    if (pro != null)
                    //    {
                    //        item.PRD_NAME = pro.PRD_NAME;
                    //    }
                    //}
                }
            }

            return ProcessTemplate2(rptForm);
        }

        /// <summary>
        /// 시재점검
        /// </summary>
        /// <param name="rptForm"></param>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="regiSeq"></param>
        /// <returns></returns>
        public string GetTrialCheckPrintText(string rptForm, string shopCode, string posNo, string saleDate, string regiSeq)
        {
            _printTitle = PRINT_TITLES.TRIALCHECK;

            // get trnHeader and product list of bill
            using (var db = new DataContext())
            {
                _setPosAccount = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq);
            }

            return ProcessTemplate(rptForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptForm"></param>
        /// <returns></returns>
        private string ProcessTemplate(string rptForm)
        {
            string printText = string.Empty;
            foreach (KeyValuePair<string, string> entry in FieldMapping.dicField)
            {
                string key = entry.Key;
                string value = entry.Value;
                var pattern = "{" + key + "}";
                rptForm = rptForm.Replace(pattern, "{" + value + "}");
            }

            try
            {
                printText = Regex.Replace(rptForm, @"\{(.*?)\}", match =>
                {
                    string key = match.Groups[1].Value;
                    string value = string.Empty;
                    //LogHelper.Logger.Trace("Key-" + key);
                    switch (key)
                    {
                        case "TITLE":
                            return PrintTitle();
                        case "PrintHeader":
                            return PrintHeader(_isPrintItem);
                        case "PrintPayment":
                            return PrintPayment();
                        case "PrintVAT":
                            return PrintVAT();
                        case "PrintCashBill":
                            return PrintCashBill();
                        case "PrintCreditCard":
                            return PrintCreditCard();
                        case "PrintItemList1":
                            return PrintItemList1(_isPrintItem);
                        case "PrintItemList2":
                            return PrintItemList2(_isPrintItem);
                        case "PrintCornerItems":
                            return PrintCornerItems();
                        case "PrintMemberInfo":
                            return PrintMemberInfo();
                        case "PrintPushchaseInfo":
                            return PrintPushchaseInfo();
                        case "PrintSimpleCard":
                            return PrintSimpleCard();
                        case "Menu3":
                            return PrintKitchenMenu3();
                        case "PrintSaleGift":
                            return PrintSaleGift();
                        case "PrintNonSaleSettlement":
                            return PrintNonSaleSettlement();
                        case "PrintGiftCard":
                            return PrintGiftCard();
                        case "PrintMemberPointInfo":
                            return PrintMemberPoint();
                        case "PrintSaleByCard":
                            return PrintSaleByCard();
                        case "PrintCreditCardSettlement":
                            return PrintCreditCardSettlement();
                        case "PrintCashSettlement":
                            return PrintCashSettlement();
                        case "PrintCashBalance":
                            return PrintCashBalance();
                        case "PrintDiscountSales":
                            return PrintDiscountSales();
                        case "PaymentMethod":
                            return PaymentMethod();
                        case "PrintSaleTotal":
                            return PrintSaleTotal();
                        case "PrintTrialCheck": // 시재점검
                            return PrintTrialCheck();
                        case "PrintGiftInfo":
                            return PrintGiftInfo();
                        case "PrintTicketFood":
                            return PrintTicketFood();
                        case "CustOrderMenuList"://고객주문서
                            return PrintCustOrderList();
                        default:
                            return FormatHeaderField(key);
                    }
                });
                return printText;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                return string.Empty;
            }
        }  /// <summary>
           /// 
           /// </summary>
           /// <param name="rptForm"></param>
           /// <returns></returns>
        private string ProcessTemplate2(string rptForm)
        {
            string printText = string.Empty;
            foreach (KeyValuePair<string, string> entry in FieldMapping.dicField)
            {
                string key = entry.Key;
                string value = entry.Value;
                var pattern = "{" + key + "}";
                rptForm = rptForm.Replace(pattern, "{" + value + "}");
            }

            try
            {
                printText = Regex.Replace(rptForm, @"\{(.*?)\}", match =>
                {
                    string key = match.Groups[1].Value;
                    string value = string.Empty;
                    //LogHelper.Logger.Trace("Key-" + key);
                    switch (key)
                    {
                        case "TITLE":
                            return PrintTitle();
                        case "PrintHeader":
                            return PrintHeader(_isPrintItem);
                        case "PrintPayment":
                            return PrintPayment();
                        case "PrintVAT":
                            return PrintVAT();
                        case "PrintCashBill":
                            return PrintCashBill();
                        case "PrintCreditCard":
                            return PrintCreditCard2();
                        case "PrintItemList1":
                            return PrintItemList1(_isPrintItem);
                        case "PrintItemList2":
                            return PrintItemList2(_isPrintItem);
                        case "PrintCornerItems":
                            return PrintCornerItems();
                        case "PrintMemberInfo":
                            return PrintMemberInfo();
                        case "PrintPushchaseInfo":
                            return PrintPushchaseInfo();
                        case "PrintSimpleCard":
                            return PrintSimpleCard();
                        case "Menu3":
                            return PrintKitchenMenu3();
                        case "PrintSaleGift":
                            return PrintSaleGift();
                        case "PrintNonSaleSettlement":
                            return PrintNonSaleSettlement();
                        case "PrintGiftCard":
                            return PrintGiftCard();
                        case "PrintMemberPointInfo":
                            return PrintMemberPoint();
                        case "PrintSaleByCard":
                            return PrintSaleByCard();
                        case "PrintCreditCardSettlement":
                            return PrintCreditCardSettlement();
                        case "PrintCashSettlement":
                            return PrintCashSettlement();
                        case "PrintCashBalance":
                            return PrintCashBalance();
                        case "PrintDiscountSales":
                            return PrintDiscountSales();
                        case "PaymentMethod":
                            return PaymentMethod();
                        case "PrintSaleTotal":
                            return PrintSaleTotal();
                        case "PrintTrialCheck": // 시재점검
                            return PrintTrialCheck();
                        case "PrintGiftInfo":
                            return PrintGiftInfo();
                        case "PrintTicketFood":
                            return PrintTicketFood();
                        case "CustOrderMenuList"://고객주문서
                            return PrintCustOrderList();
                        case "Reprint":
                            return _isRePrint == false ? "" : "";
                        case "ReprintYN":
                            return "";
                        default:
                            return FormatHeaderField2(key);
                    }
                });
                return printText;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                return string.Empty;
            }
        }

        private string PrintCreditCard2()
        {
            string CreditCard = string.Empty;
            if (_ntrnPrechargeCards == null || _ntrnPrechargeCards.Length == 0) return CreditCard;
            CreditCard += "         ** 신용승인정보(고객용) **       " + Environment.NewLine;
            foreach (NTRN_PRECHARGE_CARD ntrnPrechargeCard in _ntrnPrechargeCards)
            {
                CreditCard += " " + Environment.NewLine;
                CreditCard += "카 드 종 류:{:R42}" + (!string.IsNullOrEmpty(_ntrnPrechargeCards[0].ISS_CRDCP_NAME) ? _ntrnPrechargeCards[0].ISS_CRDCP_NAME : "  단말기 승인 후 임의등록  ") + Environment.NewLine;
                CreditCard += "카 드 번 호:{:R42}" + (!string.IsNullOrEmpty(_ntrnPrechargeCards[0].CRD_CARD_NO) ? _ntrnPrechargeCards[0].CRD_CARD_NO : "  -**-****- ") + Environment.NewLine;
                CreditCard += "승 인 금 액: " + "   일시불" + "{:R42}" + (_ntrnPrechargeCards[0].APPR_AMT > 0 ? _isReturn : "") + (_isRePrint == true ? (_ntrnPrechargeCards[0].APPR_AMT * -1).ToString("N0") : _ntrnPrechargeCards[0].APPR_AMT.ToString("N0")) + Environment.NewLine;
                CreditCard += "승 인 번 호:{:R42}" + _ntrnPrechargeCards[0].APPR_NO + Environment.NewLine;
                CreditCard += "승 인 일 시:{:R42}" + _ntrnPrechargeCards[0].APPR_DATE.FormatDateString() + " " + _ntrnPrechargeCards[0].APPR_TIME.FormatTimeString() + Environment.NewLine;
                CreditCard += "가맹점 번호:{:R42}" + _ntrnPrechargeCards[0].CRDCP_TERM_NO + Environment.NewLine;
            }
            CreditCard += "------------------------------------------" + Environment.NewLine;
            return CreditCard;
        }

        /// 
        /// </summary>
        /// <returns></returns>
        private string PrintTitle()
        {
            switch (_printTitle)
            {
                case PRINT_TITLES.RECEIPT:
                    return "영수증" + (_isRePrint ? "[재출력]" : "");
                case PRINT_TITLES.MIDDLESETTLE:
                    return "중강정산서";
                case PRINT_TITLES.CLOSESETTLE:
                    return "마감정산서";
                case PRINT_TITLES.KITCHEN:
                    return "[주방주문서]";
                case PRINT_TITLES.TRIALCHECK:
                    return "[시재점검]";
                case PRINT_TITLES.ORDERLIST:
                    return "[고객주문서]";
                case PRINT_TITLES.PREPAYMENTINPUT:
                    return "[선결제 충전]";
                default:
                    return "일반영수증";
            }
        }

        #endregion

        #region Print bill

        private string PrintHeader(bool isPrintItem)
        {
            if (_trnHeader == null) return string.Empty;
            string Header = string.Empty; // replace tất cả các cột còn lại trong header.
            return Header;
        }

        /// <summary>
        /// 결제항목
        /// </summary>
        /// <returns></returns>
        private string PrintPayment()
        {
            string Payment = string.Empty;
            if (_payMent == null) return Payment;
            if (_payMent.PAY_AMT >= 0)
            {
                Payment += "현      금{:R42}" + (_payMent.PAY_AMT >= 0 ? _isReturn : "") + _payMent.PAY_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.CRD_CARD_AMT >= 0)
            {
                Payment += "신용  카드{:R42}" + (_payMent.CRD_CARD_AMT >= 0 ? _isReturn : "") + _payMent.CRD_CARD_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.TK_GFT_AMT >= 0)
            {
                Payment += "상  품  권{:R42}" + (_payMent.TK_GFT_AMT >= 0 ? _isReturn : "") + _payMent.TK_GFT_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.TK_FOD_AMT >= 0)
            {
                Payment += "식      권{:R42}" + (_payMent.TK_FOD_AMT >= 0 ? _isReturn : "") + _payMent.TK_FOD_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.JCD_CARD_AMT >= 0)
            {
                Payment += "제휴  카드{:R42}" + (_payMent.JCD_CARD_AMT >= 0 ? _isReturn : "") + _payMent.JCD_CARD_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.RFC_AMT >= 0)
            {
                Payment += "사원  카드{:R42}" + (_payMent.RFC_AMT >= 0 ? _isReturn : "") + _payMent.RFC_AMT.ToString("N0") + Environment.NewLine;
            }
            if (_payMent.EASY_PAY_AMT >= 0)
            {
                Payment += "간편  결제{:R42}" + (_payMent.EASY_PAY_AMT >= 0 ? _isReturn : "") + _payMent.EASY_PAY_AMT.ToString("N0") + Environment.NewLine;
                // Comment tempo 
                Payment += new string('-', 42) + Environment.NewLine;
                foreach (var easyItem in _payMent.EASY_PAY_AMTs)
                {
                    Payment += easyItem.PAY_NAME + "{:R42}" + (easyItem.PAY_AMT >= 0 ? _isReturn : "") + easyItem.PAY_AMT.ToString("N0") + Environment.NewLine;
                }
            }
            if (_payMent.CST_POINT_AMT > 0 || _payMent.CST_STAMP_AMT > 0)
            {
                decimal pntStampAmt = _payMent.CST_POINT_AMT > 0 ? _payMent.CST_POINT_AMT : _payMent.CST_STAMP_AMT;
                Payment += (_payMent.CST_POINT_AMT > 0 ? "포인트금액{:R42}" : "스탬프금액{:R42}") + (pntStampAmt >= 0 ? _isReturn : "") + pntStampAmt.ToString("N0") + Environment.NewLine;
            }


            if (_payMent.PPC_PAY_AMT >= 1)
            {
                Payment += "선  결  제{:R42}" + (_payMent.PPC_PAY_AMT >= 0 ? _isReturn : "") + _payMent.PPC_PAY_AMT.ToString("N0") + Environment.NewLine;
            }
            return Payment;
        }

        private string PrintVAT()
        {
            string Vat = string.Empty;
            if (_trnHeader == null) return Vat;
            Vat += "*부가세 면세물품가액{:R42}" + (_trnHeader.NO_VAT_SALE_AMT > 0 ? _isReturn : "") + _trnHeader.NO_VAT_SALE_AMT.ToString("N0") + Environment.NewLine;
            Vat += " 부가세 과세물품가액{:R42}" + (_trnHeader.VAT_SALE_AMT > 0 ? _isReturn : "") + (_trnHeader.VAT_SALE_AMT).ToString("N0") + Environment.NewLine;
            Vat += " 부       가      세{:R42}" + (_trnHeader.VAT_AMT > 0 ? _isReturn : "") + _trnHeader.VAT_AMT.ToString("N0") + Environment.NewLine;
            return Vat;
        }

        private string PrintCashBill()
        {
            string CashBill = string.Empty;
            if (_trnCashRecs == null || _trnCashRecs.Length == 0) return CashBill;
            CashBill += "      *** 현금 영수증 (소득공제) ***      " + Environment.NewLine;
            foreach (TRN_CASHREC _trnCashRec in _trnCashRecs)
            {
                CashBill += "확 인  번 호:{:R42}" + _trnCashRec.APPR_IDT_NO + Environment.NewLine;
                CashBill += "승 인  번 호:{:R42}" + _trnCashRec.APPR_NO + " " + (_trnCashRec.APPR_AMT > 0 ? _isReturn : "") + _trnCashRec.APPR_AMT.ToString("N0") + Environment.NewLine;
            }
            CashBill += "------------------------------------------" + Environment.NewLine;
            CashBill += "알       림 : 현금 영수증 문의 (126)      " + Environment.NewLine;
            CashBill += "홈 페 이 지 : http://현금영수증.kr        " + Environment.NewLine;
            CashBill += "------------------------------------------" + Environment.NewLine;
            return CashBill;
        }

        private string PrintCreditCard()
        {
            string CreditCard = string.Empty;
            if (_arrTrnCards == null || _arrTrnCards.Length == 0) return CreditCard;
            CreditCard += "         *** 신용승인정보(고객용) ***       " + Environment.NewLine;
            foreach (TRN_CARD _arrTrnCard in _arrTrnCards)
            {
                CreditCard += " " + Environment.NewLine;
                CreditCard += "카 드 종 류:{:R42}" + (!string.IsNullOrEmpty(_arrTrnCard.ISS_CRDCP_NAME) ? _arrTrnCard.ISS_CRDCP_NAME : " * 단말기 승인 후 임의등록 * ") + Environment.NewLine;
                CreditCard += "카 드 번 호:" + (!string.IsNullOrEmpty(_arrTrnCard.CRD_CARD_NO) ? _arrTrnCard.CRD_CARD_NO : "  -**-****- ") + Environment.NewLine;
                CreditCard += "승 인 금 액: " + "   일시불" + "{:R42}" + (_arrTrnCard.APPR_AMT > 0 ? _isReturn : "") + _arrTrnCard.APPR_AMT.ToString("N0") + Environment.NewLine;
                CreditCard += "승 인 번 호:{:R42}" + _arrTrnCard.APPR_NO + Environment.NewLine;
                CreditCard += "승 인 일 시:{:R42}" + _arrTrnCard.APPR_DATE.FormatDateString() + " " + _arrTrnCard.APPR_TIME.FormatTimeString() + Environment.NewLine;
                CreditCard += "가맹점 번호:{:R42}" + _arrTrnCard.CRDCP_TERM_NO + Environment.NewLine;
            }
            CreditCard += "------------------------------------------" + Environment.NewLine;
            return CreditCard;
        }

        private string PrintGiftInfo()
        {
            string GiftInfo = string.Empty;
            decimal dGftUAmt = 0;
            decimal dDftAmt = 0;
            decimal dRepayCshAmt = 0;
            decimal dRepayGftAmt = 0;

            if (_arrTrnGift == null || _arrTrnGift.Length == 0) return GiftInfo;
            GiftInfo += "          *** 상 품 권 정 보 ***          " + Environment.NewLine;
            foreach (TRN_GIFT _arrTrnGift in _arrTrnGift)
            {
                dGftUAmt += _arrTrnGift.TK_GFT_UAMT;
                dDftAmt += _arrTrnGift.TK_GFT_AMT;
                dRepayCshAmt += _arrTrnGift.REPAY_CSH_AMT;
                dRepayGftAmt += _arrTrnGift.REPAY_GFT_AMT;
            }
            GiftInfo += "액 면  가 액:{:R42}" + (dGftUAmt > 0 ? _isReturn : "") + dGftUAmt.ToString("N0") + Environment.NewLine;
            GiftInfo += "결   제   액:{:R42}" + (dDftAmt > 0 ? _isReturn : "") + dDftAmt.ToString("N0") + Environment.NewLine;
            GiftInfo += "현금  환불액:{:R42}" + (dRepayCshAmt > 0 ? _isReturn : "") + dRepayCshAmt.ToString("N0") + Environment.NewLine;
            GiftInfo += "상품권환불액:{:R42}" + (dRepayGftAmt > 0 ? _isReturn : "") + dRepayGftAmt.ToString("N0") + Environment.NewLine;
            GiftInfo += "------------------------------------------" + Environment.NewLine;
            return GiftInfo;
        }

        private string PrintTicketFood()
        {
            string FoodTicket = string.Empty;
            decimal dFodUAmt = 0;
            decimal dFodAmt = 0;
            decimal dFodEtcAmt = 0;

            if (_arrTrnFoodCpn == null || _arrTrnFoodCpn.Length == 0) return FoodTicket;
            FoodTicket += "          *** 식  권  정  보 ***          " + Environment.NewLine;
            foreach (TRN_FOODCPN _arrTrnFoodCpn in _arrTrnFoodCpn)
            {
                dFodUAmt += _arrTrnFoodCpn.TK_FOD_UAMT;
                dFodAmt += _arrTrnFoodCpn.TK_FOD_AMT;
                dFodEtcAmt += _arrTrnFoodCpn.ETC_AMT;
            }
            FoodTicket += "액 면  가 액:{:R42}" + (dFodUAmt > 0 ? _isReturn : "") + dFodUAmt.ToString("N0") + Environment.NewLine;
            FoodTicket += "결   제   액:{:R42}" + (dFodAmt > 0 ? _isReturn : "") + dFodAmt.ToString("N0") + Environment.NewLine;
            FoodTicket += "자 투  리 액:{:R42}" + (dFodEtcAmt > 0 ? _isReturn : "") + dFodEtcAmt.ToString("N0") + Environment.NewLine;
            FoodTicket += "------------------------------------------" + Environment.NewLine;

            return FoodTicket;
        }

        /// <summary>
        /// 상품목록1
        /// </summary>
        /// <param name="isPrintItem"></param>
        /// <returns></returns>
        private string PrintItemList1(bool isPrintItem)
        {
            string ItemList = string.Empty;
            ItemList += "상품명               단가  수량       금액" + Environment.NewLine;
            ItemList += "------------------------------------------" + Environment.NewLine;
            if (!isPrintItem)
            {
                return ItemList;
            }
            else
            {
                if (_arrPrdt != null && _arrPrdt.Length > 0)
                {
                    string prdName = "";
                    foreach (TRN_PRDT prd in _arrPrdt)
                    {
                        prdName = GetPrdName(prd.PRD_CODE);
                        if (string.IsNullOrEmpty(prdName))
                        {
                            MST_INFO_PRODUCT pro = GetPrdProperties(prd.SDS_PARENT_CODE, prd.PRD_CODE);
                            if (pro != null)
                            {
                                prdName = pro.PRD_NAME;
                            }
                            else
                            {
                                prdName = "";
                            }
                        }

                        // CHANGE_DUTY ==>
                        ItemList += (prd.TAX_YN == "N" ? "*" : string.Empty) + prdName + Environment.NewLine;
                        ItemList += prd.PRD_CODE + "{:R42}" + prd.SALE_UPRC.ToString("N0").PadLeft(11) + ((prd.SALE_QTY > 0 ? _isReturn : "") + prd.SALE_QTY.ToString("N0")).PadLeft(6) + ((prd.SALE_AMT > 0 ? _isReturn : "") + prd.SALE_AMT.ToString("N0")).PadLeft(11) + Environment.NewLine;
                    }
                }
            }
            return ItemList;
        }

        /// <summary>
        /// 상품목록2
        /// </summary>
        /// <param name="isPrintItem"></param>
        /// <returns></returns>
        private string PrintItemList2(bool isPrintItem)
        {
            string ItemList = string.Empty;
            ItemList += "상품명               단가  수량       금액" + Environment.NewLine;
            ItemList += "------------------------------------------" + Environment.NewLine;
            if (!isPrintItem)
            {
                return ItemList;
            }
            else
            {
                if (_arrPrdt != null && _arrPrdt.Length > 0)
                {
                    string prdName = "";
                    foreach (TRN_PRDT prd in _arrPrdt)
                    {
                        prdName = "";
                        prdName = GetPrdName(prd.PRD_CODE);
                        if (string.IsNullOrEmpty(prdName))
                        {
                            MST_INFO_PRODUCT pro = GetPrdProperties(prd.SDS_PARENT_CODE, prd.PRD_CODE);
                            if (pro != null)
                            {
                                prdName = pro.PRD_NAME;
                            }
                            else
                            {
                                prdName = "";
                            }
                        }
                        prdName = (prd.TAX_YN == "N" ? "*" : string.Empty) + prdName;

                        if (prdName.Length > 10)
                        {
                            prdName = prdName.Substring(0, 10);
                        }
                        ItemList += prdName + "{:R42}" + prd.SALE_UPRC.ToString("N0").PadLeft(11) + ((prd.SALE_QTY > 0 ? _isReturn : "") + prd.SALE_QTY.ToString("N0")).PadLeft(6) + ((prd.SALE_AMT > 0 ? _isReturn : "") + prd.SALE_AMT.ToString("N0")).PadLeft(11) + Environment.NewLine;
                    }
                }
            }
            return ItemList;
        }

        private string PrintCornerItems()
        {
            StringBuilder sbPrint = new StringBuilder();
            foreach (var prd in _arrPrdt)
            {
                var prdName = GetPrdName(prd.PRD_CODE);
                if (string.IsNullOrEmpty(prdName))
                {
                    MST_INFO_PRODUCT pro = GetPrdProperties(prd.SDS_PARENT_CODE, prd.PRD_CODE);
                    if (pro != null)
                    {
                        prdName = pro.PRD_NAME;
                    }
                    else
                    {
                        prdName = "";
                    }
                }

                sbPrint.Append(prdName.Trim());
                sbPrint.Append("{:R42}");
                sbPrint.Append((prd.SALE_QTY > 0 ? _isReturn : "") + prd.SALE_QTY.ToString("N0"));
                sbPrint.AppendLine();
            }

            return sbPrint.ToString();
        }

        private string GetPrdName(string prdCode)
        {
            using (var db = new DataContext())
            {
                var prd = db.mST_INFO_PRODUCTs.FirstOrDefault(p => p.PRD_CODE == prdCode);
                return prd != null ? prd.PRD_NAME : string.Empty;
            }
        }

        private string GetSimplePayName(string cpPayCode)
        {
            using (var db = new DataContext())
            {
                var sPay = db.mST_INFO_EASYPAYs.FirstOrDefault(p => p.PAYCP_CODE == cpPayCode);
                return sPay == null ? "간편결제" : sPay.PAYCP_NAME;
            }
        }

        private MST_INFO_PRODUCT GetPrdProperties(string prdCode, string subCode)
        {
            try
            {
                string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetPrdProperties");
                List<MST_INFO_PRODUCT> result = DapperORM.ReturnListAsync<MST_INFO_PRODUCT>(SQL,
                    new string[]
                    {
                        "@SHOP_CODE",
                        "@PRD_CODE",
                        "@SUB_CODE"
                    },
                    new object[]
                    {
                        DataLocals.AppConfig.PosInfo.StoreNo,
                        prdCode,
                        subCode
                    }).Result;

                return result.FirstOrDefault();
            }
            catch (FbException ex)
            {
                return new MST_INFO_PRODUCT();
            }
            catch (Exception ex)
            {
                return new MST_INFO_PRODUCT();
            }
        }

        private string PrintMemberInfo()
        {
            if (_trnPointSave == null)
            {
                return string.Empty;
            }
            var check = _trnPointSave.SALE_YN;
            //var stampCheck = DataLocals.AppConfig.PosOption.StampUseMethod;
            var stampCheck = DataLocals.AppConfig.PosOption.PointStampFlag;
            decimal usePoint = _trnPointUse != null ? _trnPointUse.Sum(z => z.USE_PNT) : 0;
            decimal useStampQty = _trnPointUse != null ? _trnPointUse.Sum(z => z.USE_STAMP) : 0;
            decimal remainPoint = _trnPointUse != null &&_trnPointUse.Any() ? _trnPointUse[_trnPointUse.Count() - 1].TOT_PNT : (_trnPointSave != null ? _trnPointSave.TOT_PNT : 0);
            decimal savePoint = _trnPointSave != null ? _trnPointSave.SAVE_PNT : 0;
            string MemberInfo = string.Empty;
            string cstNo =  _trnPointSave.CST_NO;

            MemberInfo += "            *** 회원정보 ***              " + Environment.NewLine;
            MemberInfo += "회원번호        " + cstNo.PadLeft(10) + Environment.NewLine;
            MemberInfo += DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "적립포인트      잔여포인트      사용포인트" + Environment.NewLine :
                "적립스탬프      잔여스탬프      사용스탬프" + Environment.NewLine;
     //       MemberInfo += savePoint.ToString("N0").PadLeft(10) + remainPoint.ToString("N0").PadLeft(16) +
     //                     (check == "Y" && stampCheck == "0"? (usePoint.ToString("N0").PadLeft(16)) : ("-".PadLeft(11) + usePoint.ToString("N0"))) + Environment.NewLine;
            if(check == "Y" && stampCheck=="0")
            {
                MemberInfo += savePoint.ToString("N0").PadLeft(10) + remainPoint.ToString("N0").PadLeft(16) +
                              usePoint.ToString("N0").PadLeft(16) + Environment.NewLine;

            }
            else if (check == "Y" && stampCheck =="1")
            {
                MemberInfo += savePoint.ToString("N0").PadLeft(10) + remainPoint.ToString("N0").PadLeft(16) +
                              useStampQty.ToString("N0").PadLeft(16) + Environment.NewLine;
            }
            else if (check == "N" && stampCheck == "0")
            {
                MemberInfo += ("-" + savePoint.ToString("N0")).PadLeft(10) + remainPoint.ToString("N0").PadLeft(16) +
                              ("-" + usePoint.ToString("N0")).PadLeft(16) + Environment.NewLine;
            }
            else
            {
                MemberInfo += ("-" + savePoint.ToString("N0")).PadLeft(10) + remainPoint.ToString("N0").PadLeft(16) +
                              ("-" + useStampQty.ToString("N0")).PadLeft(16) + Environment.NewLine;
            }
            if(_arrPpCards != null && _arrPpCards.Any())
            {
                MemberInfo += "선결제 잔액{:R42}" + _arrPpCards[_arrPpCards.Count() - 1].PPC_BALANCE.ToString("N0").PadLeft(15) + Environment.NewLine;
            }
            MemberInfo += "------------------------------------------" + Environment.NewLine;
            return MemberInfo;
        }

        private string PrintPushchaseInfo()
        {
            string PushchaseInfo = string.Empty;
            if (_arrTrnGift == null || _arrTrnGift.Length == 0) return PushchaseInfo;
            PushchaseInfo += "          *** 상품권정보 [1] *** " + Environment.NewLine;
            PushchaseInfo += "------------------------------------------" + Environment.NewLine;
            PushchaseInfo += "상품권명            액면가 수량      금액" + Environment.NewLine;
            PushchaseInfo += "------------------------------------------" + Environment.NewLine;
            foreach (TRN_GIFT trGift in _arrTrnGift)
            {
                PushchaseInfo += "           *** 상품권 정보 ***            " + Environment.NewLine;
                PushchaseInfo += "액 면  가 액:{:R42}" + (trGift.TK_GFT_UAMT > 0 ? _isReturn : "") + trGift.TK_GFT_UAMT.ToString("N0") + Environment.NewLine;
                PushchaseInfo += "결   제   액:{:R42}" + (trGift.TK_GFT_AMT > 0 ? _isReturn : "") + trGift.TK_GFT_AMT.ToString("N0") + Environment.NewLine;
                PushchaseInfo += "현  금환불액:{:R42}" + (trGift.REPAY_CSH_AMT > 0 ? _isReturn : "") + trGift.REPAY_CSH_AMT.ToString("N0") + Environment.NewLine;
                PushchaseInfo += "상품권환불액:{:R42}" + (trGift.REPAY_GFT_AMT > 0 ? _isReturn : "") + trGift.REPAY_GFT_AMT.ToString("N0") + Environment.NewLine;
            }
            return PushchaseInfo;
        }

        private string PrintSimpleCard()
        {
            string SimpleCard = string.Empty;
            SimpleCard += "          *** 간편결제정보 ***            " + Environment.NewLine;
            SimpleCard += "결 제 종 류:{:R42}00페이" + Environment.NewLine;
            SimpleCard += "카 드 번 호:{:R42}{XXXXXXXXXXXXXXXXXXXX}" + Environment.NewLine;
            SimpleCard += "승 인 금 액: 일시불{:R42}{PAY_CARD_NO}" + Environment.NewLine;
            SimpleCard += "승 인 번 호:{:R42}{INST_MM_CNT} {APPR_REQ_AMT}" + Environment.NewLine;
            SimpleCard += "승 인 일 시:{:R42}{APPR_DATE} {APPR_TIME}" + Environment.NewLine;
            SimpleCard += "가맹점 번호:{:R42}{CRDCP_TERM_NO}" + Environment.NewLine;
            SimpleCard += "------------------------------------------" + Environment.NewLine;
            return SimpleCard;
        }


        #endregion

        #region Print kitchen

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string PrintKitchenMenu3()
        {
            string Kitchen = string.Empty;
            if (_arrPrdt != null && _arrPrdt.Length > 0)
            {
                Kitchen += "메뉴명                                수량" + Environment.NewLine;
                Kitchen += "------------------------------------------" + Environment.NewLine;
                foreach (TRN_PRDT prd in _arrPrdt)
                {
                    Kitchen += prd.PRD_NAME + "{:R42}" + prd.SALE_QTY + Environment.NewLine;
                }
            }
            return Kitchen;
        }
        #endregion

        #region Print CustomerOrder 고객주문서
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string PrintCustOrderList()
        {
            string CustOrder = string.Empty;
            if (_arrPrdt != null && _arrPrdt.Length > 0)
            {
                CustOrder += "메뉴명                         수량   구분" + Environment.NewLine;
                CustOrder += "------------------------------------------" + Environment.NewLine;
                foreach (TRN_PRDT prd in _arrPrdt)
                {
                    //var prdName = GetPrdName(prd.PRD_CODE);
                    CustOrder += prd.PRD_NAME + "{:R42}" + prd.SALE_QTY + "   신규" + Environment.NewLine;
                }
            }
            return CustOrder;
        }
        #endregion

        #region Print middle, settle method
        private string PaymentMethod()
        {
            string PaymentMethod = string.Empty;
            if (_setPosAccount == null) { return PaymentMethod; }
            decimal sumData = _setPosAccount.TOT_SALE_AMT - _setPosAccount.TOT_DC_AMT;
            PaymentMethod += "        ** 결제수단별 매출내역 **" + Environment.NewLine;
            PaymentMethod += "현    금{:R42}" + _setPosAccount.CASH_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "신용카드{:R42}" + _setPosAccount.CRD_CARD_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "포 인 트{:R42}" + _setPosAccount.CST_POINTUSE_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "상 품 권{:R42}" + _setPosAccount.TK_GFT_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "외    상{:R42}" + _setPosAccount.WES_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "식    권{:R42}" + _setPosAccount.TK_FOD_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "간편결제{:R42}" + _setPosAccount.SP_PAY_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "선 결 제{:R42}" + _setPosAccount.PPC_CARD_AMT.ToString("N0") + Environment.NewLine;
            PaymentMethod += "------------------------------------------" + Environment.NewLine;
            PaymentMethod += "매출합계{:R42}" + sumData.ToString("N0") + Environment.NewLine;
            PaymentMethod += "------------------------------------------" + Environment.NewLine;
            return PaymentMethod;
        }

        private string PrintDiscountSales()
        {
            string DiscountSales = string.Empty;
            if (_setPosAccount == null) return DiscountSales;
            DiscountSales += "           ** 할인 매출내역 **" + Environment.NewLine;
            DiscountSales += "일반할인{:R42}" + _setPosAccount.DC_GEN_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "서비스{:R42}" + _setPosAccount.DC_SVC_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "포인트카드{:R42}" + _setPosAccount.DC_JCD_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "쿠폰할인{:R42}" + _setPosAccount.DC_CPN_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "회원할인{:R42}" + _setPosAccount.DC_CST_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "식권할인{:R42}" + _setPosAccount.DC_TFD_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "프로모션{:R42}" + _setPosAccount.DC_PRM_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "신용카드할인{:R42}" + _setPosAccount.DC_CRD_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "------------------------------------------" + Environment.NewLine;
            DiscountSales += "할인합계{:R42}" + _setPosAccount.TOT_DC_AMT.ToString("N0") + Environment.NewLine;
            DiscountSales += "------------------------------------------" + Environment.NewLine;
            return DiscountSales;
        }

        private string PrintCashBalance()
        {
            string CashBalance = string.Empty;
            if (_setPosAccount == null) return CashBalance;
            CashBalance += "         ** 현금시재 입력내역 **" + Environment.NewLine;
            CashBalance += "기타금액{:R42}" + _setPosAccount.REM_CHECK_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "   10만원권{:R42}" + _setPosAccount.REM_W100000_CNT + (_setPosAccount.REM_W100000_CNT * 100000).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "   5만원권{:R42}" + _setPosAccount.REM_W50000_CNT + (_setPosAccount.REM_W50000_CNT * 50000).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "10,000원권{:R42}" + _setPosAccount.REM_W10000_CNT + (_setPosAccount.REM_W10000_CNT * 10000).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += " 5,000원권{:R42}" + _setPosAccount.REM_W5000_CNT + (_setPosAccount.REM_W5000_CNT * 5000).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += " 1,000원권{:R42}" + _setPosAccount.REM_W1000_CNT + (_setPosAccount.REM_W1000_CNT * 1000).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "   500원권{:R42}" + _setPosAccount.REM_W500_CNT + (_setPosAccount.REM_W500_CNT * 500).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "   100원권{:R42}" + _setPosAccount.REM_W100_CNT + (_setPosAccount.REM_W100_CNT * 100).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "    50원권{:R42}" + _setPosAccount.REM_W50_CNT + (_setPosAccount.REM_W50_CNT * 50).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "    10원권{:R42}" + _setPosAccount.REM_W10_CNT + (_setPosAccount.REM_W10_CNT * 10).ToString("N0").PadLeft(11) + Environment.NewLine;
            CashBalance += "마감시재액{:R42}" + _setPosAccount.REM_CASH_AMT.ToString("N0") + Environment.NewLine;
            CashBalance += "현금과부족{:R42}" + _setPosAccount.LOSS_CASH_AMT.ToString("N0") + Environment.NewLine;
            CashBalance += "------------------------------------------" + Environment.NewLine;
            return CashBalance;
        }

        private string PrintCashSettlement()
        {
            string CashSettlement = string.Empty;
            if (_setPosAccount == null) return CashSettlement;
            CashSettlement += "           ** 현금 정산액 **" + Environment.NewLine;
            CashSettlement += "준비금{:R42}" + _setPosAccount.POS_READY_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "현금매출액{:R42}" + _setPosAccount.CASH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "외상현금입금{:R42}" + _setPosAccount.WEA_IN_CSH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "상품권현금판매{:R42}" + _setPosAccount.TK_GFT_SALE_CSH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "식권현금판매{:R42}" + _setPosAccount.TK_FOD_SALE_CSH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "시재입금액{:R42}" + _setPosAccount.POS_CSH_IN_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "시재출금액{:R42}" + _setPosAccount.POS_CSH_OUT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "현금환불액{:R42}" + _setPosAccount.REPAY_CASH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "현금과부족{:R42}" + _setPosAccount.LOSS_CASH_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CashSettlement += "------------------------------------------" + Environment.NewLine;
            return CashSettlement;
        }

        private string PrintCreditCardSettlement()
        {
            string CreditCardSettlement = string.Empty;
            if (_setPosAccount == null) return CreditCardSettlement;
            CreditCardSettlement += "         ** 신용카드정산 내역 **" + Environment.NewLine;
            CreditCardSettlement += "카드승인{:R42}" + _arrTrnCards.Where(x => x.SALE_YN == "Y").Count() + _arrTrnCards.Where(x => x.SALE_YN == "Y").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            CreditCardSettlement += "카드취소{:R42}" + _arrTrnCards.Where(x => x.SALE_YN == "N").Count() + _arrTrnCards.Where(x => x.SALE_YN == "N").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            CreditCardSettlement += "카드매출{:R42}" + _setPosAccount.CRD_CARD_CNT + _setPosAccount.CRD_CARD_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            CreditCardSettlement += "------------------------------------------" + Environment.NewLine;
            return CreditCardSettlement;
        }

        private string PrintSaleByCard()
        {
            string SaleByCard = string.Empty;
            if (_arrTrnCards.Length == 0) return SaleByCard;
            SaleByCard += "         ** 카드사별 매출내역 **" + Environment.NewLine;
            SaleByCard += "BC  카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "BC  카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "BC  카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "국민카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "국민카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "국민카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "삼성카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "삼성카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "삼성카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "현대카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "현대카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "현대카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "신한카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "신한카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "신한카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "외환카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "외환카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "외환카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "롯데카드{:R42}" + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "롯데카드사").Count() + _arrTrnCards.Where(x => x.ISS_CRDCP_NAME == "롯데카드사").Sum(x => x.APPR_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleByCard += "------------------------------------------" + Environment.NewLine;
            return SaleByCard;
        }

        /// <summary>
        ///           ** 회원 포인트 내역 **
        ///           총영수건수                               0
        ///           매정내방객수                             0
        ///           적립포인트             0                 0
        ///           사용포인트             0                 0
        ///           SETT_POSACCOUNT	TOT_BILL_CNT
        ///           SETT_POSACCOUNT VISIT_CST_CNT
        ///           SETT_POSACCOUNT CST_POINTSAVE_CNT
        ///           SETT_POSACCOUNT CST_POINTSAVE_AMT
        ///           SETT_POSACCOUNT CST_POINTUSE_CNT
        ///           SETT_POSACCOUNT CST_POINTUSE_AMT
        /// </summary>
        /// <returns></returns>
        private string PrintMemberPoint()
        {
            if (_setPosAccount == null) return string.Empty;
            StringBuilder sbText = new StringBuilder();
            sbText.AppendLine("          ** 회원 포인트 내역 **");
            sbText.AppendLine("총영수건수{:R42}" + _setPosAccount.TOT_BILL_CNT.ToString("N0").PadLeft(11));
            sbText.AppendLine("매정내방객수{:R42}" + _setPosAccount.VISIT_CST_CNT.ToString("N0").PadLeft(11));
            sbText.AppendLine("적립포인트{:R42}" + _setPosAccount.CST_POINTSAVE_CNT.ToString("N0") + _setPosAccount.CST_POINTSAVE_AMT.ToString("N0").PadLeft(11));
            sbText.AppendLine("사용포인트{:R42}" + _setPosAccount.CST_POINTUSE_CNT.ToString("N0") + _setPosAccount.CST_POINTUSE_AMT.ToString("N0").PadLeft(11));
            return sbText.ToString();
        }

        private string PrintGiftCard()
        {
            string GiftCard = string.Empty;
            if (_setPosAccount == null) return GiftCard;
            GiftCard += "          ** 상품권시재 내역 **" + Environment.NewLine;
            GiftCard += "상품권회수{:R42}" + _setPosAccount.TK_GFT_CNT + _setPosAccount.TK_GFT_UAMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            GiftCard += "상품권환불{:R42}" + _setPosAccount.REPAY_TK_GFT_CNT + _setPosAccount.REPAY_TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            GiftCard += "상품권시재{:R42}" + _setPosAccount.REM_TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            GiftCard += "상품권과부족{:R42}" + _setPosAccount.LOSS_TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            GiftCard += "------------------------------------------" + Environment.NewLine;
            return GiftCard;
        }

        private string PrintNonSaleSettlement()
        {
            //NTRN_TKSALE_CASH nTRN_TKSALE_CASH = new NTRN_TKSALE_CASH();
            //NTRN_TKSALE_CARD nTRN_TKSALE_CARD = new NTRN_TKSALE_CARD();
            //NTRN_CREDIT_IN_CASH nTRN_CREDIT_IN_CASH = new NTRN_CREDIT_IN_CASH();
            //NTRN_CREDIT_IN_CARD nTRN_CREDIT_IN_CARD = new NTRN_CREDIT_IN_CARD();
            //NonSaleSettlement += "** 상품권판매" + Environment.NewLine;
            //NonSaleSettlement += "현금{:R42}" + nTRN_TKSALE_CASH.CASH_AMT.ToString("N0") + Environment.NewLine;
            //NonSaleSettlement += "신용카드{:R42}" + nTRN_TKSALE_CARD.APPR_AMT.ToString("N0") + Environment.NewLine;
            //NonSaleSettlement += Environment.NewLine;
            //NonSaleSettlement += "** 외상입금" + Environment.NewLine;
            //NonSaleSettlement += "현금{:R42}" + nTRN_CREDIT_IN_CASH.CASH_AMT.ToString("N0") + Environment.NewLine;
            //NonSaleSettlement += "신용카드{:R42}" + nTRN_CREDIT_IN_CARD.APPR_AMT.ToString("N0") + Environment.NewLine;

            string NonSaleSettlement = string.Empty;
            if (_setPosAccount == null) { return NonSaleSettlement; }
            NonSaleSettlement += "         *** 비매출 정산 내역 ***" + Environment.NewLine;
            NonSaleSettlement += "** 선결제충전" + Environment.NewLine;
            NonSaleSettlement += "신용카드{:R42}" + _setPosAccount.PRE_PNT_SALE_CRD_AMT.ToString("N0") + Environment.NewLine;
            NonSaleSettlement += "------------------------------------------" + Environment.NewLine;
            return NonSaleSettlement;
        }

        private string PrintSaleGift()
        {
            string SaleGift = string.Empty;
            if (_arrTrnGift.Length == 0) return SaleGift;

            // 매출결제
            TRN_GIFT[] saleList = _arrTrnGift
                .Where(x => x.SALE_YN == "Y" && x.TK_GFT_SALE_FLAG == "0")
                .GroupBy(x => new { x.TK_GFT_CODE, x.TK_GFT_UAMT })
                .Select(group => new TRN_GIFT
                {
                    TK_GFT_CODE = group.Key.TK_GFT_CODE,
                    TK_GFT_UAMT = group.Key.TK_GFT_UAMT,
                    REGI_SEQ = group.Count().ToString(),
                    TK_GFT_AMT = group.Sum(x => x.TK_GFT_AMT)
                }).ToArray();

            // 매출환입
            TRN_GIFT[] saleBackList = _arrTrnGift
                .Where(x => x.SALE_YN == "Y" && x.TK_GFT_SALE_FLAG == "3")
                .GroupBy(x => new { x.TK_GFT_CODE, x.TK_GFT_UAMT })
                .Select(group => new TRN_GIFT
                {
                    TK_GFT_UAMT = group.Key.TK_GFT_UAMT,
                    REGI_SEQ = group.Count().ToString(),
                    TK_GFT_AMT = group.Sum(x => x.TK_GFT_AMT)
                }).ToArray();

            TRN_GIFT[] retList = _arrTrnGift
                .Where(x => x.SALE_YN == "N" && x.TK_GFT_SALE_FLAG == "0")
                .GroupBy(x => new { x.TK_GFT_CODE, x.TK_GFT_UAMT })
                .Select(group => new TRN_GIFT
                {
                    TK_GFT_UAMT = group.Key.TK_GFT_UAMT,
                    REGI_SEQ = group.Count().ToString(),
                    TK_GFT_AMT = group.Sum(x => x.TK_GFT_AMT)
                }).ToArray();

            TRN_GIFT[] retBackList = _arrTrnGift
                .Where(x => x.SALE_YN == "N" && x.TK_GFT_SALE_FLAG == "3")
                .GroupBy(x => new { x.TK_GFT_CODE, x.TK_GFT_UAMT })
                .Select(group => new TRN_GIFT
                {
                    TK_GFT_UAMT = group.Key.TK_GFT_UAMT,
                    REGI_SEQ = group.Count().ToString(),
                    TK_GFT_AMT = group.Sum(x => x.TK_GFT_AMT)
                }).ToArray();


            SaleGift += "         *** 매출 상품권 내역 ***" + Environment.NewLine;
            SaleGift += "------------------------------------------" + Environment.NewLine;
            SaleGift += "상품권명       액면가       수량      금액" + Environment.NewLine;
            SaleGift += "------------------------------------------" + Environment.NewLine;
            foreach (TRN_GIFT tRN_GIFT in saleList)
            {
                SaleGift += GetGiftName(tRN_GIFT.TK_GFT_CODE) + "{:R42}매출-결제" + Environment.NewLine;
                SaleGift += tRN_GIFT.TK_GFT_CODE + "{:R42}" + tRN_GIFT.TK_GFT_UAMT.ToString("N0") + tRN_GIFT.REGI_SEQ.PadLeft(4) + tRN_GIFT.TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            }
            foreach (TRN_GIFT tRN_GIFT in saleBackList)
            {
                SaleGift += GetGiftName(tRN_GIFT.TK_GFT_CODE) + "{:R42}매출-환불" + Environment.NewLine;
                SaleGift += tRN_GIFT.TK_GFT_CODE + "{:R42}" + tRN_GIFT.TK_GFT_UAMT.ToString("N0") + tRN_GIFT.REGI_SEQ.PadLeft(4) + tRN_GIFT.TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            }
            foreach (TRN_GIFT tRN_GIFT in retList)
            {
                SaleGift += GetGiftName(tRN_GIFT.TK_GFT_CODE) + "{:R42}반품-환불" + Environment.NewLine;
                SaleGift += tRN_GIFT.TK_GFT_CODE + "{:R42}" + tRN_GIFT.TK_GFT_UAMT.ToString("N0") + tRN_GIFT.REGI_SEQ.PadLeft(4) + tRN_GIFT.TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            }
            foreach (TRN_GIFT tRN_GIFT in retBackList)
            {
                SaleGift += GetGiftName(tRN_GIFT.TK_GFT_CODE) + "{:R42}반품-환입" + Environment.NewLine;
                SaleGift += tRN_GIFT.TK_GFT_CODE + "{:R42}" + tRN_GIFT.TK_GFT_UAMT.ToString("N0") + tRN_GIFT.REGI_SEQ.PadLeft(4) + tRN_GIFT.TK_GFT_AMT.ToString("N0").PadLeft(11) + Environment.NewLine;
            }
            SaleGift += "------------------------------------------" + Environment.NewLine;
            SaleGift += "매출 - 결제{:R42}" + saleList.Count() + saleList.Sum(x => x.TK_GFT_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleGift += "매출 - 환불{:R42}" + saleBackList.Count() + saleBackList.Sum(x => x.TK_GFT_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleGift += "반품 - 환불{:R42}" + retList.Count() + retList.Sum(x => x.TK_GFT_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleGift += "반품 - 환입{:R42}" + retBackList.Count() + retBackList.Sum(x => x.TK_GFT_AMT).ToString("N0").PadLeft(11) + Environment.NewLine;
            SaleGift += "------------------------------------------" + Environment.NewLine;
            return SaleGift;
        }

        private string PrintSaleTotal()
        {
            string SaleTotal = string.Empty;
            if (_setPosAccount == null) return string.Empty;
            SaleTotal += "             **  매출 합계 **" + Environment.NewLine;
            SaleTotal += "총 판매액{:R42}" + (_setPosAccount.TOT_SALE_AMT - _setPosAccount.RET_BILL_AMT).ToString("N0") + Environment.NewLine;
            SaleTotal += "총 반품액{:R42}" + _setPosAccount.RET_BILL_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "총 매출액{:R42}" + _setPosAccount.TOT_SALE_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "총 할인액{:R42}" + _setPosAccount.TOT_DC_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "총 봉사료{:R42}" + _setPosAccount.SVC_TIP_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "실 매출액{:R42}" + _setPosAccount.DCM_SALE_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "면세매출액{:R42}" + _setPosAccount.NO_VAT_SALE_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "과세매출액{:R42}" + _setPosAccount.VAT_SALE_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "부가 세액{:R42}" + _setPosAccount.VAT_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "------------------------------------------" + Environment.NewLine;
            SaleTotal += "순 매출액{:R42}" + _setPosAccount.NO_TAX_SALE_AMT.ToString("N0") + Environment.NewLine;
            SaleTotal += "------------------------------------------" + Environment.NewLine;
            return SaleTotal;
        }

        private string GetGiftName(string giftCode)
        {
            using (var db = new DataContext())
            {
                return db.mST_INFO_TICKETs.Where(x => x.TK_GFT_CODE == giftCode).Select(x => x.TK_GFT_NAME).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string PrintTrialCheck()
        {
            if (_setPosAccount == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**매출 합계**");
            sb.Append("총판매액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.TOT_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("총반품액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.RET_BILL_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("총매출액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.TOT_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("총할인액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.TOT_DC_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("실매출액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.DCM_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("과세매출액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.VAT_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("부가 세액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.VAT_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("면세매출액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.NO_VAT_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.AppendLine("------------------------------------------");
            sb.Append("순매출액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.NO_TAX_SALE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.AppendLine("------------------------------------------");
            sb.AppendLine("**결제수단별 매출내역**");
            sb.Append("일반  현금{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.CASH_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("현금영수증{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.CASH_BILL_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("신용 카드{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.CRD_CARD_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("외 상{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.WES_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("상 품  권{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.TK_GFT_UAMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("회원포인트{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.CST_POINTUSE_AMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("식 권{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.TK_FOD_UAMT.ToString("N0").PadLeft(11), Environment.NewLine);
            sb.Append("현금시재액{:R42}");
            sb.AppendFormat("{0}{1}", _setPosAccount.REM_CASH_AMT.ToString("N0").PadLeft(11), Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        #endregion

        #region get data product Name, payment, helper functions

        private PAYMENT GetPayMent(TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns, TRN_EASYPAY[] trEasypays,
            TRN_PARTCARD[] trPartCards, TRN_POINTUSE[] tRN_POINTUSE, TRN_POINTSAVE tRN_POINTSAVE, TRN_PPCARD[] tRN_PPCARD)
        {
            PAYMENT pAYMENT = new PAYMENT();
            if (trGifts != null)
            {
                foreach (var trGift in trGifts)
                {
                    pAYMENT.TK_GFT_AMT = pAYMENT.TK_GFT_AMT + trGift.TK_GFT_AMT;
                }
            }
            pAYMENT.TK_GFT_AMT = trGifts == null || trGifts.Length == 0 ? -1 : pAYMENT.TK_GFT_AMT;
            if (trCards != null)
            {
                foreach (var trCard in trCards)
                {
                    pAYMENT.CRD_CARD_AMT = pAYMENT.CRD_CARD_AMT + trCard.APPR_AMT;
                }
            }
            pAYMENT.CRD_CARD_AMT = trCards == null || trCards.Length == 0 ? -1 : pAYMENT.CRD_CARD_AMT;

            if (trCashs != null)
            {
                foreach (var trCash in trCashs)
                {
                    pAYMENT.PAY_AMT = pAYMENT.PAY_AMT + (trCash.CASH_AMT - trCash.RET_AMT);
                }
            }
            pAYMENT.PAY_AMT = trCashs == null || trCashs.Length == 0 ? -1 : pAYMENT.PAY_AMT;

            if (trFoodCpns != null)
            {
                foreach (var trFoodCpn in trFoodCpns)
                {

                    pAYMENT.TK_FOD_AMT = pAYMENT.TK_FOD_AMT + trFoodCpn.TK_FOD_AMT;
                }
            }
            pAYMENT.TK_FOD_AMT = trFoodCpns == null || trFoodCpns.Length == 0 ? -1 : pAYMENT.TK_FOD_AMT;

            if (trEasypays != null)
            {
                List<PAY_PRINT_ITEM> easyPayPrints = new List<PAY_PRINT_ITEM>();
                foreach (var trEasypay in trEasypays)
                {
                    var ePay = easyPayPrints.FirstOrDefault(p => p.PAY_NAME == trEasypay.PAYCP_CODE);
                    if (ePay != null)
                    {
                        ePay.PAY_AMT += trEasypay.APPR_REQ_AMT;
                        continue;
                    }

                    easyPayPrints.Add(new PAY_PRINT_ITEM()
                    {
                        PAY_NAME = GetSimplePayName(trEasypay.PAYCP_CODE),
                        PAY_AMT = trEasypay.APPR_REQ_AMT
                    });

                    pAYMENT.EASY_PAY_AMT = pAYMENT.EASY_PAY_AMT + trEasypay.APPR_REQ_AMT;
                }

                pAYMENT.EASY_PAY_AMTs = easyPayPrints.ToArray();
            }
            pAYMENT.EASY_PAY_AMT = trEasypays == null || trEasypays.Length == 0 ? -1 : pAYMENT.EASY_PAY_AMT;

            if (trPartCards != null)
            {
                foreach (var tRN_PARTCARD in trPartCards)
                {
                    pAYMENT.JCD_CARD_AMT = pAYMENT.JCD_CARD_AMT + tRN_PARTCARD.APPR_AMT;
                }
            }
            pAYMENT.JCD_CARD_AMT = trPartCards == null || trPartCards.Length == 0 ? -1 : pAYMENT.JCD_CARD_AMT;
            if (tRN_POINTUSE != null)
            {
                foreach(var point in tRN_POINTUSE)
                {
                    if (point.USE_TYPE == "1")
                    {

                        pAYMENT.CST_POINT_AMT = pAYMENT.CST_POINT_AMT + point.USE_PNT;
                    }
                    else
                    {
                        pAYMENT.CST_STAMP_AMT = pAYMENT.CST_STAMP_AMT + point.USE_PNT;// (tRN_POINTUSE.USE_TYPE == "03" ? tRN_POINTUSE.USE_STAMP : tRN_POINTUSE.USE_PNT);
                    }
                }
            }
            pAYMENT.PPC_PAY_AMT = tRN_PPCARD == null || tRN_PPCARD.Length == 0 ? -1 : pAYMENT.PPC_PAY_AMT;
            if (tRN_PPCARD != null)
            {
                foreach (var ppcard in tRN_PPCARD)
                {
                    pAYMENT.PPC_PAY_AMT = pAYMENT.PPC_PAY_AMT + ppcard.PPC_AMT;
                }
            }
            return pAYMENT;
        }
        private string FormatHeaderField(string key)
        {
            string value = string.Empty;
            try
            {
                switch (key)
                {
                    case "EX_NUM":
                        return _trnHeader.POS_NO.Substring(1) + _trnHeader.BILL_NO.Substring(1);
                    case "SHOP_NAME":
                        return DataLocals.ShopInfo.SHOP_NAME == null ? "" : DataLocals.ShopInfo.SHOP_NAME;
                    case "BIZ_NO":
                        StringBuilder bizNo = new StringBuilder(DataLocals.ShopInfo.BIZ_NO);
                        bizNo.Insert(3, "-");
                        bizNo.Insert(6, "-");
                        return bizNo.ToString();
                    case "ADDRESS":
                        return DataLocals.ShopInfo.ADDR + " " + DataLocals.ShopInfo.ADDR_DTL;
                    case "OWNER_NAME":
                        return DataLocals.ShopInfo.OWNER_NAME == null ? "" : DataLocals.ShopInfo.OWNER_NAME;
                    case "TEL_NO":
                        return DataLocals.ShopInfo.TEL_NO == null ? "" : DataLocals.ShopInfo.TEL_NO;
                    case "DATETIME":
                        return string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
                    case "SALE_TIME":
                        if (string.IsNullOrEmpty(_trnHeader.INSERT_DT)) return "          ";
                        string date = _trnHeader.INSERT_DT.Substring(0, 8);
                        string time = _trnHeader.INSERT_DT.Substring(8, 6);
                        DateTime parsedDate;
                        string fDate = "";
                        if (DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            fDate = parsedDate.ToString("yyyy-MM-dd");
                        }
                        string formattedTime = $"{time.Substring(0, 2)}:{time.Substring(2, 2)}:{time.Substring(4, 2)}";
                        return $"{fDate} {formattedTime}";
                    case "POS_NO":
                        return _trnHeader == null ? _setPosAccount.POS_NO : _trnHeader.POS_NO;
                    case "EMP_NO":
                        return _trnHeader != null ? _trnHeader.EMP_NO : _setPosAccount.EMP_NO;
                    case "BILL_NUMBER":
                        return $"{_trnHeader.SALE_DATE}-{_trnHeader.POS_NO}-{_trnHeader.BILL_NO}";
                    case "TOT_SALE_AMT":
                        return "합 계  금 액{:R42}" + ((_trnHeader.TOT_SALE_AMT > 0 ? _isReturn : "") + _trnHeader.TOT_SALE_AMT.ToString("N0")).PadLeft(11);
                    case "TOT_DC_AMT":
                        return "할 인  금 액{:R42}" + ((_trnHeader.TOT_DC_AMT > 0 ? _isReturn : "") + _trnHeader.TOT_DC_AMT.ToString("N0")).PadLeft(11);
                    case "EXP_PAY_AMT":
                        return "받 을  금 액{:R42}" + ((_trnHeader.EXP_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.EXP_PAY_AMT.ToString("N0")).PadLeft(6);
                    case "GST_PAY_AMT":
                        return "받 은  금 액{:R42}" + ((_trnHeader.GST_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.GST_PAY_AMT.ToString("N0")).PadLeft(6);
                    case "RET_PAY_AMT":
                        if (_trnHeader.RET_PAY_AMT == 0)
                        {
                            return "";
                        }
                        else
                        {
                            return "거 스  름 돈{:R42}" + ((_trnHeader.RET_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.RET_PAY_AMT.ToString("N0")).PadLeft(6);
                        }
                    case "ORDER_ID":
                        return _trnHeader.BILL_NO + "-" + _trnHeader.ORDER_NO;
                    case "SALE_DATE":
                        string SaleDate = _trnHeader == null ? _setPosAccount.SALE_DATE : _trnHeader.SALE_DATE;
                        DateTime dtSaleDate = DateTime.ParseExact(SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                        return dtSaleDate.ToString("yyyy-MM-dd");
                    case "EMP_NO_NAME":
                        return GetEmpName(_trnHeader == null ? _setPosAccount.EMP_NO : _trnHeader.EMP_NO);
                    //[" + (_trnHeader == null ? _setPosAccount.EMP_NO : _trnHeader.EMP_NO) + "] " + GetEmpName(DataLocals.PosStatus.EMP_NO);
                    case "REGI_SEQ":
                        return string.Format("{0}차", int.Parse(_trnHeader == null ? _setPosAccount.REGI_SEQ : _trnHeader.REGI_SEQ));
                    case "OPEN_DT":
                        string OpenDt = _setPosAccount.OPEN_DT;
                        DateTime dtOpenDt = DateTime.ParseExact(OpenDt, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        return dtOpenDt.ToString("yyyy-MM-dd HH:mm:ss");
                    case "REGI_TIME":
                        DateTime dt = DateTime.Now;
                        return dt.ToString("yyyy-MM-dd tt hh:mm:ss", new System.Globalization.CultureInfo("ko-KR"));
                    case "RechareProducts":
                        return "";
                    case "TotalPrechangreAmount":
                        return "";
                    case "AmountInAdvance":
                        return "";
                    case "PrerpaidAmout":
                        return "";
                    case "PrepaidChange":
                        return "";
                    default:
                        return "{" + key + "}";
                }
            }
            catch
            {
                return value;
            }
        }

        private string FormatHeaderField2(string key)
        {
            string value = string.Empty;
            try
            {
                switch (key)
                {
                    case "EX_NUM":
                        return _ntrnPreCHARGE.POS_NO.Substring(1) + _ntrnPreCHARGE.SALE_NO.Substring(1);
                    case "SHOP_NAME":
                        return DataLocals.ShopInfo.SHOP_NAME == null ? "" : DataLocals.ShopInfo.SHOP_NAME;
                    case "BIZ_NO":
                        StringBuilder bizNo = new StringBuilder(DataLocals.ShopInfo.BIZ_NO);
                        bizNo.Insert(3, "-");
                        bizNo.Insert(6, "-");
                        return bizNo.ToString();
                    case "ADDRESS":
                        return DataLocals.ShopInfo.ADDR + " " + DataLocals.ShopInfo.ADDR_DTL;
                    case "OWNER_NAME":
                        return DataLocals.ShopInfo.OWNER_NAME == null ? "" : DataLocals.ShopInfo.OWNER_NAME;
                    case "TEL_NO":
                        return DataLocals.ShopInfo.TEL_NO == null ? "" : DataLocals.ShopInfo.TEL_NO;
                    case "DATETIME":
                        return string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
                    case "SALE_TIME":
                        if (string.IsNullOrEmpty(_ntrnPreCHARGE.INSERT_DT)) return "          ";
                        string date = _ntrnPreCHARGE.INSERT_DT.Substring(0, 8);
                        string time = _ntrnPreCHARGE.INSERT_DT.Substring(8, 6);
                        DateTime parsedDate;
                        string fDate = "";
                        if (DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            fDate = parsedDate.ToString("yyyy-MM-dd");
                        }
                        string formattedTime = $"{time.Substring(0, 2)}:{time.Substring(2, 2)}:{time.Substring(4, 2)}";
                        return $"{fDate} {formattedTime}";
                    case "POS_NO":
                        return _ntrnPreCHARGE == null ? _setPosAccount.POS_NO : _ntrnPreCHARGE.POS_NO;
                    case "EMP_NO":
                        return _ntrnPreCHARGE != null ? _ntrnPreCHARGE.EMP_NO : _setPosAccount.EMP_NO;
                    case "BILL_NUMBER":
                        return $"{_ntrnPreCHARGE.SALE_DATE}-{_ntrnPreCHARGE.POS_NO}-{_ntrnPreCHARGE.SALE_NO}";
                    case "TOT_SALE_AMT":
                        return "합 계  금 액{:R42}" + ((_trnHeader.TOT_SALE_AMT > 0 ? _isReturn : "") + _trnHeader.TOT_SALE_AMT.ToString("N0")).PadLeft(11);
                    case "TOT_DC_AMT":
                        return "할 인  금 액{:R42}" + ((_trnHeader.TOT_DC_AMT > 0 ? _isReturn : "") + _trnHeader.TOT_DC_AMT.ToString("N0")).PadLeft(11);
                    case "EXP_PAY_AMT":
                        return "받 을  금 액{:R42}" + ((_trnHeader.EXP_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.EXP_PAY_AMT.ToString("N0")).PadLeft(6);
                    case "GST_PAY_AMT":
                        return "받 은  금 액{:R42}" + ((_trnHeader.GST_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.GST_PAY_AMT.ToString("N0")).PadLeft(6);
                    case "RET_PAY_AMT":
                        if (_trnHeader.RET_PAY_AMT == 0)
                        {
                            return "";
                        }
                        else
                        {
                            return "거 스  름 돈{:R42}" + ((_trnHeader.RET_PAY_AMT > 0 ? _isReturn : "") + _trnHeader.RET_PAY_AMT.ToString("N0")).PadLeft(6);
                        }
                    case "ORDER_ID":
                        return _trnHeader.BILL_NO + "-" + _trnHeader.ORDER_NO;
                    case "SALE_DATE":
                        string SaleDate = _ntrnPreCHARGE == null ? _setPosAccount.SALE_DATE : _ntrnPreCHARGE.SALE_DATE;
                        DateTime dtSaleDate = DateTime.ParseExact(SaleDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                        return dtSaleDate.ToString("yyyy-MM-dd");
                    case "EMP_NO_NAME":
                        return GetEmpName(_ntrnPreCHARGE == null ? _setPosAccount.EMP_NO : _ntrnPreCHARGE.EMP_NO);
                    //[" + (_trnHeader == null ? _setPosAccount.EMP_NO : _trnHeader.EMP_NO) + "] " + GetEmpName(DataLocals.PosStatus.EMP_NO);
                    case "REGI_SEQ":
                        return string.Format("{0}차", int.Parse(_ntrnPreCHARGE == null ? _setPosAccount.REGI_SEQ : _ntrnPreCHARGE.REGI_SEQ));
                    case "OPEN_DT":
                        string OpenDt = _setPosAccount.OPEN_DT;
                        DateTime dtOpenDt = DateTime.ParseExact(OpenDt, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        return dtOpenDt.ToString("yyyy-MM-dd HH:mm:ss");
                    case "REGI_TIME":
                        DateTime dt = DateTime.Now;
                        return dt.ToString("yyyy-MM-dd tt hh:mm:ss", new System.Globalization.CultureInfo("ko-KR"));
                    case "RechareProducts":
                        string ItemList = string.Empty;
                        ItemList += "상품명                수량       충전금액" + Environment.NewLine;
                        ItemList += "------------------------------------------" + Environment.NewLine;
                        ItemList += "선결제 충전{:R42}" + (_isRePrint == true ? "-1".PadLeft(12) : 1.ToString().PadLeft(12)) + ((_ntrnPreCHARGE.CHARGE_AMT > 0 ? _isReturn : "") + (_isRePrint == true ? (_ntrnPreCHARGE.CHARGE_AMT * -1).ToString("N0").PadLeft(16) : _ntrnPreCHARGE.CHARGE_AMT.ToString("N0").PadLeft(16))) + Environment.NewLine;
                        //ItemList += "선결제 잔액{:R42}" + ((_ntrnPreCHARGE.CHARGE_REM_AMT > 0 ? _isReturn : "") + _ntrnPreCHARGE.CHARGE_REM_AMT.ToString("N0")) + Environment.NewLine;
                        ItemList += "회원번호{:R42}" + _ntrnPreCHARGE.CST_NO + Environment.NewLine;

                        return ItemList;
                    case "TotalPrechangreAmount":
                        return "합 계 금 액{:R42}" + (_isRePrint == true ? (_ntrnPreCHARGE.CHARGE_AMT * -1).ToString("N0").PadLeft(11) : _ntrnPreCHARGE.CHARGE_AMT.ToString("N0").PadLeft(11));
                    case "AmountInAdvance":
                        return "받 을 금 액{:R42}" + (_isRePrint == true ? (_ntrnPreCHARGE.CHARGE_AMT * -1).ToString("N0").PadLeft(11) : _ntrnPreCHARGE.CHARGE_AMT.ToString("N0").PadLeft(11));
                    case "PrerpaidAmout":
                        return "받 은 금 액{:R42}" + (_isRePrint == true ? (_ntrnPreCHARGE.CHARGE_AMT * -1).ToString("N0").PadLeft(11) : _ntrnPreCHARGE.CHARGE_AMT.ToString("N0").PadLeft(11));
                    case "PrepaidChange":
                        return "거 스 름 돈{:R42}" + 0.ToString("N0").PadLeft(11);
                    default:
                        return "{" + key + "}";
                }
            }
            catch
            {
                return value;
            }
        }

        private string GetEmpName(string empNo)
        {
            using (var db = new DataContext())
            {
                return db.mST_INFO_EMPs.Where(x => x.EMP_NO == empNo).Select(x => x.EMP_NAME).FirstOrDefault();
            }
        }

        #endregion
    }
}
