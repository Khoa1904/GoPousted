using GoPOS.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class MEMBER_CLASH : INotifyPropertyChanged
    {
        public string posNo { get; set; } = string.Empty;
        private string _createdAt = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string salesDt { get; set; } = string.Empty;
        public string createdAt
        {
            get
            {
                if (_createdAt.Length >= 8)
                {
                    return DateTime.ParseExact(_createdAt.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    .ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
            set
            {
                _createdAt = value;
            }
        }
        public string mbrCelno { get; set; } = string.Empty;
        public string mbrCardno { get; set; } = string.Empty;
        public string mbrNm { get; set; } = string.Empty;
        public string mbrCode { get; set; } = string.Empty;
        public string mbrGrdCode { get; set; } = string.Empty;
        public string mbrGrdNm { get; set; } = string.Empty;
        public string joinStoreCode { get; set; } = string.Empty;
        public string mbrTelno { get; set; } = string.Empty;
        public string mbrAdres { get; set; } = string.Empty;
        public string mbrMttlDe { get; set; } = string.Empty;
        public string mbrRemark { get; set; } = string.Empty;
        public decimal totalSaleCount { get; set; } = 0;
        public decimal totalSaleAmt { get; set; } = 0;
        public decimal totalPoint { get; set; } = 0;
        public decimal totalUsePoint { get; set; } = 0;
        public decimal avalidPoint { get; set; } = 0;
        public decimal creditAmt { get; set; } = 0;
        public decimal ppcAmt { get; set; } = 0;
        public decimal receivable { get; set; } = 0;
        public decimal prevAvalidPoint { get; set; } = 0;
        public decimal savePoint { get; set; } = 0;
        public decimal balance { get; set; } = 0;
        public decimal dscRt { get; set; } = 0;
        public decimal dscLmtWage { get; set; } = 0;
        public decimal minUsePt { get; set; } = 0;
        public decimal maxUsePt { get; set; } = 0;
        public decimal avalidStampAmt { get; set; } = 0;
        /// <summary>
        /// Return data
        /// </summary>
        public decimal useStampCnt { get; set; } = 0;
    }
    public class MEMBERPOINT_HISTORY
    {
        public string storeNm { get; set; } = string.Empty;
        public string occurDate { get; set; } = string.Empty;
        public string pointAt { get; set; } = string.Empty;
        public decimal salePrice { get; set; } = 0;
        public string flagName
        {
            get
            {
                switch (pointAt)
                {
                    case "1":
                        return "적립";
                    case "2":
                        return "사용";
                    case "3":
                        return "조정";
                    case "4":
                        return "외상입금";
                    default:
                        return "";
                }
            }
        }
        public string showDate
        {
            get
            {
                return DateTime.ParseExact(occurDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            }
        }
    }
    public class MEMBER_GRADE
    {
        public string grdSeq { get; set; } = string.Empty;
        public string grdCode { get; set; } = string.Empty;
        public string grdNm { get; set; } = string.Empty;
    }
    public class PREPAYMENT_FLOWS
    {
        public string storeCode { get; set; } = string.Empty;
        public string salesDt { get; set; } = string.Empty;
        public string posNo { get; set; } = string.Empty;
        public string billNo { get; set; } = string.Empty;
        public string saleSeCode { get; set; } = string.Empty;
        public string mbrCode { get; set; } = string.Empty;
        public decimal payAmt { get; set; } = 0;
        public decimal crntAvlblAmt { get; set; } = 0;
        public string ppcChgSeCode { get; set; } = string.Empty;
        public string createdAt { get; set; } = string.Empty;
        public string SellDay
        {
            get
            {
                var x = (!string.IsNullOrEmpty(salesDt) ? DateTime.ParseExact(salesDt, "yyyyMMdd", CultureInfo.InvariantCulture) : DateTime.MinValue);
                string y = (x != null ? x.ToString("yyyy'년'MM'월'dd'일'", System.Globalization.CultureInfo.GetCultureInfo("ko-KR")) : string.Empty);
                return y;
            }
        }
        public string OccurDate
        {
            get
            {
                var x = (!string.IsNullOrEmpty(createdAt) ? DateTime.ParseExact(createdAt, "yyyyMMddHHmmss", CultureInfo.InvariantCulture) : DateTime.MinValue);
                string y = (x!= null ? x.ToString("yyyy'년'MM'월'dd'일'", System.Globalization.CultureInfo.GetCultureInfo("ko-KR")) : string.Empty);
                return y;
            }          
        }
        public string PayType
        {
           get
            {
                switch(ppcChgSeCode)
                {
                    case "UA":
                        return "사용";
                    case "UC":
                        return "취소";
                    case "AA":
                        return "충전";
                    case "AC":
                        return "취소";
                    default:
                        return "";
                }
            }
        }
        public string payAmtcurrency
        {
            get
            {
                return decimal.Truncate(payAmt).ToString("N0") + " 원";
            }
        }
        public string balCurrency
        {
            get
            {
                return decimal.Truncate(crntAvlblAmt).ToString("N0") + " 원";
            }
        }
    }

    public class PREPAYMENT_USEINFO
    {
        public string mbrCode       { get; set; } = string.Empty;
        public decimal useAmt       { get; set; } = 0;
        public decimal chargeRemAmt { get; set; } = 0;
        public string createdAt     { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        public string storeCode   {get;set;} = string.Empty;
        public string salesDt     {get;set;} = string.Empty;
        public string posNo       {get;set;} = string.Empty;
        public string billNo      {get;set;} = string.Empty;
        public string saleSeCode  {get;set;} = string.Empty;
        public string orgApprInfo {get;set;} = string.Empty;
        public string apprNo      { get; set; } = string.Empty;
        public string orgApprNo   { get; set; } = string.Empty;

    }                      
}