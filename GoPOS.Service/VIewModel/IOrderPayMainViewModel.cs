using GoPOS.Models;
using GoPOS.Models.Custom.Payment;
using GoPOS.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace GoPOS.ViewModels
{
    public interface IOrderPayMainViewModel
    {
        List<FkMapInfo>? FunKeyMaps { get; set; }
        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }
        List<ORDER_FUNC_KEY>? PayBtnKeys { get; set; }
        ObservableCollection<ORDER_GRID_ITEM> OrderItems { get; set; }
        ORDER_SUM_INFO OrderSumInfo { get; }
        List<COMPPAY_PAY_INFO> payInfos { get; }
        TRN_HEADER trHeader { get; }
        List<TRN_PRDT> trProducts { get; }
        List<TRN_TENDERSEQ> payTenders { get; }
        List<TRN_CASH> payCashs { get; }
        List<TRN_CASHREC> payCashRecs { get; }
        List<TRN_CARD> payCards { get; }
        List<TRN_GIFT> payGifts { get; }
        List<TRN_FOODCPN> payFoodCpns { get; }
        List<TRN_EASYPAY> payEasys { get; }

        string StatusMessage { get; set; }

        void DiscountApply(bool isAll, bool isAmt, bool isApp, decimal fixDc);
        void ProcessFuncKeyClicked(string fnKeyNo);
        void ProcessFuncKeyClicked(ORDER_FUNC_KEY fkKey, params object[] csParams);
        void ProcessFuncKeyClicked(FkMapInfo mapKey, params object[] csParams);
        void TouchProductClicked(ORDER_TU_PRODUCT touchProduct);
        void ActiveForm(string formName, Type formItemType);

        bool UpdatePaymentTRN(string viewModelName, COMPPAY_PAY_INFO payInfo);
        bool RemovePaymentTRN(COMPPAY_PAY_INFO payInfo);
        void ItemGrid_OnItemAdded(params MST_INFO_PRODUCT[] infoProducts);
        int GetTRPaySeq(string payViewModel);
        void LoadExistOrderTRs(Dictionary<string, object> trDatas);
    }
}
    