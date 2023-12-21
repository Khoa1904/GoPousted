using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.API
{
    public class TranData
    {
        public TranData() { }

        public string SHOP_CODE { get; set; }
        public string SALE_DATE { get; set; }
        public string POS_NO { get; set; }
        public string BILL_NO { get; set; }

        [JsonPropertyName("tranHeader")]
        public TRN_HEADER TranHeader { get; set; }

        [JsonPropertyName("tranProduct")]
        public TRN_PRDT[] TranProduct { get; set; } = new TRN_PRDT[0];

        [JsonPropertyName("tranTenderSeq")]
        public TRN_TENDERSEQ[] TranTenderSeq { get; set; } = new TRN_TENDERSEQ[0];

        [JsonPropertyName("tranCash")]
        public TRN_CASH[] TranCash { get; set; } = new TRN_CASH[0];

        [JsonPropertyName("tranCashRec")]
        public TRN_CASHREC[] TranCashRec { get; set; } = new TRN_CASHREC[0];

        [JsonPropertyName("tranCard")]
        public TRN_CARD[] TranCard { get; set; } = new TRN_CARD[0];

        [JsonPropertyName("tranPartnerCard")]
        public TRN_PARTCARD[] TranPartnerCard { get; set; } = new TRN_PARTCARD[0];

        [JsonPropertyName("tranGift")]
        public TRN_GIFT[] TranGift { get; set; } = new TRN_GIFT[0];

        [JsonPropertyName("tranFoodCoupon")]
        public TRN_FOODCPN[] TranFoodCpn { get; set; } = new TRN_FOODCPN[0];

        [JsonPropertyName("tranEasyPay")]
        public TRN_EASYPAY[] TranEasyPay { get; set; } = new TRN_EASYPAY[0];

        [JsonPropertyName("tranCredit")]
        public TRN_CREDIT[] TranCredit { get; set; } = new TRN_CREDIT[0];

        [JsonPropertyName("tranPointUse")]
        public TRN_POINTUSE[] TranPointuse { get; set;}

        [JsonPropertyName("tranPointSave")]
        public TRN_POINTSAVE TranPointSave { get; set; }
        [JsonPropertyName("tranPpCard")]
        public TRN_PPCARD[] TranPpCard { get; set; }

        [JsonPropertyName("NTranPreChargeHeader")]
        public NTRN_PRECHARGE_HEADER NTranPreChargeHeader { get; set; }

        [JsonPropertyName("NTranPreChargeCard")]
        public NTRN_PRECHARGE_CARD[] NTranPreChargeCard { get; set; }
    }
    
    public class TranAccount
    {
        [JsonPropertyName("NTranAccount")]
        public SETT_POSACCOUNT NTranAccount { get; set; }

    }
}
