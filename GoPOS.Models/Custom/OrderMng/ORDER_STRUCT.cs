using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoShared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    #region OrderPay

    //============================== using
    public class ORDER_FUNC_KEY
    {

        /*
        FK_NO
        FK_NAME
        AUTH_YN     
        IMG_FILE_NAME
        POSITION_NO
        COL_NUM
        ROW_NUM
        WIDTH_NUM
        HEIGHT_NUM
        USE_YN        
         */
        public string FK_NO { get; set; } = string.Empty;
        public string FK_NAME { get; set; } = string.Empty;
        public string AUTH_YN { get; set; } = string.Empty;
        public string IMG_FILE_NAME { get; set; } = string.Empty;
        public string POSITION_NO { get; set; } = string.Empty;
        public Int16 COL_NUM { get; set; }
        public Int16 ROW_NUM { get; set; }
        public Int16 WIDTH_NUM { get; set; }
        public Int16 HEIGHT_NUM { get; set; }
        public string USE_YN { get; set; } = string.Empty;
        public string ExternData { get; set; }
    };

    public class ORDER_TU_PRODUCT
    {
        /// <summary>
        /// 매장코드
        /// </summary>
        [Comment("매장코드")]
        public string? SHOP_CODE { get; set; }
        /// <summary>
        /// 판매주문구분 (S:판매 O:발주)
        /// </summary>
        [Comment("판매주문구분 (S:판매 O:발주)")]
        public string? TU_FLAG { get; set; }
        /// <summary>
        /// 터치분류코드
        /// </summary>
        [Comment("터치분류코드")]
        public string? TU_CLASS_CODE { get; set; }
        /// <summary>
        /// 터치키코드
        /// </summary>
        [Comment("터치키코드")]
        public string? TU_KEY_CODE { get; set; }
        /// <summary>
        /// 상품코드
        /// </summary>
        [Comment("상품코드")]
        public string? PRD_CODE { get; set; }

        [Comment("상품명")]
        public string? PRD_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Comment("세금여부")]
        public string? TAX_YN { get; set; }

        [Comment("판매단가")]
        public decimal SALE_UPRC { get; set; }
        /// <summary>
        /// 페이지수
        /// </summary>
        [Comment("페이지수")]
        public Int16 TU_PAGE { get; set; }
        /// <summary>
        /// 위치X
        /// </summary>
        [Comment("위치X")]
        public Int16 X { get; set; }
        /// <summary>
        /// 위치Y
        /// </summary>
        [Comment("위치Y")]
        public Int16 Y { get; set; }
        /// <summary>
        /// 폭
        /// </summary>
        [Comment("폭")]
        public Int16 WIDTH { get; set; }
        /// <summary>
        /// 높이
        /// </summary>
        [Comment("높이")]
        public Int16 HEIGHT { get; set; }
        [Comment("사이드")]
        public string SIDE_MENU_YN { get; set; }
        public string SET_PRD_FLAG { get; set; }

        /// <summary>
        /// 0: NORMAL, 1: SELECTIVE
        /// </summary>
        public string PRD_TYPE_CODE { get; set; }

        /// <summary>
        /// 품절
        /// </summary>
        public string? STOCK_OUT_YN { get; set; }
        public string PRD_DC_FLAG { get; set; }
        public string CST_ACCDC_YN { get; set; }
        public string STAMP_ACC_YN { get; set; }
        public string STAMP_USE_YN { get; set; }
        
        public int STAMP_ACC_QTY { get; set; }
        public int STAMP_USE_QTY { get; set; }

        /// <summary>
        /// 싯가상품 = 1
        /// 
        /// </summary>
        public string PRICE_MGR_FLAG { get; set; }
    }
    

    public class ORDER_SIDE_CLS_MENU
    {
        public string CLASS_CODE { get; set; }
        public string CLASS_NAME { get; set; }
        public string SUB_CODE { get; set; }
        public string SUB_NAME { get; set; }
        public int ORDER_SEQ { get; set; }
        public int MAX_QTY { get; set; }
        public decimal UNIT_PRICE { get; set; }

        /// <summary>
        /// P - 속성
        /// S - 선택
        /// </summary>
        public string CLASS_TYPE { get; set; }

        /// <summary>
        /// 0: main
        /// 1: side
        /// </summary>
        public string PRD_TYPE_FLAG { get; set; }
        public string TAX_YN { get; set; }
        public string PRD_CODE { get; set; }
        public string SHOP_CODE { get; set; }
    }

    //==============================


    public class ORDER_GRID_ITEM : INotifyPropertyChanged
    {
        private string _NODIS = "";
        public string NODIS { get => _NODIS; 
            set 
            {
                _NODIS = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NODIS"));
            }
        }
        
        private int _NO = 0;
        public int NO
        {
            get
            {
                return _NO;
            }
            set
            {
                _NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NO"));
            }
        }

        public string PRD_CODE { get; set; }
        public string PRD_NAME { get; set; }

        private decimal _normal_UPRC = 0;
        public decimal NORMAL_UPRC
        {
            get
            {
                return _normal_UPRC;
            }
            set
            {
                _normal_UPRC = value;
                //_DCM_SALE_AMT = _SALE_QTY * _normal_UPRC - _DC_AMT;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NORMAL_UPRC"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }

        private decimal _SALE_QTY = 0;
        public decimal SALE_QTY
        {
            get
            {
                return _SALE_QTY;
            }
            set
            {
                _SALE_QTY = value;
                //_DCM_SALE_AMT = _SALE_QTY * NORMAL_UPRC - _DC_AMT;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SALE_QTY"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }

        public decimal SALE_QTY_TMP { get; set; }

        public decimal DC_AMT
        {
            get
            {
                return DC_AMT_CPN + DC_AMT_CRD + DC_AMT_CST + DC_AMT_FOD + 
                        DC_AMT_GEN + DC_AMT_JCD + DC_AMT_PACK + DC_AMT_PRM + DC_AMT_SVC;
            }
        }

        public decimal DC_AMT_PERQTY
        {
            get
            {
                return TypeHelper.RoundNear(DC_AMT / SALE_QTY_TMP, 1);
            }
        }

        public decimal DC_VALUE { get; set; }

        /// <summary>
        /// // 할인액-일반
        /// </summary>
        public decimal DC_AMT_GEN
        {
            get => dC_AMT_GEN; set
            {
                dC_AMT_GEN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// // 할인액-서비스
        /// </summary>
        public decimal DC_AMT_SVC
        {
            get => dC_AMT_SVC; set
            {
                dC_AMT_SVC = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// // 할인액-제휴카드
        /// </summary>
        public decimal DC_AMT_JCD
        {
            get => dC_AMT_JCD; set
            {
                dC_AMT_JCD = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// // 할인액-쿠폰
        /// </summary>
        public decimal DC_AMT_CPN
        {
            get => dC_AMT_CPN; set
            {
                dC_AMT_CPN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        // 
        /// <summary>
        /// 할인액-회원 
        /// </summary>
        public decimal DC_AMT_CST
        {
            get => dC_AMT_CST; set
            {
                dC_AMT_CST = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// // 할인액-식권
        /// </summary>
        public decimal DC_AMT_FOD
        {
            get => dC_AMT_FOD; set
            {
                dC_AMT_FOD = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// // 할인액-프로모션
        /// </summary>
        public decimal DC_AMT_PRM
        {
            get => dC_AMT_PRM; set
            {
                dC_AMT_PRM = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        /// <summary>
        /// 할인액-신용카드-현장할인
        /// </summary>
        public decimal DC_AMT_CRD
        {
            get => dC_AMT_CRD; set
            {
                dC_AMT_CRD = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
            }
        }
        /// <summary>
        /// 할인액-포장
        /// </summary>
        public decimal DC_AMT_PACK
        {
            get => dC_AMT_PACK; set
            {
                dC_AMT_PACK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DC_AMT"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            }
        }
        public decimal SALE_AMT
        {
            get
            {
                return SALE_QTY * NORMAL_UPRC;
            }
        }

        // private decimal _DCM_SALE_AMT = 0;
        public decimal DCM_SALE_AMT
        {
            get
            {
                return decimal.Truncate(SALE_AMT - DC_AMT);
            }
            //set
            //{
            //    _DCM_SALE_AMT = value;
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DCM_SALE_AMT"));
            //}
        }

        private string _REMARK = string.Empty;
        public string REMARK
        {
            get
            {
                _REMARK = string.Empty;
                for (int i = 0; i < remarkItems.Keys.Count; i++)
                {
                    _REMARK += remarkItems[remarkItems.Keys.ElementAt(i)];
                    _REMARK += " ";
                }
                return _REMARK;
            }
        }

        public string MARK_ICON { get; set; }
        public string TAX_YN        { get; set; }
        public string PRD_DC_FLAG   { get; set; }
        public string CST_ACCDC_YN  { get; set; }
        public string STAMP_ACC_YN  { get; set; }
        public string STAMP_USE_YN  { get; set; }
        /// <summary>
        /// 싯가상품
        /// </summary>
        public string PRICE_MGR_FLAG { get; set; }        
        public int?   STAMP_ACC_QTY { get; set; }
        public int? STAMP_USE_QTY { get; set; }
        public string SIDE_MENU_YN  { get; set; }

        /// <summary>
        /// Default 0
        /// 1: 선택
        /// 2: 속성
        /// </summary>
        public string PRD_TYPE_FLAG { get; set; } = "0";

        /// <summary>
        /// Prente item
        /// </summary>
        public string UP_PRD_CODE { get; set; }

        public int MAX_QTY { get; set; }

        /// <summary>
        /// M: main, P: Property, S: Selection
        /// </summary>
        public string CLASS_TYPE { get; set; }

        public string CLASS_CODE { get; set; }

        /// <summary>
        /// (CCD_CODEM_T : 030) 0:일반 1:배달 2:포장
        /// 전체배달을 선택했을때 상품 전체 배달 구분 처리 함
        /// </summary>
        public string DLV_PACK_FLAG { get; set; } = "0";

        Dictionary<string, string> remarkItems = new Dictionary<string, string>();
        private decimal dC_AMT_CPN;
        private decimal dC_AMT_JCD;
        private decimal dC_AMT_PACK;
        private decimal dC_AMT_CRD;
        private decimal dC_AMT_PRM;
        private decimal dC_AMT_FOD;
        private decimal dC_AMT_CST;
        private decimal dC_AMT_SVC;
        private decimal dC_AMT_GEN;

        public void UpdateRemarkItem(string key, string itemRmk, bool add)
        {
            if (string.IsNullOrEmpty(itemRmk))
            {
                add = false;
            }

            if (add)
            {

                if (remarkItems.ContainsKey(key))
                {
                    remarkItems[key] = itemRmk;
                }
                else
                {
                    remarkItems.Add(key, itemRmk);
                }
            }
            else
            {
                remarkItems.Remove(key);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REMARK)));
        }

        public bool DiscSelPer { get; set; }
        public bool DiscSelAmt { get; set; }
        public bool DiscAllPer { get; set; }
        public bool DiscAllAmt { get; set; }

        /// <summary>
        /// Member exclusive discount
        /// </summary>
        public bool DiscMbr { get; set; }
        //public decimal DiscMbrAmt { get; set; }
        public decimal DiscMbrRate { get; set; }

        public ORDER_GRID_ITEM Copy(params bool[] removeDC)
        {
            var odi = new ORDER_GRID_ITEM()
            {
                CLASS_CODE = this.CLASS_CODE,
                CLASS_TYPE = this.CLASS_TYPE,
                //DCM_SALE_AMT = this.DCM_SALE_AMT,
                DLV_PACK_FLAG = this.DLV_PACK_FLAG,
                SALE_QTY = this.SALE_QTY,
                SALE_QTY_TMP = this.SALE_QTY_TMP,
                SIDE_MENU_YN = this.SIDE_MENU_YN,
                TAX_YN = this.TAX_YN,
                MARK_ICON = this.MARK_ICON,
                MAX_QTY = this.MAX_QTY,
                NODIS=this.NODIS,
                NO = this.NO,
                NORMAL_UPRC = this.NORMAL_UPRC,
                PRD_CODE = this.PRD_CODE,
                PRD_NAME = this.PRD_NAME,
                PRD_TYPE_FLAG = this.PRD_TYPE_FLAG,
                UP_PRD_CODE = this.UP_PRD_CODE,
                remarkItems = this.remarkItems,
                //DC_AMT = this.DC_AMT,
                DC_AMT_CPN = this.DC_AMT_CPN,
                DC_AMT_CRD =  this.DC_AMT_CRD,
                DC_AMT_CST = this.DC_AMT_CST,
                DC_AMT_FOD = this.DC_AMT_FOD,
                DC_AMT_GEN = this.DC_AMT_GEN,
                DC_AMT_JCD = this.DC_AMT_JCD,
                DC_AMT_PACK = this.DC_AMT_PACK,
                DC_AMT_PRM = this.DC_AMT_PRM,
                DC_AMT_SVC = this.DC_AMT_SVC,
                DC_VALUE = this.DC_VALUE,
                STAMP_ACC_QTY = this.STAMP_ACC_QTY,
                STAMP_ACC_YN = this.STAMP_ACC_YN,
                CST_ACCDC_YN = this.CST_ACCDC_YN,
                STAMP_USE_QTY = this.STAMP_USE_QTY,
                STAMP_USE_YN = this.STAMP_USE_YN
            };

            if (removeDC != null && removeDC.Length > 0)
            {
                odi.remarkItems.Remove("DC");
                odi.remarkItems.Remove("DCP");
            }

            return odi;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static ORDER_GRID_ITEM FromTRNProduct(TRN_PRDT p, int itemCount)
        {
            var oi = new ORDER_GRID_ITEM()
            {
                MARK_ICON = "0".Equals(p.PRD_TYPE_FLAG) ? "" : "u",
                PRD_CODE = p.PRD_CODE,
                PRD_NAME = p.PRD_NAME,
                PRD_TYPE_FLAG = p.PRD_TYPE_FLAG,
                SALE_QTY = p.SALE_QTY,
                NORMAL_UPRC = p.SALE_UPRC,
                //SALE_AMT = p.SALE_AMT,
                //DC_AMT = p.DC_AMT,
                //DCM_SALE_AMT = p.DCM_SALE_AMT,
                DLV_PACK_FLAG = p.DLV_PACK_FLAG,
                CLASS_CODE = p.SDS_CLASS_CODE,
                NO = itemCount + 1,
                SIDE_MENU_YN = p.SIDE_MENU_YN,
                CLASS_TYPE = p.PRD_TYPE_FLAG == "0" ? "M" : (p.PRD_TYPE_FLAG == "2" ? "P" : "S"),
                MAX_QTY = 0,
                TAX_YN = p.TAX_YN,
                //CST_ACCDC_YN = p.CST_ACCDC_YN
            };

            if (p.DC_AMT > 0)
            {
                oi.UpdateRemarkItem("DC", p.REMARK, true);
            }

            if (!"0".Equals(p.PRD_TYPE_FLAG))
            {
                oi.UpdateRemarkItem("CLASS", "2".Equals(oi.PRD_TYPE_FLAG) ? "속성" : "선택", true);
            }
            return oi;
        }

        public bool HasRemarkItem(string rmkKey)
        {
            return remarkItems.ContainsKey(rmkKey);
        }

        public override bool Equals(object? obj)
        {
            return obj is ORDER_GRID_ITEM iTEM &&
                   DC_AMT_GEN == iTEM.DC_AMT_GEN;
        }
    }

    public class ORDER_SUM_INFO : INotifyPropertyChanged
    {
        private decimal gST_PAY_AMT = 0;
        private decimal tOT_DC_AMT = 0;
        private decimal tOT_SALE_AMT = 0;
        private decimal tOT_QTY = 0;
        private decimal tips = 0;
        private decimal eTC_AMT;
        private decimal rEPAY_CASH_AMT;

        /// <summary>
        /// 총금액
        /// </summary>
        public decimal TOT_SALE_AMT
        {
            get => tOT_SALE_AMT; set
            {
                tOT_SALE_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DCM_SALE_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOT_SALE_AMT)));
            }
        }

        /// <summary>
        /// 실매출액
        /// </summary>
        public decimal DCM_SALE_AMT
        {
            get
            {
                return TOT_SALE_AMT - TOT_DC_AMT;
            }
        }

        /// <summary>
        /// 할인금액
        /// </summary>
        public decimal TOT_DC_AMT
        {
            get => tOT_DC_AMT; set
            {
                tOT_DC_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOT_DC_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EXP_PAY_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DCM_SALE_AMT)));
            }
        }

        public decimal TOT_QTY
        {
            get => tOT_QTY; set
            {
                tOT_QTY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOT_QTY)));
            }
        }

        /// <summary>
        /// 받을금액
        /// Remain
        /// </summary>
        public decimal EXP_PAY_AMT
        {
            get
            {
                return DCM_SALE_AMT - GST_PAY_AMT;
            }
        }
        /// <summary>
        /// 받은금액
        /// Payment된금액
        /// </summary>
        public decimal GST_PAY_AMT
        {
            get => gST_PAY_AMT; set
            {
                gST_PAY_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GST_PAY_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EXP_PAY_AMT)));
            }
        }
        /// <summary>
        /// 거스름돈
        /// </summary>
        public decimal RET_PAY_AMT
        {
            get
            {
                var retPayAmt = (((DCM_SALE_AMT > GST_PAY_AMT) || GST_PAY_AMT <= 0) ? 0 : GST_PAY_AMT - DCM_SALE_AMT - ETC_AMT) + REPAY_CASH_AMT;
                return retPayAmt > 0 ? retPayAmt : 0;
            }
        }

        public decimal REPAY_CASH_AMT
        {
            get => rEPAY_CASH_AMT; set
            {
                rEPAY_CASH_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RET_PAY_AMT)));
            }
        }

        /// <summary>
        /// 짜투리
        /// </summary>
        public decimal ETC_AMT
        {
            get => eTC_AMT; set
            {
                eTC_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RET_PAY_AMT)));
            }
        }


        // tiền bo (tips)
        public decimal TIPS
        {
            get => tips; set
            {
                tips = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TIPS)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    #endregion

    #region 대기 - 보류

    public class ORD_WAIT_ITEM
    {
        public string TABLE_NO { get; set; } = string.Empty;
        public int SEQ { get; set; }
        public string WAIT_NO { get; set; }
        public string INSERT_DT { get; set; }
        public string INSERT_DT_PRO
        {
            get
            {
                var x = (!string.IsNullOrEmpty(INSERT_DT) ? DateTime.ParseExact(INSERT_DT, "yyyyMMddHHmmss", CultureInfo.InvariantCulture) : DateTime.MinValue);
                string y = (x != null ? x.ToString("MM'월'dd'일'','HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("ko-KR")) : string.Empty);
                return y;
            }
        }

        /// <summary>
        /// 주문금액
        /// </summary>
        public decimal EXP_PAY_AMT { get; set; }

        /// <summary>
        /// 받을금액
        /// </summary>
        public decimal GST_PAY_AMT { get; set; }
    }

    #endregion



}
