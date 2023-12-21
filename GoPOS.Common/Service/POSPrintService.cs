using GoPOS.Common.PrinterLib;
using GoPOS.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows.Media;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using static GoPOS.Common.PrinterLib.protocol.BasicProtocol;
using GoShared.Helpers;
using System.Text.RegularExpressions;
using ESCPOS_NET.Emitters;
using GoShared.Contract;
using NLog.Fluent;
using System.Windows.Markup;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using System.Data;
using System.Reflection;

namespace GoPOS.Common.Service
{
    /// <summary>
    /// functions for all print templates
    /// </summary>
    public class POSPrintService : IPOSPrintService, IDisposable
    {
        byte[] FeedLines1 = new byte[3] { 27, 100, 1 };
        byte[] FeedLines2 = new byte[3] { 27, 100, 2 };
        byte[] SetCharacterSize0 = new byte[3] { 29, 33, 0 };
        byte[] SetCharacterSize1 = new byte[3] { 29, 33, 17 };
        byte[] FeedLines5 = new byte[3] { 27, 100, 5 };
        byte[] CutPaper1 = new byte[3] { 29, 86, 1 };
        byte[] AlignRight = new byte[3] { 27, 97, 2 };
        byte[] AlignLeft = new byte[3] { 27, 97, 0 };
        byte[] AlignCenter = new byte[3] { 27, 97, 1 };

        private readonly IServiceProvider serviceProvider;
        public GeneralPrinter? Printer { get; set; }
        public ImageSource _logo { get; set; }
        private bool _printErrorShown = false;

        public POSPrintService(IServiceProvider serviceProvider)
        {
            Printer = serviceProvider.GetRequiredService<GeneralPrinter>();

            string directory = string.Empty + Directory.GetParent(Environment.CurrentDirectory);
            _logo = new BitmapImage(new Uri(System.IO.Path.Combine(directory, "Data/res/logo.png"), UriKind.Absolute));
        }

        #region Template functions

        MST_CNFG_DETAIL GetConfigValue(bool isShopValue, string setCode)
        {
            using (var context = new DataContext())
            {
                string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, isShopValue ? "GetShopConfigValue" : "GetPosConfigValue");
                return DapperORM.ReturnSingleAsync<MST_CNFG_DETAIL>(sql, new string[]
                {
                    "@SET_CODE"
                },
                new object[]
                {
                    setCode
                }).Result;
            }
        }

        public Task<bool> TryOpen()
        {
            try
            {
                int port = Convert.ToInt32(DataLocals.AppConfig.PosOption.ReceiptPrintPort);
                int rate = Convert.ToInt32(DataLocals.AppConfig.PosOption.ReceiptPrintSpeed);
                if (port <= 0 || rate <= 0)
                {
                    return Task.FromResult(false);
                }

                bool isOpen = Printer.TryOpen(port, rate);
                return Task.FromResult(isOpen);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public void EndPrinting()
        {
            _printErrorShown = false;
            Printer.ClosePrinter();
        }

        public (ReceiptPrintReturns, PrintStatus) CheckShowPrinterStatus(bool silenceMode)
        {
            return CheckShowPrinterStatus(true, silenceMode);
        }

        /// <summary>
        /// 
        /// </summary>
        public (ReceiptPrintReturns, PrintStatus) CheckShowPrinterStatus(bool printYN, bool silenceMode)
        {
            if (!DataLocals.AppConfig.PosOption.ReceiptPrinterYN || !printYN)
            {
                LogHelper.Logger.Trace("출력안함.");
                return (ReceiptPrintReturns.DontPrint, Printer._status);
            }

            if (!Printer.IsOpen)
            {
                if (!TryOpen().Result)
                {
                    // Show msg
                    if (!silenceMode && !_printErrorShown)
                    {
                        DialogHelper.MessageBox("프린터 포트 접속오류 입니다.\r\n환경설정에서 포스정보를 확인하여 주십시오.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        _printErrorShown = true;
                    }
                    return (ReceiptPrintReturns.Errored, Printer._status);
                }
            }

            if (!Printer._status.isOnline)
            {
                // Show msg
                if (!silenceMode && !_printErrorShown)
                {
                    DialogHelper.MessageBox("프린터 포트 접속오류 입니다.\r\n환경설정에서 포스정보를 확인하여 주십시오.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    _printErrorShown = true;
                }

                LogHelper.Logger.Trace("프린터 포트 접속오류 입니다.\r\n환경설정에서 포스정보를 확인하여 주십시오.");
            }
            else
            {
                _printErrorShown = false;
            }

            return (Printer._status.isOnline ? ReceiptPrintReturns.CanPrint : ReceiptPrintReturns.Offline, Printer._status);
        }

        #region Template functions

        #region PrintReceipt

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="trHeader"></param>
        /// <param name="orderSumInfo"></param>
        /// <param name="printItems"></param>
        /// <param name="trProducts"></param>
        /// <param name="trTenders"></param>
        /// <param name="trCashs"></param>
        /// <param name="trCashRecs"></param>
        /// <param name="trCards"></param>
        /// <param name="trnPartCards"></param>
        /// <param name="trGifts"></param>
        /// <param name="trFoodCpns"></param>
        /// <param name="trEasypays"></param>
        /// <param name="tRN_POINTUSE"></param>
        /// <param name="tRN_POINTSAVE"></param>
        public void PrintReceipt(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo,
            bool printItems, TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders, TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards,
                    TRN_PARTCARD[] trnPartCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns, TRN_EASYPAY[] trEasypays
                   , TRN_POINTUSE[] tRN_POINTUSE, TRN_POINTSAVE tRN_POINTSAVE, TRN_PPCARD[] tRN_PPcard, MEMBER_CLASH memberInfo)
        {
            printOptions = CheckShowPrinterStatus(printOptions != PrintOptions.JournalOnly, false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "1")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                object[] datas = new object[]
                {
                    trHeader, orderSumInfo, trProducts, trTenders, trCashs, trCashRecs, trCards,trGifts, trFoodCpns, trEasypays,trnPartCards,  null,
                    tRN_POINTUSE, tRN_POINTSAVE, tRN_PPcard, memberInfo
                };
                PrintTemplateProcess template = new PrintTemplateProcess();

                string plainText = string.Empty;
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetReceiptText(prtForm, printItems, false, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if ((trCashs != null && trCashs.Length > 0) || (trGifts != null && trGifts.Length > 0) || (trFoodCpns != null && trFoodCpns.Length > 0))
                {
                    OpenCashDrawer();
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetReceiptText(prtForm, printItems, false, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = trHeader?.BILL_NO,
                        PosNo = trHeader?.POS_NO,
                        SaleDate = trHeader?.SALE_DATE,
                        ShopCode = trHeader?.SHOP_CODE
                    }, plainText);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="billNo"></param>
        /// <param name="isPrintItem"></param>
        /// <param name="isRePrint"></param>
        public void PrintReceipt(PrintOptions printOptions, string shopCode, string posNo, string saleDate, string billNo,
            bool isPrintItem, bool isRePrint)
        {
            printOptions = CheckShowPrinterStatus(printOptions != PrintOptions.JournalOnly, false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "1")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                object[] datas = GetPosData(shopCode, posNo, saleDate, "00", billNo, null);
                PrintTemplateProcess template = new PrintTemplateProcess();

                string plainText = string.Empty;
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetReceiptText(prtForm, isPrintItem, isRePrint, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                TRN_CASH[] payCashs = datas[4] as TRN_CASH[];
                TRN_GIFT[] payGifts = datas[7] as TRN_GIFT[];
                TRN_FOODCPN[] payFoodCpns = datas[8] as TRN_FOODCPN[];
                if ((payCashs != null && payCashs.Length > 0) || (payGifts != null && payGifts.Length > 0) || (payFoodCpns != null && payFoodCpns.Length > 0))
                {
                    if (!isRePrint) OpenCashDrawer();
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetReceiptText(prtForm, isPrintItem, false, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = billNo,
                        PosNo = posNo,
                        SaleDate = saleDate,
                        ShopCode = shopCode
                    }, plainText);
                }
            }
        }

        public void PrintPrePaymentInput(OrderInfo info, bool isReprint = false)
        {
            var printOptions = CheckShowPrinterStatus(false).Item1 == ReceiptPrintReturns.CanPrint ? PrintOptions.Normal : PrintOptions.JournalOnly;

            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "21")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                NTRN_PRECHARGE_HEADER nTRN_HEADER = db.nTRN_PRECHARGE_HEADERs.FirstOrDefault(x =>
                    x.SHOP_CODE == info.ShopCode && x.SALE_DATE == info.SaleDate
                                                 && x.POS_NO == info.PosNo
                                                 && x.SALE_NO == info.BillNo);

                //get trn_prd
                var ntTRN_CARD = db.nTRN_PRECHARGE_CARDs.Where(x => x.SHOP_CODE == info.ShopCode &&
                                                                      x.SALE_DATE == info.SaleDate &&
                                                                      x.POS_NO == info.PosNo &&
                                                                      x.SALE_NO == info.BillNo).ToArray();

                var templateHandler = new PrintTemplateProcess();

                string plainText = string.Empty;
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = templateHandler.GetPrePaymentInputPrintText(prtForm, nTRN_HEADER, ntTRN_CARD, isReprint);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = templateHandler.GetPrePaymentInputPrintText(prtForm, nTRN_HEADER, ntTRN_CARD, isReprint);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }

                    JournalWrite(new OrderInfo
                    {
                        BillNo = info.BillNo,
                        PosNo = info.PosNo,
                        SaleDate = info.SaleDate,
                        ShopCode = info.ShopCode
                    }, plainText);
                }

            }

        }

        #endregion

        #region GetPreviewPrintForm

        public async Task<List<Dictionary<string, string>>> GetPreviewPrintForm(string shopCode, string posNo, string saleDate, string billNo, bool isPrintItem)
        {
            List<Dictionary<string, string>> sb = new List<Dictionary<string, string>>();
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "1")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return sb;
                }

                object[] datas = GetPosData(shopCode, posNo, saleDate, "00", billNo, null);
                PrintTemplateProcess template = new PrintTemplateProcess();
                string printText = template.GetReceiptText(prtForm, isPrintItem, false, datas);
                return Printer?.PreviewBill(printText);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="billNo"></param>
        /// <param name="isPrintItem"></param>
        /// <returns></returns>
        public string PreviewReceipt(string shopCode, string posNo, string saleDate, string billNo, bool isPrintItem)
        {
            List<Dictionary<string, string>> sb = new List<Dictionary<string, string>>();
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "1")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return string.Empty;
                }

                object[] datas = GetPosData(shopCode, posNo, saleDate, "00", billNo, null);
                PrintTemplateProcess template = new PrintTemplateProcess();
                string printText = template.GetReceiptText(prtForm, isPrintItem, false, datas);
                return Printer?.PrintReceiptPlain(printText);
            }
        }

        #endregion

        #region PrintCustomerOrder 고객주문서

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="trHeader"></param>
        /// <param name="trProducts"></param>
        /// <param name="trTenders"></param>
        /// <param name="trCashs"></param>
        /// <param name="trCashRecs"></param>
        /// <param name="trCards"></param>
        /// <param name="trGifts"></param>
        /// <param name="trFoodCpns"></param>
        /// <param name="trEasys"></param>
        /// <param name="trPartCards"></param>
        /// <param name="tRN_POINTUSE"></param>
        /// <param name="tRN_POINTSAVE"></param>
        public void PrintCustomerOrder(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, bool printItems, TRN_PRDT[] trProducts,
                    TRN_TENDERSEQ[] trTenders, TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_PARTCARD[] trPartCards, TRN_GIFT[] trGifts,
                    TRN_FOODCPN[] trFoodCpns, TRN_EASYPAY[] trEasys, TRN_POINTUSE[] tRN_POINTUSE, TRN_POINTSAVE tRN_POINTSAVE, TRN_PPCARD[] trn_PPCARD)
        {
            //do nothing
            printOptions = CheckShowPrinterStatus(printOptions != PrintOptions.JournalOnly, false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "20")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }
                object[] datas = new object[]
                {
                    trHeader, orderSumInfo, trProducts, trTenders, trCashs, trCashRecs, trCards, trGifts, trFoodCpns, trEasys, trPartCards, null,
                    tRN_POINTUSE, tRN_POINTSAVE, trn_PPCARD
                };

                PrintTemplateProcess template = new PrintTemplateProcess();
                string plainText = string.Empty;
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetCustomerOrderText(prtForm, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetCustomerOrderText(prtForm, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = trHeader?.BILL_NO,
                        PosNo = trHeader?.POS_NO,
                        SaleDate = trHeader?.SALE_DATE,
                        ShopCode = trHeader?.SHOP_CODE
                    }, plainText);
                }
            }
        }
        #endregion

        #region PrintFoodCoupon - 푸드교환권

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="trHeader"></param>
        /// <param name="orderSumInfo"></param>
        /// <param name="trProducts"></param>
        public void PrintFoodCoupon(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, TRN_PRDT[] trProducts)
        {
            printOptions = CheckShowPrinterStatus(printOptions != PrintOptions.JournalOnly, false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "18")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                object[] datas = new object[]
                {
                    trHeader, orderSumInfo, trProducts
                };

                PrintTemplateProcess template = new PrintTemplateProcess();
                var plainText = "";
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetFoodCouponText(prtForm, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetFoodCouponText(prtForm, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = trHeader?.BILL_NO,
                        PosNo = trHeader?.POS_NO,
                        SaleDate = trHeader?.SALE_DATE,
                        ShopCode = trHeader?.SHOP_CODE
                    }, plainText);
                }
            }
        }

        #endregion

        #region PrintKitchenOrder
        public ResultInfo PrintKitchenOrder(OrderInfo info)
        {
            var result = new ResultInfo();
            try
            {

                using (var db = new DataContext())
                {
                    //get template
                    var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                    p.PRT_CLASS_CODE == "2")?.PRT_FORM;
                    if (string.IsNullOrEmpty(prtForm))
                    {
                        result.Success = false;
                        result.Message = "템플릿을 찾을 수 없습니다.";
                        return result;
                    }

                    TRN_HEADER tRN_HEADER = db.tRN_HEADERs.FirstOrDefault(x =>
                             x.SHOP_CODE == info.ShopCode && x.SALE_DATE == info.SaleDate
                             && x.POS_NO == info.PosNo
                             && x.BILL_NO == info.BillNo);

                    //get trn_prd
                    var tRN_PRDTs = db.tRN_PRDTs.Where(x => x.SHOP_CODE == info.ShopCode &&
                                            x.SALE_DATE == info.SaleDate &&
                                            x.POS_NO == info.PosNo &&
                                            x.BILL_NO == info.BillNo).ToArray();


                    #region Get all device is printer

                    var templateHandler = new PrintTemplateProcess();
                    var printers = db.mST_INFO_KDSs.Where(p => p.KDS_TYPE == "P").ToArray();
                    foreach (var printerInfo in printers)
                    {
                        var printPrdts = (from trnPrd in tRN_PRDTs
                                          join kdsPrd in db.mST_INFO_KDS_PRDs.Where(p => p.KDS_NO == printerInfo.KDS_NO)
                                              on trnPrd.PRD_CODE equals kdsPrd.PRD_CODE
                                          select trnPrd).ToArray();

                        if (printPrdts.Length == 0) continue;
                        string dataJson = templateHandler.GetKitchenPrintText(prtForm, tRN_HEADER, printPrdts);
                        string outPrintText = string.Empty;
                        var arrayOfByteArrays = Printer?.PrintFormToByte(_logo, 0, dataJson, out outPrintText);

                        JournalWrite(new OrderInfo
                        {
                            BillNo = tRN_HEADER.BILL_NO,
                            PosNo = tRN_HEADER.POS_NO,
                            SaleDate = tRN_HEADER.SALE_DATE,
                            ShopCode = tRN_HEADER.SHOP_CODE
                        }, outPrintText);


                        switch (printerInfo.SEND_TYPE)
                        {
                            case "S":
                                IPAddress ipAddress = IPAddress.Parse(printerInfo.KDS_TCP_IP);
                                IPEndPoint printerEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(printerInfo.KDS_TCP_PORT));

                                Task.Factory.StartNew(() =>
                                {
                                    RWriteToNetPorts1(printerEndPoint, arrayOfByteArrays);
                                });
                                break;
                            case "C":
                                var printer = new GeneralPrinter();
                                var port = TypeHelper.ToInt32(printerInfo.KDS_TCP_PORT);
                                var bitrate = TypeHelper.ToInt32(printerInfo.KDS_SPEED);
                                if (bitrate == 0) bitrate = SystemHelper.SerialPortBitrateDefault;
                                var status = printer.TryOpen(port, bitrate);
                                if (status)
                                {
                                    printer.PrintBytes(arrayOfByteArrays);
                                }
                                printer.ClosePrinter();
                                printer.Dispose();
                                break;
                            default:
                                Printer?.PrintForm(_logo, 0, dataJson);
                                break;
                        }
                    }

                    #endregion

                    result.Success = true;
                    result.Message = "Ok";
                }

                return result;
            }
            catch (Exception ex)
            {
                //string str = line;
                Console.WriteLine(ex.ToString());
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
        public Task<ResultInfo> PrintKitchenOrderAsyn(OrderInfo info)
        {
            return Task.Factory.StartNew(() => { return PrintKitchenOrder(info); });
        }
        #endregion

        #region PrintSimplifiedReceipt
        public void PrintSimplifiedReceipt(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, bool printItems, TRN_PRDT[] trProducts,
                                            TRN_TENDERSEQ[] trTenders, TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns)
        {
            //do nothing
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "27")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }
                //string data = headerObject.ToString();
                string data = "";
                //Printer?.PrintBill(prtForm, _logo, 1, printItems,false, false);
            }
        }

        public void PrintIntermediateBill(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, bool printItems, TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders, TRN_CASH[] trCashs,
            TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns)
        {
            //do nothing
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "29")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }
                //JObject headerObject = JObject.FromObject(trHeader);
                //string data = headerObject.ToString();
                //Printer?.PrintDataWithTemplate(data, prtForm, PrintLogo, 1, printItems);
            }
        }
        #endregion

        #region PrintPrevBill

        public void PrintPrevBill()
        {
            if (DataLocals.PosStatus.BILL_NO == "0000")
            {
                return;
            }

            PrintReceipt(PrintOptions.Normal, DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO,
                DataLocals.PosStatus.SALE_DATE, DataLocals.PosStatus.BILL_NO, true, true);
            EndPrinting();
        }

        #endregion

        #region PrintMiddleSettle PrintCloseSettle

        /// <summary>
        /// 중간정산지
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="LastSettAccount"></param>
        public void PrintMiddleSettle(PrintOptions printOptions, SETT_POSACCOUNT LastSettAccount)
        {
            printOptions = CheckShowPrinterStatus(false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "3")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                object[] datas = GetPosData(DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE,
                                                                    LastSettAccount.REGI_SEQ, string.Empty, LastSettAccount);
                PrintTemplateProcess template = new PrintTemplateProcess();
                var plainText = "";
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetSettleText(true, prtForm, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetSettleText(true, prtForm, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = DataLocals.PosStatus.BILL_NO,
                        PosNo = DataLocals.PosStatus.POS_NO,
                        SaleDate = DataLocals.PosStatus.SALE_DATE,
                        ShopCode = DataLocals.AppConfig.PosInfo.StoreNo
                    }, plainText);
                }
            }
        }

        /// <summary>
        /// 마감전표
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="LastSettAccount"></param>
        public void PrintCloseSettle(PrintOptions printOptions, SETT_POSACCOUNT LastSettAccount)
        {
            printOptions = CheckShowPrinterStatus(false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "4")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                object[] datas = GetPosData(DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO,
                            DataLocals.PosStatus.SALE_DATE, DataLocals.PosStatus.REGI_SEQ, DataLocals.PosStatus.BILL_NO, LastSettAccount);
                PrintTemplateProcess template = new PrintTemplateProcess();

                var plainText = "";
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetSettleText(false, prtForm, datas);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetSettleText(false, prtForm, datas);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = DataLocals.PosStatus.BILL_NO,
                        PosNo = DataLocals.PosStatus.POS_NO,
                        SaleDate = DataLocals.PosStatus.SALE_DATE,
                        ShopCode = DataLocals.AppConfig.PosInfo.StoreNo
                    }, plainText);
                }

            }
        }

        #endregion

        /// <summary>
        /// 시재점검
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="regiSeq"></param>
        public void PrintTrialCheck(PrintOptions printOptions, string shopCode, string posNo, string saleDate, string regiSeq)
        {
            printOptions = CheckShowPrinterStatus(false).Item1 == ReceiptPrintReturns.CanPrint ? printOptions : PrintOptions.JournalOnly;
            using (var db = new DataContext())
            {
                //get template
                var prtForm = db.mST_FORM_PRINTERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.PRT_CLASS_CODE == "5")?.PRT_FORM;
                if (string.IsNullOrEmpty(prtForm))
                {
                    return;
                }

                PrintTemplateProcess template = new PrintTemplateProcess();

                var plainText = "";
                if (printOptions != PrintOptions.JournalOnly)
                {
                    string printText = template.GetTrialCheckPrintText(prtForm, shopCode, posNo, saleDate, regiSeq);
                    plainText = Printer?.PrintForm(_logo, 0, printText);
                }

                if (printOptions == PrintOptions.Normal || printOptions == PrintOptions.JournalOnly)
                {
                    if (string.IsNullOrEmpty(plainText))
                    {
                        string printText = template.GetTrialCheckPrintText(prtForm, shopCode, posNo, saleDate, regiSeq);
                        plainText = Printer?.PrintReceiptPlain(printText);
                    }
                    JournalWrite(new OrderInfo
                    {
                        BillNo = regiSeq,
                        PosNo = posNo,
                        SaleDate = saleDate,
                        ShopCode = shopCode
                    }, plainText);
                }
            }
        }
        #endregion

        #region WriteToNetPort
        public (bool, string) WriteToNetPort(IPEndPoint printerEndPoint, byte[][] sendBytes)
        {
            try
            {
                using (Socket printerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    printerSocket.Connect(printerEndPoint);

                    foreach (var packet in sendBytes)
                    {
                        printerSocket.Send(packet.ToArray());
                    }
                    printerSocket.Shutdown(SocketShutdown.Both);
                    printerSocket.Close();
                    Console.WriteLine("Data sent to printer.");
                    return new(true, "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new(false, ex.Message);
            }
        }
        public ResultInfo RWriteToNetPort(IPEndPoint printerEndPoint, byte[][] sendBytes)
        {
            var result = new ResultInfo();
            try
            {
                using (Socket printerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    printerSocket.Connect(printerEndPoint);

                    foreach (var packet in sendBytes)
                    {
                        printerSocket.Send(packet.ToArray());
                    }
                    printerSocket.Shutdown(SocketShutdown.Both);
                    printerSocket.Close();
                    Console.WriteLine("Data sent to printer.");
                    result.Success = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public ResultInfo RWriteToNetPorts(IPEndPoint printerEndPoint, List<byte> sendBytes)
        {
            var result = new ResultInfo();
            try
            {
                using (Socket printerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    printerSocket.Connect(printerEndPoint);
                    printerSocket.Send(sendBytes.ToArray());
                    printerSocket.Shutdown(SocketShutdown.Both);
                    printerSocket.Close();
                    Console.WriteLine("Data sent to printer.");
                    result.Success = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public ResultInfo RWriteToNetPorts1(IPEndPoint printerEndPoint, List<byte[]> sendBytes)
        {
            var result = new ResultInfo();
            try
            {
                using (Socket printerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    printerSocket.Connect(printerEndPoint);
                    foreach (var item in sendBytes)
                    {
                        printerSocket.Send(item);
                    }

                    printerSocket.Shutdown(SocketShutdown.Both);
                    printerSocket.Close();
                    Console.WriteLine("Data sent to printer.");
                    result.Success = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public static void WriteToNetPort_Test(string ip, int port)
        {
            try
            {
                using (Socket printerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPAddress printerIp = IPAddress.Parse(ip);
                    IPEndPoint printerEndPoint = new IPEndPoint(printerIp, port);

                    printerSocket.Connect(printerEndPoint);
                    List<byte> sendBytes = new List<byte>();
                    byte[] receipt = { 0x20, 0x20, 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x4C , 0x61 , 0x73 , 0x20 , 0x76 , 0x65 , 0x67 , 0x61 , 0x73 , 0x2C , 0x4E , 0x56 , 0x35 , 0x32 , 0x30 , 0x38 , 0x0D , 0x0A, 0x0A
                             , 0x54 , 0x69 , 0x63 , 0x6B , 0x65 , 0x74 , 0x20 , 0x23 , 0x33 , 0x30 , 0x2D , 0x35 , 0x37 , 0x33 , 0x32 , 0x30 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x55 , 0x73 , 0x65 , 0x72 , 0x3A , 0x48 , 0x41 , 0x50 , 0x50 , 0x59 , 0x0D , 0x0A
                             , 0x53 , 0x74 , 0x61 , 0x74 , 0x69 , 0x6F , 0x6E , 0x3A , 0x35 , 0x32 , 0x2D , 0x31 , 0x30 , 0x32 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x53 , 0x61 , 0x6C , 0x65 , 0x73 , 0x20 , 0x52 , 0x65 , 0x70 , 0x20 , 0x48 , 0x41 , 0x50 , 0x50 , 0x59 , 0x0D , 0x0A
                             , 0x31 , 0x30 , 0x2F , 0x31 , 0x30 , 0x2F , 0x32 , 0x30 , 0x31 , 0x39 , 0x20 , 0x33 , 0x3A , 0x35 , 0x35 , 0x3A , 0x30 , 0x31 , 0x50 , 0x4D , 0x0D , 0x0A
                             , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x0D , 0x0A
                             , 0x49 , 0x74 , 0x65 , 0x6D , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x51 , 0x54 , 0x59 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x50 , 0x72 , 0x69 , 0x63 , 0x65 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x54 , 0x6F , 0x74 , 0x61 , 0x6C , 0x0D , 0x0A
                             , 0x44 , 0x65 , 0x73 , 0x63 , 0x72 , 0x69 , 0x70 , 0x74 , 0x69 , 0x6F , 0x6E , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x0D , 0x0A
                             , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x0D , 0x0A
                             , 0x31 , 0x30 , 0x30 , 0x33 , 0x32 , 0x38 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x31 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x37 , 0x2E , 0x39 , 0x39 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x37 , 0x2E , 0x39 , 0x39 , 0x0D , 0x0A
                             , 0x4D , 0x41 , 0x47 , 0x41 , 0x52 , 0x49 , 0x54 , 0x41 , 0x20 , 0x4D , 0x49 , 0x58 , 0x0D , 0x0A
                             , 0x36 , 0x38 , 0x30 , 0x30 , 0x31 , 0x35 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x30 , 0x2E , 0x39 , 0x39 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x33 , 0x2E , 0x39 , 0x36 , 0x0D , 0x0A
                             , 0x4C , 0x49 , 0x4D , 0x45 , 0x0D , 0x0A
                             , 0x31 , 0x30 , 0x32 , 0x35 , 0x30 , 0x31 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x31 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x33 , 0x2E , 0x39 , 0x39 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x33 , 0x2E , 0x39 , 0x39 , 0x0D , 0x0A
                             , 0x56 , 0x4F , 0x44 , 0x4B , 0x41 , 0x0D , 0x0A
                             , 0x30 , 0x32 , 0x31 , 0x30 , 0x34 , 0x38 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x31 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x2E , 0x39 , 0x39 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x2E , 0x39 , 0x39 , 0x0D , 0x0A
                             , 0x4F , 0x52 , 0x41 , 0x4E , 0x47 , 0x45 , 0x20 , 0x33 , 0x32 , 0x4F , 0x5A , 0x0D , 0x0A
                             , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x0D , 0x0A
                             , 0x53 , 0x75 , 0x62 , 0x74 , 0x6F , 0x62 , 0x61 , 0x6C , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x36 , 0x30 , 0x2E , 0x39 , 0x33 , 0x0D , 0x0A
                             , 0x38 , 0x2E , 0x31 , 0x25 , 0x20 , 0x53 , 0x61 , 0x6C , 0x65 , 0x73 , 0x20 , 0x54 , 0x61 , 0x78 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x34 , 0x2E , 0x32 , 0x31 , 0x0D , 0x0A
                             , 0x32 , 0x25 , 0x20 , 0x43 , 0x6F , 0x6E , 0x63 , 0x65 , 0x73 , 0x73 , 0x69 , 0x6F , 0x6E , 0x20 , 0x52 , 0x65 , 0x63 , 0x6F , 0x76 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x31 , 0x2E , 0x30 , 0x34 , 0x0D , 0x0A
                             , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x2D , 0x0D , 0x0A
                             , 0x54 , 0x6F , 0x74 , 0x61 , 0x6C , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x20 , 0x36 , 0x36 , 0x2E , 0x31 , 0x38

                             , 0x20 , 0x20 , 0x20 , 0x20 , 0x0A,
                            0x1b , 0x7b , 0x00 , 0x1b , 0x74 , 0x00 , 0x1b , 0x4d ,  0x00 , 0x1b , 0x20 , 0x00 ,  0x1b , 0x32 , 0x1b , 0x45 , 0x00 , 0x1b , 0x2d , 0x30 , 0x1d , 0x42 , 0x00 , 0x1d , 0x21 , 0x00 , 0x1b , 0x61 , 0x30 , 0x1b , 0x61 , 0x01 ,
                            0x1d , 0x48 , 0x30 , 0x1d , 0x77 , 0x02 , 0x1d , 0x68 ,  0x80 , 0x1d , 0x6b , 0x49 ,  0x0e , 0x43 , 0x6f , 0x75 , 0x6e , 0x74 , 0x30 , 0x31 , 0x32 , 0x33 , 0x34 , 0x35 , 0x36 , 0x37 , 0x21 , 0x1b , 0x61 , 0x00 , 0x1b , 0x24 ,
                            00 , 0x00 , 0x0A};
                    foreach (byte tempbyte in receipt)
                    {
                        sendBytes.Add(tempbyte);
                    }
                    foreach (byte tempbyte in CutPaper(66, 01))
                    {
                        sendBytes.Add(tempbyte);
                    }

                    printerSocket.Send(sendBytes.ToArray());

                    printerSocket.Shutdown(SocketShutdown.Both);
                    printerSocket.Close();

                    Console.WriteLine("Data sent to printer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion

        private static byte[] CutPaper(int m, int n)
        {
            byte[] cutpaper = new byte[] { 29, 86, (byte)m, (byte)n };
            return cutpaper;
        }

        public void OpenCashDrawer()
        {
            Printer?.OpenCashDraw();
        }

        public void OpenCashDrawer(bool needOpen)
        {
            try
            {
                if (needOpen)
                    TryOpen();
                Printer?.OpenCashDraw();
            }
            catch
            {
            }
            finally
            {
                if (needOpen)
                    EndPrinting();
            }
        }

        public void Dispose()
        {
            Printer?.Dispose();
        }

        #region Helper functions

        private object[] GetPosData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo, SETT_POSACCOUNT LastSettAccount)
        {
            List<object> lstDatas = new List<object>();
            try
            {
                using (var db = new DataContext())
                {
                    //get infoShop
                    MST_INFO_SHOP mST_INFO_SHOP = db.mST_INFO_SHOPs.FirstOrDefault(x => x.SHOP_CODE == shopCode);

                    //get transaction header
                    TRN_HEADER tRN_HEADER = db.tRN_HEADERs.FirstOrDefault(x =>
                            x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                            && x.POS_NO == posNo
                            && (regiSeq == "00" || x.REGI_SEQ == regiSeq)
                            && x.BILL_NO == billNo);

                    lstDatas.Add(tRN_HEADER);

                    //get order sum info cái này để tính lại
                    ORDER_SUM_INFO oRDER_SUM_INFO = new ORDER_SUM_INFO();
                    lstDatas.Add(oRDER_SUM_INFO);

                    //get trn_prd
                    TRN_PRDT[] tRN_PRDTs = db.tRN_PRDTs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_PRDTs);

                    //get trn_tenderSeq
                    TRN_TENDERSEQ[] tRN_TENDERSEQs = db.tRN_TENDERSEQs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();
                    lstDatas.Add(tRN_TENDERSEQs);

                    //get trn CASH
                    TRN_CASH[] tRN_CASHes = db.tRN_CASHes.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();
                    lstDatas.Add(tRN_CASHes);


                    TRN_CASHREC[] tRN_CASHRECs = db.tRN_CASHRECs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                             && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_CASHRECs);

                    //get trn CARD
                    TRN_CARD[] tRN_CARDs = db.tRN_CARDs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_CARDs);

                    //get tRN_GIFTs
                    TRN_GIFT[] tRN_GIFTs = db.tRN_GIFTs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_GIFTs);

                    //get tRN_FOODCPNs
                    TRN_FOODCPN[] tRN_FOODCPNs = db.tRN_FOODCPNs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_FOODCPNs);

                    //get tRN_EASYPAYs
                    TRN_EASYPAY[] tRN_EASYPAYs = db.tRN_EASYPAYs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                             && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_EASYPAYs);

                    //get TRN_PARTCARDs
                    TRN_PARTCARD[] tRN_PARTCARDs = db.tRN_PARTCARDs.Where(x => x.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo && x.SALE_DATE == saleDate
                                                                              && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();

                    lstDatas.Add(tRN_PARTCARDs);

                    lstDatas.Add(LastSettAccount);

                    TRN_POINTUSE[] tRN_POINTUSE = db.tRN_POINTUSEs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                             && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();
                    lstDatas.Add(tRN_POINTUSE);

                    TRN_POINTSAVE tRN_POINTSAVE = db.tRN_POINTSAVEs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                             && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).FirstOrDefault();
                    lstDatas.Add(tRN_POINTSAVE);

                    TRN_PPCARD[] tRN_PPCARD = db.tRN_PPCARDs.Where(x => x.SHOP_CODE == shopCode && x.SALE_DATE == saleDate
                                                                             && x.POS_NO == posNo && (regiSeq == "00" || x.REGI_SEQ == regiSeq) && (billNo == "" || x.BILL_NO == billNo)).ToArray();
                    lstDatas.Add(tRN_PPCARD);
                    return lstDatas.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
            }

            return lstDatas.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="printText"></param>
        private void JournalWrite(OrderInfo order, string printText)
        {
            var test = @$"
**----------------------------------------------------------
ShopCode:{order.ShopCode}, SaleDate:{order.SaleDate}, PosNo:{order.PosNo}, BillNo:{order.BillNo}
**----------------------------------------------------------
{printText}";
            LogHelper.Logger.Fatal(test);
        }
  

        #endregion
    }
    #endregion
}