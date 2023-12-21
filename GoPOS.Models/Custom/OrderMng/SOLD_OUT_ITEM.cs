using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.OrderMng
{
    public class SOLD_OUT_ITEM : INotifyPropertyChanged
    {
        private string sTOCK_OUT_YN;
        private string tAX_YN;

        public string SHOP_CODE { get; set; }
        public string PRD_CODE { get; set; }
        public string PRD_NAME { get; set; }
        public decimal SALE_UPRC { get; set; }
        public string STOCK_OUT_YN
        {
            get => sTOCK_OUT_YN; set
            {
                sTOCK_OUT_YN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("STOCK_OUT_YN"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SO_NAME"));
            }
        }
        public string SO_NAME
        {
            get
            {
                return "N".Equals(STOCK_OUT_YN) ? "판매" : "품절";
            }
        }
        public string TAX_YN
        {
            get => tAX_YN; set
            {
                tAX_YN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TAX_YN"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TY_NAME"));
            }
        }

        public string TY_NAME
        {
            get
            {
                return "Y".Equals(TAX_YN) ? "과세" : "면세";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; 
    }
}
