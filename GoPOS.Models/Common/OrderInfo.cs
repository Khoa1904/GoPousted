using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Common
{
    public enum EPrintType { IP, IPDefault, COM }
    public class OrderInfo
    {
        public string ShopCode { get; set; } = DataLocals.ShopInfo?.SHOP_CODE??string.Empty;
        public string PosNo { get; set; } = DataLocals.PosStatus?.POS_NO ?? string.Empty;
        public string SaleDate { get; set; } = DataLocals.PosStatus?.SALE_DATE ?? string.Empty;
        public string BillNo { get; set; } = DataLocals.PosStatus?.BILL_NO ?? string.Empty;
        public PrinterInfo PrintInfo { get; set; }
    }
    public class PrinterInfo
    {
        public EPrintType Type { get; set;}
        public string IP { get; set;}
        public int Port { get; set; }
        public string SerialPort { get; set; }
        public int Speed { get; set; } = 9600;
    }
}
