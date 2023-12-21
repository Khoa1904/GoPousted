using GoPOS.Models.Custom.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Common
{
    public class KDSDataSendInfo
    {
        public MST_INFO_KDS? DeviceInfo { get; set; }
        public List<TRN_PRDT>? Products { get; set; }
        public void AddProducts(List<TRN_PRDT> pros, IList<ProductInfo>? _productInfo)
        {
            if (pros == null) return;
            if(Products == null) Products = new List<TRN_PRDT>();
            foreach(TRN_PRDT p in pros)
            {
                var check = Products.Any(t => t.SHOP_CODE == p.SHOP_CODE && t.SALE_DATE == p.SALE_DATE && t.POS_NO == p.POS_NO && t.BILL_NO == p.BILL_NO && t.SEQ_NO == p.SEQ_NO);
                if (!check)
                {
                    if(_productInfo != null)
                    {
                        var pro = _productInfo.FirstOrDefault(t=> t.SHOP_CODE == p.SHOP_CODE && t.PRD_CODE == p.PRD_CODE);
                        if(pro != null)
                        {
                            p.PRD_NAME = pro.PRD_NAME;
                        }
                    }
                    Products.Add(p);
                }
            }
        }
    }
}
