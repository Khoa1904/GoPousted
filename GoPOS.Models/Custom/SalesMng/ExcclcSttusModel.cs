using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.SalesMng
{
    public class
        ExcclcSttusModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private decimal rEM_CHECK_AMT;
        private decimal rEM_W100000_CNT;
        private decimal rEM_W50000_CNT;
        private decimal rEM_W10000_CNT;
        private decimal rEM_W5000_CNT;
        private decimal rEM_W1000_CNT;
        private decimal rEM_W500_CNT;
        private decimal rEM_W100_CNT;
        private decimal rEM_W50_CNT;
        private decimal rEM_W10_CNT;
        private decimal rEM_TK_GFT_AMT;
        private int cARD_C_CNT;
        private int cARD_C_AMT;
        private int cARD_S_CNT;
        private int cARD_S_AMT;
        private int total;

        public int Total
        {
            get => CARD_S_AMT - CARD_C_AMT;
          
        }

        public decimal Total2
        {
            get => Math.Abs(REM_TK_GFT_AMT - TK_GFT_UAMT + REPAY_TK_GFT_AMT);
        }

        public string SALE_DATE { get; set; }

        public int CARD_C_CNT
        {
            get => cARD_C_CNT;
            set
            {
                cARD_C_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CARD_C_CNT)));
            }
        }

        public int CARD_C_AMT
        {
            get => cARD_C_AMT;
            set
            {
                cARD_C_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CARD_C_AMT)));
            }
        }

        public int CARD_S_CNT
        {
            get => cARD_S_CNT;
            set
            {
                cARD_S_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CARD_S_CNT)));
            }
        }

        public int CARD_S_AMT
        {
            get => cARD_S_AMT;
            set
            {
                cARD_S_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CARD_S_AMT)));
            }
        }

        public decimal REM_CHECK_AMT
        {
            get => rEM_CHECK_AMT;
            set
            {
                rEM_CHECK_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CHECK_AMT)));
            }
        }

        public decimal REM_W100000_CNT
        {
            get => rEM_W100000_CNT;
            set
            {
                rEM_W100000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W100000_CNT)));
            }
        }

        public decimal TOTAL_REM_W100000_CNT
        {
            get { return REM_W100000_CNT * 100000; }
        }

        public decimal REM_W50000_CNT
        {
            get => rEM_W50000_CNT;
            set
            {
                rEM_W50000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W50000_CNT)));
            }
        }

        public decimal TOTAL_REM_W50000_CNT
        {
            get { return REM_W50000_CNT * 50000; }
        }

        public decimal REM_W10000_CNT
        {
            get => rEM_W10000_CNT;
            set
            {
                rEM_W10000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W10000_CNT)));
            }
        }

        public decimal TOTAL_REM_W10000_CNT
        {
            get { return REM_W10000_CNT * 10000; }
        }

        public decimal REM_W5000_CNT
        {
            get => rEM_W5000_CNT;
            set
            {
                rEM_W5000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W5000_CNT)));
            }
        }

        public decimal TOTAL_REM_W5000_CNT
        {
            get { return REM_W5000_CNT * 5000; }
        }

        public decimal REM_W1000_CNT
        {
            get => rEM_W1000_CNT;
            set
            {
                rEM_W1000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W1000_CNT)));
            }
        }

        public decimal TOTAL_REM_W1000_CNT
        {
            get { return REM_W1000_CNT * 1000; }
        }

        public decimal REM_W500_CNT
        {
            get => rEM_W500_CNT;
            set
            {
                rEM_W500_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W500_CNT)));
            }
        }

        public decimal TOTAL_REM_W500_CNT
        {
            get { return REM_W500_CNT * 500; }
        }

        public decimal REM_W100_CNT
        {
            get => rEM_W100_CNT;
            set
            {
                rEM_W100_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W100_CNT)));
            }
        }

        public decimal TOTAL_REM_W100_CNT
        {
            get { return REM_W100_CNT * 100; }
        }

        public decimal REM_W50_CNT
        {
            get => rEM_W50_CNT;
            set
            {
                rEM_W50_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W50_CNT)));
            }
        }

        public decimal TOTAL_REM_W50_CNT
        {
            get { return REM_W50_CNT * 50; }
        }

        public decimal REM_W10_CNT
        {
            get => rEM_W10_CNT;
            set
            {
                rEM_W10_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_W10_CNT)));
            }
        }

        public decimal TOTAL_REM_W10_CNT
        {
            get { return REM_W10_CNT * 10; }
        }

        private decimal _pos_ready_amt { get; set; }

        public decimal POS_READY_AMT
        {
            get => _pos_ready_amt;
            set
            {
                _pos_ready_amt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_READY_AMT)));
            }
        }

        private decimal _cast_amt { get; set; }

        public decimal CASH_AMT
        {
            get => _cast_amt;
            set
            {
                _cast_amt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        private decimal wea_cash { get; set; }

        public decimal WEA_IN_CSH_AMT
        {
            get => wea_cash;
            set
            {
                wea_cash = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WEA_IN_CSH_AMT)));
            }
        }

        private decimal _tk_giftcashamt { get; set; }

        public decimal TK_GFT_CSH_AMT
        {
            get => _tk_giftcashamt;
            set
            {
                _tk_giftcashamt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TK_GFT_CSH_AMT)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal TK_GFT_SALE_CSH_AMT { get; set; }

        private decimal _tkfodsalecashAmt { get; set; }

        public decimal TK_FOD_SALE_CSH_AMT
        {
            get => _tkfodsalecashAmt;
            set
            {
                _tk_giftcashamt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TK_FOD_SALE_CSH_AMT)));
            }
        }

        private decimal _posCashinAmt { get; set; }

        public decimal POS_CSH_IN_AMT
        {
            get => _posCashinAmt;
            set
            {
                _posCashinAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_CSH_IN_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        private decimal _poscashoutAmt { get; set; }

        public decimal POS_CSH_OUT_AMT
        {
            get => _poscashoutAmt;
            set
            {
                _poscashoutAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_CSH_OUT_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        private decimal _repaycashAmt { get; set; }

        public decimal REPAY_CASH_AMT
        {
            get => _repaycashAmt;
            set
            {
                _repaycashAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REPAY_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        private decimal _crcardAmt { get; set; }

        public decimal CRD_CARD_AMT
        {
            get => _crcardAmt;
            set
            {
                _crcardAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CRD_CARD_AMT)));
            }
        }

        private decimal _rET_BILL_CNT { get; set; }

        public decimal RET_BILL_CNT
        {
            get => _rET_BILL_CNT;
            set
            {
                _rET_BILL_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RET_BILL_CNT)));
            }
        }


        private decimal _cRD_CARD_CNT { get; set; }

        public decimal CRD_CARD_CNT
        {
            get => _cRD_CARD_CNT;
            set
            {
                _cRD_CARD_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CRD_CARD_CNT)));
            }
        }


        private decimal weaCredit { get; set; }

        public decimal WEA_IN_CRD_AMT
        {
            get => weaCredit;
            set
            {
                weaCredit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WEA_IN_CRD_AMT)));
            }
        }

        public decimal REM_CASH_AMT
        {
            get
            {
                return REM_CHECK_AMT + TOTAL_REM_W100000_CNT + TOTAL_REM_W10000_CNT + TOTAL_REM_W1000_CNT +
                       TOTAL_REM_W100_CNT + TOTAL_REM_W10_CNT + TOTAL_REM_W50000_CNT + TOTAL_REM_W5000_CNT +
                       TOTAL_REM_W500_CNT + TOTAL_REM_W50_CNT;
            }
            set { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT))); }
        }

        public decimal TK_GFT_CNT { get; set; }
        public decimal TK_GFT_UAMT { get; set; }

        public decimal REPAY_TK_GFT_CNT { get; set; }
        public decimal REPAY_TK_GFT_AMT { get; set; }

        public decimal REM_TK_GFT_CNT { get; set; }

        public decimal REM_TK_GFT_AMT
        {
            get => rEM_TK_GFT_AMT;
            set
            {
                rEM_TK_GFT_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_TK_GFT_AMT)));
            }
        }

        public decimal _lOSS_CASH_AMT;

        public decimal LOSS_CASH_AMT
        {
            get => _lOSS_CASH_AMT;
            set
            {
                _lOSS_CASH_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        public decimal LOSS_TK_GFT_AMT
        {
            get { return INPUT_TK_GFT_AMT - REM_TK_GFT_AMT + REPAY_TK_GFT_AMT; }
        }

        //private decimal _RET_BILL_AMT   { get; set; }   = 0 ;
        //private decimal _TOT_SALE_AMT { get; set; } = 0;
        public decimal JOKE_SALE_AMT
        {
            get { return TOT_SALE_AMT + RET_BILL_AMT; }
        }

        public decimal _rET_BILL_AMT;

        public decimal RET_BILL_AMT
        {
            get => _rET_BILL_AMT;
            set
            {
                rEM_TK_GFT_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RET_BILL_AMT)));
            }
        }

        private decimal inputTKgftAMT { get; set; }

        public decimal INPUT_TK_GFT_AMT
        {
            get => inputTKgftAMT;
            set
            {
                inputTKgftAMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(INPUT_TK_GFT_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_TK_GFT_AMT)));
            }
        }

        public decimal TOT_SALE_AMT { get; set; }
        public decimal TOT_DC_AMT { get; set; } = 0;
        public decimal DCM_SALE_AMT { get; set; } = 0;
        public decimal VAT_SALE_AMT { get; set; } = 0;
        public decimal VAT_AMT { get; set; } = 0;
        public decimal NO_VAT_SALE_AMT { get; set; } = 0;
        public decimal NO_TAX_SALE_AMT { get; set; } = 0;
        public decimal CST_POINTSAVE_AMT { get; set; } = 0;
        public decimal CST_POINTUSE_AMT { get; set; } = 0;
        public decimal TK_GFT_AMT { get; set; } = 0;
        public decimal TK_FOD_AMT { get; set; } = 0;
    }
}