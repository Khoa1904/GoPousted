using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GoShared.Helpers;
using System.DirectoryServices;

namespace GoPOS.Models.Config
{
    public class PosOption
    {
        public static int SyncTrnTime { get; set; } = 10; //  sending cycle time(seconds)
        public static int Send_RecordPick { get; set; } = 30; // maximum of records per send
        public static int Send_ErorCount { get; set; } = int.MaxValue; // number of retry on error
        //---------KDS----------------------------------------------------------------
        public static int SyncKDSTime { get; set; } = 10; //  sending cycle time(seconds)
        public static int KDSErrorCount { get; set; } = 10; // number of retry on error
        public static int PrtErrorCount { get; set; } = 10; // number of retry on error

        public string KitchenPRTPort { get; set; }
        public string KitchenPRTIp { get; set; }

        #region Added PosOptions

        [PosOptionAttr(0, "001", true)]
        public string PrintReceiptType { get; set; }

        /// <summary>
        /// 구매내역 영수증발행 문의여부
        /// </summary>
        [PosOptionAttr(0, "002", true)]
        public string ReceiptPrintAsk { get; set; }
        public string ReceiptPrintAskMsg = "영수증출력을 하시겠습니까?";

        /// <summary>
        /// 교환권번호출력
        /// (1): 영수증번호출력
        /// (2) : 포스직접입력
        /// </summary>
        [PosOptionAttr(0, "003", true)]
        public string CouponNoPrintOpt { get; set; }

        /// <summary>
        /// 교환권출력여부
        /// </summary>
        [PosOptionAttr(0, "004", true)]
        public string FoodCouponPrintYN { get; set; }

        /// <summary>
        /// 고객주문서 출력여부
        /// </summary>
        [PosOptionAttr(0, "005", true)]
        public string CustOrderPrintYN { get; set; }

        /// <summary>
        /// [507] 주방주문서 출력여부
        /// </summary>
        [PosOptionAttr(0, "006", true)]
        public string KitOrderPrintYN { get; set; }

        /// <summary>
        /// [238] 주방프린터출력제어
        /// </summary>
        [PosOptionAttr(0, "007", true)]
        public string KitOrderPrintOpt { get; set; }

        /// <summary>
        /// [723] 고객주문서 출력제어
        /// </summary>
        [PosOptionAttr(0, "008", true)]
        public string CustOrderPrintOpt { get; set; }

        /// <summary>
        /// [743] 상품내역 출력제어
        /// </summary>
        [PosOptionAttr(0, "009", true)]
        public string BillItemsPrintOpt { get; set; }

        /// <summary>
        /// [294] 신용카드-임의등록버튼
        /// </summary>
        [PosOptionAttr(1, "010", true)]
        public string PayCardManualUseYN { get; set; }

        /// <summary>
        /// [201] 포스-본체종류
        /// </summary>
        [PosOptionAttr(1, "011", true)]
        public string POSMachineOpt { get; set; }

        /// <summary>
        /// [202] 포스-용도
        /// </summary>
        [PosOptionAttr(1, "012", true)]
        public string POSUseTypeOpt { get; set; }

        /// <summary>
        /// [532] 사이드메뉴 처리구분
        /// </summary>
        [PosOptionAttr(1, "013", true)]
        public string SideMenuProOpt { get; set; }

        /// <summary>
        /// [737] 사이드메뉴 개별수량변경
        /// </summary>
        [PosOptionAttr(1, "014", true)]
        public string SideMenuQtyIncOpt { get; set; }

        /// <summary>
        /// [204] 포스-형태
        /// 0: 선불
        /// 1: 후불
        /// </summary>
        [PosOptionAttr(1, "204", true)]
        public string POSSaleType { get; set; }

        /// <summary>
        /// [205] 포스-메인여부
        /// 0: main
        /// 1: Sub
        /// </summary>
        [PosOptionAttr(1, "205", true)]
        public string POSFlag { get; set; }

        /// <summary>
        /// 0: use point
        /// 1: use stamp
        /// </summary>
        [PosOptionAttr(0, "017", true)]
        public string PointStampFlag { get; set; }

        /// <summary>
        /// 0: use amount
        /// 1: use qty of stamp
        /// </summary>
        [PosOptionAttr(0, "018", true)]
        public string StampUseMethod { get; set; }

        /// <summary>
        /// 0: print
        /// 1: not print
        /// 2: ask
        /// </summary>
        [PosOptionAttr(1, "790", true)]
        public string SettlePrintFlag { get; set; }

        /// <summary>
        /// 0000000: 후불제				
        /// 0000001: 선불제				
        /// </summary>
        [PosOptionAttr(1, "204", true)]
        public string POSOrderMethod { get; set; }

        public bool ReceiptPrinterYN
        {
            get
            { 
                return TypeHelper.ToInt32(PrinterTypeCode) != 0;
            }
        }

        /// <summary>
        /// [206] 영수프린터-종류
        /// </summary>
        [PosOptionAttr(1, "206", true)]
        public string PrinterTypeCode { get; set; }

        /// <summary>
        /// [207] 영수프린터-포트
        /// </summary>
        [PosOptionAttr(1, "207", false, true)]
        public string ReceiptPrintPort { get; set; }

        /// <summary>
        /// [208] 영수프린터-속도
        /// </summary>
        [PosOptionAttr(1, "208", false, true)]
        public string ReceiptPrintSpeed { get; set; }

        /// <summary>
        /// [212] 듀얼모니터 사용여부
        /// </summary>
        [PosOptionAttr(1, "212", true)]
        public string DualMonitorUseYN { get; set; }

       
        /// <summary>
        /// 0:출력/1:출력안함/2:조건출력
        /// </summary>
        [PosOptionAttr(1, "790", true)]
        public string PrintSettleFlag { get; set; }


        #endregion

        public PosOption()
        {
        }
    }


    public enum PrintReceiptTypes
    {
        NormalReceipt = 0, // 일반영수증출력
        SlipReceipt = 2 // 전표출력
    }
}
