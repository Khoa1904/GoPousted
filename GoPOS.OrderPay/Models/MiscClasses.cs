using Flurl.Util;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.Payment;
using GoPOS.ViewModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows.Markup.Localizer;
using System.Windows.Shapes;

namespace GoPOS.OrderPay.Models
{
    public class SideMenuConfirmEventArgs
    {
        public bool Confirmed { get; set; }
    }
    public class OrderPayDiscHandleEventArgs
    {
        /// <summary>
        /// 1; show, 0: close, 2: apply
        /// </summary>
        public OrderPayDiscHandleActions Action { get; set; }
        public bool IsAll { get; set; }
        public bool IsAmt { get; set; }
        public bool IsApply { get; set; }
    }

    public class KeypadFocusInsEvent
    {
        public bool isFocus { get; set; }
    }

    public enum OrderPayDiscHandleActions
    {
        CloseLeft,
        CloseRight,
        Show,
        Apply
    }

    public enum PriceMgrProdAddingStatus
    {
        NoneAdd,
        AddingTuch,
        AddingSearch
    }

    public static class OrderPayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trHeader"></param>
        /// <param name="childTRs"></param>
        public static void UpdateFromHeader(TRN_HEADER trHeader, object[] childTRs, params string[] headerFields)
        {
            if (childTRs.Length == 0)
            {
                return;
            }

            List<PropertyInfo> pis = new List<PropertyInfo>();
            foreach (var hf in headerFields)
            {
                var pi = childTRs[0].GetType().GetProperty(hf, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null) pis.Add(pi);
            }

            foreach (var childTR in childTRs)
            {
                foreach (var pi in pis)
                {
                    var hpi = trHeader.GetType().GetProperty(pi.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (hpi == null)
                    {
                        continue;
                    }

                    var hval = hpi.GetValue(trHeader);
                    pi.SetValue(childTR, hval);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numType">0: int/qty: 1: decimal/amount</param>
        /// <returns></returns>
        public static bool ValidateNumber(this object number, int numType)
        {
            switch (numType)
            {
                case 0:
                    return (int)number <= 999;
                default:
                    return (decimal)number <= 99999999;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static T GetPropValue<T>(this object obj, string propName)
        {
            var pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payTenders"></param>
        /// <returns></returns>
        public static List<COMPPAY_PAY_INFO> ToPayInfos(this List<TRN_TENDERSEQ> payTenders,
                TRN_CARD[] payCards, TRN_CASH[] payCashs, TRN_CASHREC[] payCashRecs,
                TRN_GIFT[] payGifts, TRN_FOODCPN[] payFoodCpns, TRN_PARTCARD[] payPartCards,
                TRN_EASYPAY[] payEasys, TRN_POINTUSE[] paypoints, TRN_POINTSAVE payPointSave,
                TRN_PPCARD[] payPpCards)
        {
            var payInfos = new List<COMPPAY_PAY_INFO>();
            int i = 0;
            while (i < payTenders.Count)
            {
                string useText = DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "포인트" :
                    (DataLocals.AppConfig.PosOption.StampUseMethod == "0" ? "금액" : "쿠폰");
                var payTender = payTenders[i];
                var pi = new COMPPAY_PAY_INFO()
                {
                    PAY_SEQ = (i + 1).ToString(),
                    PAY_TYPE_CODE = payTender.PAY_TYPE_FLAG,
                    PAY_TYPE_CODE_FG = useText,
                    PAY_VM_NANE = OrderPayConsts.PayVMByTypeFlag(payTender.PAY_TYPE_FLAG),
                    PAY_CLASS_NAME = OrderPayConsts.PayTRNClassByTypeFlag(payTender.PAY_TYPE_FLAG),
                    APPR_AMT = payTender.PAY_AMT,
                    APPR_PROC_FLAG = "1",
                    PROC_METHOD = "A"
                };

                string apprIdt = string.Empty;
                string apprNo = string.Empty;
                string procFlag = "A";

                switch (payTender.PAY_TYPE_FLAG)
                {
                    case OrderPayConsts.PAY_GIFT:
                    case OrderPayConsts.PAY_MEAL:
                        List<object> subPayDatas = new List<object>();

                        // next items in collectin with same type
                        List<string> payLineNos = new List<string>();
                        int jumpIndex = -1;
                        for (int j = i; j < payTenders.Count; j++)
                        {
                            if (payTenders[j].PAY_TYPE_FLAG != payTender.PAY_TYPE_FLAG)
                            {
                                jumpIndex = j;
                                break;
                            }

                            payLineNos.Add(payTenders[j].LINE_NO);
                        }

                        decimal apprAmt = 0;
                        foreach (var lineNo in payLineNos)
                        {
                            if (OrderPayConsts.PAY_GIFT.Equals(payTender.PAY_TYPE_FLAG))
                            {
                                var pg = payGifts.FirstOrDefault(p => p.SEQ_NO == lineNo);
                                apprAmt += pg == null ? 0 : pg.TK_GFT_AMT;
                                subPayDatas.Add(pg);
                            }
                            if (OrderPayConsts.PAY_MEAL.Equals(payTender.PAY_TYPE_FLAG))
                            {
                                var pg = payFoodCpns.FirstOrDefault(p => p.SEQ_NO == lineNo);
                                subPayDatas.Add(pg);
                                apprAmt += pg == null ? 0 : pg.TK_FOD_AMT;
                            }
                        }

                        pi.APPR_AMT = apprAmt;
                        pi.PayDatas = subPayDatas.ToArray();

                        // if not -1, mean  jumpIndex is an item differ from current type, should set i to this item (same item type is added into subPayDatas)
                        if (jumpIndex != -1)
                        {
                            i = jumpIndex - 1; // -1 means: below code  +1 (i++) so minus 1 to balance
                        }
                        break;
                    case OrderPayConsts.PAY_CASH:
                        pi.PayDatas = payCashs.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.PAY_CASHREC:
                        var payCashRec = payCashRecs.FirstOrDefault(p => p.SEQ_NO == payTender.LINE_NO);
                        apprIdt = payCashRec.APPR_IDT_NO;
                        apprNo = payCashRec.APPR_NO;
                        procFlag = "M";
                        pi.PayDatas = payCashRecs.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.PAY_CARD:
                        var payCard = payCards.FirstOrDefault(p => p.SEQ_NO == payTender.LINE_NO);
                        apprIdt = payCard.CRD_CARD_NO;
                        apprNo = payCard.APPR_NO;
                        procFlag = "M";
                        pi.PayDatas = payCards.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.PAY_EASY:
                        var payEasy = payEasys.FirstOrDefault(p => p.SEQ_NO == payTender.LINE_NO);
                        apprIdt = payEasy.PAY_CARD_NO;
                        apprNo = payEasy.APPR_NO;
                        procFlag = "M";
                        pi.PayDatas = payEasys.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.PAY_PARTCARD:
                        pi.PayDatas = payPartCards.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.PAY_POINTS:
                        pi.PayDatas = paypoints.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    case OrderPayConsts.SAVE_POINTS:
                        pi.PayDatas = new TRN_POINTSAVE[] { payPointSave };
                        break;
                    case OrderPayConsts.PAY_PPCARD:
                        procFlag = "M";
                        pi.PayDatas = payPpCards.Where(p => p.SEQ_NO == payTender.LINE_NO).ToArray();
                        break;
                    default:
                        break;
                }

                pi.APPR_IDT_NO = apprIdt;
                pi.APPR_NO = apprNo;
                pi.PROC_METHOD = procFlag;

                if (pi.PAY_TYPE_CODE != OrderPayConsts.SAVE_POINTS)
                    payInfos.Add(pi);

                i++;
            }

            return payInfos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payViewModel"></param>
        /// <returns></returns>
        public static bool IsPayViewModel(string payViewModel)
        {
            string[] payViewModels = new string[] {
                "OrderPayMemberPointUseViewModel",
                "OrderPayMemberPointSaveViewModel",
                "OrderPayCardViewModel",
                "OrderPayCashViewModel",
                "OrderPayCoprtnDscntViewModel",
                "OrderPayCouponViewModel",
                "OrderPayCreditSetleViewModel",
                "OrderPayDutchPayTab1ViewModel",
                "OrderPayDutchPayTab2ViewModel",
                "OrderPayMealViewModel",
                "OrderPayMobileSetleViewModel",
                "OrderPayMobileCouponViewModel",
                "OrderPayPrepaidCardTab1ViewModel",
                "OrderPayPrepaidCardTab2ViewModel",
                "OrderPaySetleAditViewModel",
                "OrderPaySimplePayViewModel",
                "OrderPayUnionViewModel",
                "OrderPayVoucherViewModel",
                "OrderPayPrepaymentUseViewModel"
            };

            return payViewModels.Contains(payViewModel);
        }
    }

    public enum RetainLastOrderTypes
    {
        None = 0,
        RetainInMain,
        RetainInDual,
        RetainBoth
    }
}