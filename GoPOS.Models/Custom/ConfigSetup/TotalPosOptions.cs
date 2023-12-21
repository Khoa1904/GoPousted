using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.ConfigSetup
{
    public class TotalPosOptions
    {
        [PosOptionAttr(0, "524")]
        public PrintReceiptTypes PrintReceiptType { get; set; }

        /// <summary>
        /// 구매내역 영수증발행 문의여부
        /// </summary>
        [PosOptionAttr(0, "525")]
        public bool ReceiptPrintAsk { get; set; }
        public const string ReceiptPrintAskMsg = "영수증출력을 하시겠습니까?";

        /// <summary>
        /// 교환권번호출력
        /// (1): 영수증번호출력
        /// (2) : 포스직접입력
        /// </summary>
        [PosOptionAttr(0, "152")]
        public byte CouponNoPrintOpt { get; set; }

        /// <summary>
        /// 교환번호출력
        /// </summary>
        [PosOptionAttr(0, "152")]
        public bool CouponNoPrintYN { get; set; }

        /// <summary>
        /// 교환권출력여부
        /// </summary>
        [PosOptionAttr(0, "000")]
        public bool CouponPrintYN { get; set; }

        /// <summary>
        /// 고객주문서 출력여부
        /// </summary>
        [PosOptionAttr(0, "503")]
        public bool CustOrderPrintYN { get; set; }

        /// <summary>
        /// [507] 주방주문서 출력여부
        /// </summary>
        [PosOptionAttr(0, "507")]
        public bool KitOrderPrintYN { get; set; }

        /// <summary>
        /// [238] 주방프린터출력제어
        /// </summary>
        [PosOptionAttr(0, "238")]
        public bool KitOrderPrintOpt { get; set; }

        /// <summary>
        /// [723] 고객주문서 출력제어
        /// </summary>
        [PosOptionAttr(0, "723")]
        public bool CustOrderPrintOpt { get; set; }

        /// <summary>
        /// [743] 상품내역 출력제어
        /// </summary>
        [PosOptionAttr(0, "743")]
        public bool BillItemsPrintOpt { get; set; }

        /// <summary>
        /// [294] 신용카드-임의등록버튼
        /// </summary>
        [PosOptionAttr(1, "743")]
        public bool PayCardManualUseYN { get; set; }

        /// <summary>
        /// [201] 포스-본체종류
        /// </summary>
        [PosOptionAttr(1, "201")]
        public string POSMachineOpt { get; set; }

        /// <summary>
        /// [202] 포스-용도
        /// </summary>
        [PosOptionAttr(1, "202")]
        public string POSUseTypeOpt { get; set; }

        /// <summary>
        /// [205] 포스-메인여부
        /// 0: main
        /// 1: Sub
        /// </summary>
        [PosOptionAttr(1, "205")]
        public byte POSFlag { get; set; }

        /// <summary>
        /// [204] 포스-형태
        /// 0: 선불
        /// 1: 후불
        /// </summary>
        [PosOptionAttr(1, "205")]
        public byte POSSaleType { get; set; }

        
        public bool ReceiptPrinterYN { get; set; }

        /// <summary>
        /// [206] 영수프린터-종류
        /// </summary>
        [PosOptionAttr(1, "206")]
        public string PrinterTypeCode { get; set; }

        /// <summary>
        /// [207] 영수프린터-포트
        /// </summary>
        [PosOptionAttr(1, "207")]
        public string ReceiptPrintPort { get; set; }

        /// <summary>
        /// [208] 영수프린터-속도
        /// </summary>
        [PosOptionAttr(1, "208")]
        public int ReceiptPrintSpeed { get; set; }

        /// <summary>
        /// [212] 듀얼모니터 사용여부
        /// </summary>
        [PosOptionAttr(1, "212")]
        public int DualMonitorUseYN { get; set; }

        /// <summary>
        /// [532] 사이드메뉴 처리구분
        /// </summary>
        [PosOptionAttr(1, "532")]
        public byte SideMenuProOpt { get; set; }

        /// <summary>
        /// [737] 사이드메뉴 개별수량변경
        /// </summary>
        [PosOptionAttr(1, "737")]
        public byte SideMenuQtyIncOpt { get; set; }
    }

}
