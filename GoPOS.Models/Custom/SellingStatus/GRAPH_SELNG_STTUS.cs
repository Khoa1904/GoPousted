using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.SellingStatus
{
    public class GRAPH_SELNG_STTUS : INotifyPropertyChanged
    {
        private decimal cASH_AMT;
        private decimal cRD_CARD_AMT;
        private decimal jCD_CARD_AMT;
        private decimal wES_AMT;
        private decimal tK_GFT_AMT;
        private decimal cST_POINTUSE_AMT;
        private decimal eTC_AMT;
        private decimal tOT_SALE_AMT;
        private decimal tOT_DC_AMT;// 할인액추가

        public decimal TOT_DC_AMT
        {
            get => tOT_DC_AMT; set
            {
                tOT_DC_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOT_DC_AMT)));

            }
        }

        public decimal CASH_AMT
        {
            get => cASH_AMT; set
            {
                cASH_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CASH_AMT)));
            }
        }
        public decimal CRD_CARD_AMT
        {
            get => cRD_CARD_AMT; set
            {
                cRD_CARD_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CRD_CARD_AMT)));
        
            }
        }

        public decimal JCD_CARD_AMT
        {
            get => jCD_CARD_AMT; set
            {
                jCD_CARD_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JCD_CARD_AMT)));
 
            }
        }

        public decimal WES_AMT
        {
            get => wES_AMT; set
            {
                wES_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WES_AMT)));
         
            }
        }

        public decimal TK_GFT_AMT
        {
            get => tK_GFT_AMT; set
            {
                tK_GFT_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TK_GFT_AMT)));
         
            }
        }

        public decimal CST_POINTUSE_AMT
        {
            get => cST_POINTUSE_AMT; set
            {
                cST_POINTUSE_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CST_POINTUSE_AMT)));
        
            }
        }

        public decimal ETC_AMT
        {
            get => eTC_AMT; set
            {
                eTC_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ETC_AMT)));

            }
        }

        public decimal TOT_SALE_AMT
        {
            get => tOT_SALE_AMT; set
            {
                tOT_SALE_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOT_SALE_AMT)));

            }
        }
      
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
