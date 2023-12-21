using GoPOS.Common.PrinterLib;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoShared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static GoPOS.Common.PrinterLib.protocol.BasicProtocol;

namespace GoPOS.Common.Service
{
    public interface IPOSPrintService
    {
        GeneralPrinter Printer { get; set; }

        Task<bool> TryOpen();
        void EndPrinting();
        void OpenCashDrawer(bool needOpen);
        void OpenCashDrawer();
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printYN"></param>
        /// <param name="silenceMode"></param>
        /// <returns></returns>
        (ReceiptPrintReturns, PrintStatus) CheckShowPrinterStatus(bool silenceMode);
        (ReceiptPrintReturns, PrintStatus) CheckShowPrinterStatus(bool printYN, bool silenceMode);

        /// <summary>
        /// 영수증출력
        /// </summary>
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
        void PrintReceipt(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo,
                bool printItems,
                TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders,
                TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards,
                TRN_PARTCARD[] trnPartCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns, TRN_EASYPAY[] trEasypays, 
                TRN_POINTUSE[] tRN_POINTUSE, TRN_POINTSAVE tRN_POINTSAVE, TRN_PPCARD[] tRN_PPCARD, MEMBER_CLASH MemberInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="billNo"></param>
        /// <param name="isPrintItem"></param>
        /// <param name="isRePrint"></param>
        void PrintReceipt(PrintOptions printOptions, string shopCode, string posNo, string saleDate, string billNo, bool isPrintItem, bool isRePrint);


        /// <summary>
        /// 주방주문서     ->    26 
        /// </summary>
        /// <param name="OrderInfo"></param>

        void PrintPrePaymentInput(OrderInfo info,bool isReprint=false);

        ResultInfo PrintKitchenOrder(OrderInfo info);

        Task<ResultInfo> PrintKitchenOrderAsyn(OrderInfo info);


        /// <summary>
        /// 고객 주문서
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
        void PrintCustomerOrder(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, bool printItems, TRN_PRDT[] trProducts,
                    TRN_TENDERSEQ[] trTenders, TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_PARTCARD[] trPartCards, TRN_GIFT[] trGifts,
                    TRN_FOODCPN[] trFoodCpns, TRN_EASYPAY[] trEasys, TRN_POINTUSE[] tRN_POINTUSE, TRN_POINTSAVE tRN_POINTSAVE, TRN_PPCARD[] tRN_PPCARD);

        /// <summary>
        /// 푸드교환권
        /// </summary>
        /// <param name="printOptions"></param>
        /// <param name="trHeader"></param>
        /// <param name="orderSumInfo"></param>
        /// <param name="trProducts"></param>
        void PrintFoodCoupon(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo, TRN_PRDT[] trProducts);

        /// <summary>
        /// 간이영수증    ->    27 
        /// </summary>
        /// <param name="trHeader"></param>
        /// <param name="orderSumInfo"></param>
        /// <param name="printItems"></param>
        /// <param name="trProducts"></param>
        /// <param name="trTenders"></param>
        /// <param name="trCashs"></param>
        /// <param name="trCashRecs"></param>
        /// <param name="trCards"></param>
        /// <param name="trGifts"></param>
        /// <param name="trFoodCpns"></param>
        void PrintSimplifiedReceipt(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo,
            bool printItems,
            TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders,
            TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards,
            TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns);

        /// <summary>
        /// 중간계산서
        /// </summary>
        /// <param name="trHeader"></param>
        /// <param name="orderSumInfo"></param>
        /// <param name="printItems"></param>
        /// <param name="trProducts"></param>
        /// <param name="trTenders"></param>
        /// <param name="trCashs"></param>
        /// <param name="trCashRecs"></param>
        /// <param name="trCards"></param>
        /// <param name="trGifts"></param>
        /// <param name="trFoodCpns"></param>
        void PrintIntermediateBill(PrintOptions printOptions, TRN_HEADER trHeader, ORDER_SUM_INFO orderSumInfo,
        bool printItems,
        TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders,
        TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards,
        TRN_GIFT[] trGifts, TRN_FOODCPN[] trFoodCpns);

        void PrintMiddleSettle(PrintOptions printOptions, SETT_POSACCOUNT LastSettAccount);

        void PrintCloseSettle(PrintOptions printOptions, SETT_POSACCOUNT LastSettAccount);

        void PrintPrevBill();

        Task<List<Dictionary<string, string>>> GetPreviewPrintForm(string shopCode, string posNo, string saleDate, string billNo, bool isPrintItem);

        void PrintTrialCheck(Common.PrinterLib.PrintOptions printOptions, string shopCode, string posNo, string saleDate, string regiSeq);

        string PreviewReceipt(string shopCode, string posNo, string saleDate, string billNo, bool isPrintItem);

        ResultInfo RWriteToNetPort(IPEndPoint printerEndPoint, byte[][] sendBytes);
        ResultInfo RWriteToNetPorts(IPEndPoint printerEndPoint, List<byte> sendBytes);
    }
}
