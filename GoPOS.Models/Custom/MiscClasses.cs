using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class FkMapInfo
    {
        public string FK_NO { get; set; }
        public string ViewModelName { get; set; }
        public FkMapActionTypes IsPopup { get; set; } = FkMapActionTypes.Action;
        public string ItemArea { get; set; }
        public object[] CSParams { get; set; }

        public FkMapInfo(string fkNo, string mapString)
        {
            this.FK_NO = fkNo;
            var maps = mapString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (maps.Length > 0)
            {
                /// vm,1,params
                /// vm,0,ActiveItem,params
                this.ViewModelName = maps[0];
                IsPopup = (FkMapActionTypes)maps[1].ToInt32(); // maps.Length >= 2 ? "1".Equals(maps[1]) : false;
                ItemArea = maps.Length >= 3 ? maps[2] : string.Empty;

                int startIdx = 2;
                if (IsPopup != FkMapActionTypes.Popup)
                {
                    startIdx = 3;
                }

                List<object> pms = new List<object>();
                for (int i = startIdx; i < maps.Length; i++)
                {
                    pms.Add(maps[i]);
                }

                CSParams = pms.ToArray();
            }
        }

    }    

    public enum FkMapActionTypes
    {
        ActiveItem,
        Popup,
        Action
    }

    public class MasterTableIdInfo
    {
        public string MST_ID { get; set; }

        /// <summary>
        /// Physical name
        /// </summary>
        public string MST_TPNAME { get; set; }        

        /// <summary>
        /// Logical Name
        /// </summary>
        public string MST_TLNAME { get; set; }
    }
    
    public class OrderPayConsts
    {
        public const string PAY_CASH = "01";
        public const string PAY_CASHREC = "02";
        public const string PAY_CARD = "03";
        public const string PAY_CARD_ER = "04";
        public const string PAY_EASY = "05";
        public const string PAY_PARTCARD = "06";
        public const string PAY_GIFT = "07";
        public const string PAY_MEAL = "08";
        public const string PAY_CREDIT = "09";
        public const string PAY_POINTS = "15";
        public const string SAVE_POINTS = "14";
        /// <summary>
        /// 선결제
        /// </summary>
        public const string PAY_PPCARD = "11";

        /// <summary>
        /// 01:현금, 02:현금영수증, 03:신용카드, 04:은련카드, 05:간편결제,
        /// 06:제휴할인카드, 07:상품권, 08:식권, 09:외상, 10:선불카드,
        /// 11:선결제, 12:전자상품권, 13:모바일상품권, 14:회원포인트적립, 15:회원포인트사용,
        /// 16:사원카드
        /// </summary>
        /// <param name="payCode"></param>
        /// <param name="isViewModelName"></param>
        /// <returns></returns>
        public static string PayTypeFlagNameByCode(string payCode, params object[] adds)
        {
            if (payCode == "15")
            {
                return adds.Length > 0 ? "스탬프" + adds[0] : "포인트사용";
            }
            var desc = @"01:현금,02:현금영수증,03:신용카드,04:은련카드,05:간편결제,06:제휴할인,07:상품권,08:식권,09:외상,10:선불카드,11:선결제,12:전자상품권,13:모바일상품권,14:포인트적립,15:포인트사용,16:사원카드";

            var ccc = desc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var pairs = ccc.FirstOrDefault(p => p.StartsWith(payCode));
            return pairs != null ? pairs.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1] : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trnClass"></param>
        /// <returns></returns>
        public static string PayTypeFlagByTRNClass(string trnClass, object[] payDatas)
        {
            switch (trnClass)
            {
                case nameof(TRN_CASH):
                    return OrderPayConsts.PAY_CASH;
                case nameof(TRN_CASHREC):
                    return OrderPayConsts.PAY_CASHREC;
                case nameof(TRN_CARD):
                    return OrderPayConsts.PAY_CARD;
                case nameof(TRN_EASYPAY):
                    return OrderPayConsts.PAY_EASY;
                case nameof(TRN_PARTCARD):
                    return OrderPayConsts.PAY_PARTCARD;
                case nameof(TRN_GIFT):
                    if (payDatas.Length > 0 && payDatas[payDatas.Length - 1] is TRN_CASHREC)
                    {
                        return OrderPayConsts.PAY_CASHREC;
                    }

                    return OrderPayConsts.PAY_GIFT;
                case nameof(TRN_FOODCPN):
                    return OrderPayConsts.PAY_MEAL;
                case nameof(TRN_POINTUSE):
                    return OrderPayConsts.PAY_POINTS;
                case nameof(TRN_POINTSAVE):
                    return OrderPayConsts.SAVE_POINTS;
                case nameof(TRN_PPCARD):
                    return OrderPayConsts.PAY_PPCARD;
                default:
                    return "00";
            }
        }
        public static string PayTypeFlagNameByVM(string vmName)
        {
            switch (vmName)
            {
                case "OrderPayCoprtnDscntViewModel":
                    return "제휴할인";
                case "OrderPayMealViewModel":
                    return "식권";
                case "OrderPayVoucherViewModel":
                    return "상품권";
                case "OrderPayMemberPointUseViewModel":
                    return "포인트";
                case "OrderPayPrepaymentUseViewModel":
                    return "선결제";
                default:
                    return "결제";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAY_TYPE_FLAG"></param>
        /// <returns></returns>
        public static string PayVMByTypeFlag(string? pAY_TYPE_FLAG)
        {
            switch (pAY_TYPE_FLAG)
            {
                case PAY_CARD:
                    return "OrderPayCardViewModel";
                case PAY_CASH:
                case PAY_CASHREC:
                    return "OrderPayCashViewModel";
                case PAY_EASY:
                    return "OrderPaySimplePayViewModel";
                case PAY_GIFT:
                    return "OrderPayVoucherViewModel";
                case PAY_MEAL:
                    return "OrderPayMealViewModel";
                case PAY_PARTCARD:
                    return "OrderPayCoprtnDscntViewModel";
                case PAY_POINTS:
                    return "OrderPayMemberPointUseViewModel";
                case SAVE_POINTS:
                    return "OrderPayMemberPointSaveViewModel";
                default:
                    return string.Empty;
            }
        }

        public static string PayTRNClassByTypeFlag(string? pAY_TYPE_FLAG)
        {
            switch (pAY_TYPE_FLAG)
            {
                case PAY_CARD:
                    return nameof(TRN_CARD);
                case PAY_CASH:
                    return nameof(TRN_CASH);
                case PAY_CASHREC:
                    return nameof(TRN_CASHREC);
                case PAY_EASY:
                    return nameof(TRN_EASYPAY);
                case PAY_GIFT:
                    return nameof(TRN_GIFT);
                case PAY_MEAL:
                    return nameof(TRN_FOODCPN);
                case PAY_PARTCARD:
                    return nameof(TRN_PARTCARD);
                case PAY_POINTS:
                    return nameof(TRN_POINTUSE);
                case SAVE_POINTS:
                    return nameof(TRN_POINTSAVE);
                default:
                    return string.Empty;
            }
        }
    }

    public sealed class PosOptionAttr : Attribute
    {
        private int envScope;
        private string envSetCode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">0: SHOP, 1:POS</param>
        /// <param name="envSetCode"></param>
        public PosOptionAttr(int scope, string envSetCode, bool useValue) : this(scope, envSetCode, useValue, false)
        {
        }

        public PosOptionAttr(int scope, string envSetCode, bool useValue, bool parseInt)
        {
            this.envScope = scope;
            this.envSetCode = envSetCode;
            this.UseValue = useValue;
            this.ParseInt = parseInt;
        }

        public int EnvScope { get => envScope; set => envScope = value; }
        public string EnvSetCode { get => envSetCode; set => envSetCode = value; }
        public bool UseValue { get; set; }
        public bool ParseInt { get; set; }
    }
}