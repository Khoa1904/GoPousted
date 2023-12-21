using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SalesMiddleExcClcModel : INotifyPropertyChanged
    {
        private decimal rEM_CHECK_AMT;
        private int rEM_W100000_CNT;
        private int rEM_W50000_CNT;
        private int rEM_W10000_CNT;
        private int rEM_W5000_CNT;
        private int rEM_W1000_CNT;
        private int rEM_W500_CNT;
        private int rEM_W100_CNT;
        private int rEM_W50_CNT;
        private int rEM_W10_CNT;
        private decimal rEM_TK_GFT_AMT;

        public decimal REM_CHECK_AMT
        {
            get => rEM_CHECK_AMT; set
            {
                rEM_CHECK_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CHECK_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public int REM_W100000_CNT
        {
            get => rEM_W100000_CNT; set
            {
                rEM_W100000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W100000_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W100000_CNT
        {
            get
            {
                return REM_W100000_CNT * 100000;
            }
        }
        public int REM_W50000_CNT
        {
            get => rEM_W50000_CNT; set
            {
                rEM_W50000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W50000_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W50000_CNT
        {
            get
            {
                return REM_W50000_CNT * 50000;
            }
        }
        public int REM_W10000_CNT
        {
            get => rEM_W10000_CNT; set
            {
                rEM_W10000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W10000_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W10000_CNT
        {
            get
            {
                return REM_W10000_CNT * 10000;
            }
        }

        public int REM_W5000_CNT
        {
            get => rEM_W5000_CNT; set
            {
                rEM_W5000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W5000_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W5000_CNT
        {
            get
            {
                return REM_W5000_CNT * 5000;
            }
        }

        public int REM_W1000_CNT
        {
            get => rEM_W1000_CNT; set
            {
                rEM_W1000_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W1000_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W1000_CNT
        {
            get
            {
                return REM_W1000_CNT * 1000;
            }
        }
        public int REM_W500_CNT
        {
            get => rEM_W500_CNT; set
            {
                rEM_W500_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W500_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W500_CNT
        {
            get
            {
                return REM_W500_CNT * 500;
            }
        }
        public int REM_W100_CNT
        {
            get => rEM_W100_CNT; set
            {
                rEM_W100_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W100_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W100_CNT
        {
            get
            {
                return REM_W100_CNT * 100;
            }
        }
        public int REM_W50_CNT
        {
            get => rEM_W50_CNT; set
            {
                rEM_W50_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W50_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W50_CNT
        {
            get
            {
                return REM_W50_CNT * 50;
            }
        }
        public int REM_W10_CNT
        {
            get => rEM_W10_CNT; set
            {
                rEM_W10_CNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_REM_W10_CNT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REM_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        public decimal TOTAL_REM_W10_CNT
        {
            get
            {
                return REM_W10_CNT * 10;
            }
        }
        private decimal _pos_ready_amt { get; set; }
        public decimal POS_READY_AMT
        {
            get => _pos_ready_amt; set
            {
                _pos_ready_amt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_READY_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _cast_amt { get; set; }
        public decimal CASH_AMT
        {
            get => _cast_amt; set
            {
                _cast_amt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        private decimal wea_cash { get; set; }
        public decimal WEA_IN_CSH_AMT
        {
            get => wea_cash; set
            {
                wea_cash = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WEA_IN_CSH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _tk_giftcashamt { get; set; }
        public decimal TK_GFT_CSH_AMT
        {
            get => _tk_giftcashamt; set
            {
                _tk_giftcashamt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TK_GFT_CSH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TK_GFT_SALE_CSH_AMT { get; set; }

        private decimal _tkfodsalecashAmt { get; set; }
        public decimal TK_FOD_SALE_CSH_AMT
        {
            get => _tkfodsalecashAmt; set
            {
                _tk_giftcashamt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TK_FOD_SALE_CSH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _posCashinAmt { get; set; }
        public decimal POS_CSH_IN_AMT
        {
            get => _posCashinAmt; set
            {
                _posCashinAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_CSH_IN_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _poscashoutAmt { get; set; }
        public decimal POS_CSH_OUT_AMT
        {
            get => _poscashoutAmt; set
            {
                _poscashoutAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POS_CSH_OUT_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _repaycashAmt { get; set; }
        public decimal REPAY_CASH_AMT
        {
            get => _repaycashAmt; set
            {
                _repaycashAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(REPAY_CASH_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal _crcardAmt { get; set; }
        public decimal CRD_CARD_AMT
        {
            get => _crcardAmt; set
            {
                _crcardAmt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CRD_CARD_AMT)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }
        private decimal weaCredit { get; set; }
        public decimal WEA_IN_CRD_AMT
        {
            get => weaCredit; set
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
                    TOTAL_REM_W100_CNT + TOTAL_REM_W10_CNT + TOTAL_REM_W50000_CNT + TOTAL_REM_W5000_CNT + TOTAL_REM_W500_CNT + TOTAL_REM_W50_CNT;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_CASH_AMT)));
            }
        }

        public decimal CARD_S_CNT { get; set; }
        public decimal CARD_S_AMT { get; set; }
        public decimal CARD_C_CNT { get; set; }
        public decimal CARD_C_AMT { get; set; }

        public decimal TK_GFT_CNT { get; set; }
        public decimal TK_GFT_UAMT { get; set; }

        public int REPAY_TK_GFT_CNT { get; set; }
        public decimal REPAY_TK_GFT_AMT { get; set; }

        public int REM_TK_GFT_CNT { get; set; }
        public decimal REM_TK_GFT_AMT
        {
            get => rEM_TK_GFT_AMT; set
            {
                rEM_TK_GFT_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LOSS_TK_GFT_AMT)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public decimal LOSS_CASH_AMT
        {
            get
            {
                return REM_CHECK_AMT + TOTAL_REM_W100000_CNT + TOTAL_REM_W10000_CNT + TOTAL_REM_W1000_CNT + TOTAL_REM_W100_CNT
                     + TOTAL_REM_W10_CNT + TOTAL_REM_W50000_CNT + TOTAL_REM_W5000_CNT + TOTAL_REM_W500_CNT + TOTAL_REM_W50_CNT
             - POS_READY_AMT - CASH_AMT - WEA_IN_CSH_AMT - TK_GFT_CSH_AMT - TK_FOD_SALE_CSH_AMT - POS_CSH_IN_AMT - POS_CSH_OUT_AMT - REPAY_CASH_AMT;
            }
        }
        public decimal LOSS_TK_GFT_AMT
        {
            get
            {
                return INPUT_TK_GFT_AMT - REM_TK_GFT_AMT + REPAY_TK_GFT_AMT;
            }
        }

        //private decimal _RET_BILL_AMT   { get; set; }   = 0 ;
        //private decimal _TOT_SALE_AMT { get; set; } = 0;
        public decimal JOKE_SALE_AMT
        {
            get
            {
                return TOT_SALE_AMT + RET_BILL_AMT;
            }
        }
        public decimal RET_BILL_AMT { get; set; }
        //{
        //    get => _RET_BILL_AMT; set
        //    {
        //        _RET_BILL_AMT = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RET_BILL_AMT)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JOKE_SALE_AMT)));
        //    }
        //}
        private decimal inputTKgftAMT { get; set; }
        public decimal INPUT_TK_GFT_AMT
        {
            get => inputTKgftAMT; set
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
        public decimal TK_GFT_AMT {get;set;} = 0;
        public decimal TK_FOD_AMT { get; set; } = 0;
    }
}
