using Caliburn.Micro;
using GoPOS.Common.Service;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Payment.Models;
using GoPOS.Service;
using GoShared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static AutoMapper.Internal.ExpressionFactory;

namespace GoPOS.ViewModels;

/// <summary>
/// For Khoa coding
/// </summary>
public partial class OrderPayMainViewModel : IHandle<MemberInfoPass>
{
    private MEMBER_CLASH memberInfo { get; set; }
    public MEMBER_CLASH MemberInfo
    {
        get => memberInfo;
        set
        {
            memberInfo = value;
            NotifyOfPropertyChange(() => MemberInfo);
        }
    }
    public Task HandleAsync(OrderPayChildUpdatedEventArgs message, CancellationToken cancellationToken)
    {
        if (!"ODP_MAIN".Equals(message.CallerId))
        {
            return Task.CompletedTask;
        }
        if (message.Cancelled)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapKey"></param>
    /// <param name="csParams"></param>
    public override void ProcessFuncKeyClicked(FkMapInfo mapKey, params object[] csParams)
    {
        _view.ResetSelectedExtButton();

        #region Price change
        if (mapKey.ItemArea == "ChangePrice" && SelectedItemIndex >= 0)
        {
            if (SelectedItemIndex < 0)
            {
                return;
            }

            var mItem = OrderItems[SelectedItemIndex];
            string newPrice = IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text;
            if (int.Parse(newPrice) > 1)
            {
                ChangeGridPrice(mItem, int.Parse(newPrice));
            }
            else
            {
                StatusMessage = "새로운 단가를 입력하여 주십시오.";
            }
        }
        #endregion

        #region Webinfo
        if (mapKey.ItemArea == "WebInfo")
        {
            string token = DataLocals.TokenInfo.TOKEN;
            string empNo = DataLocals.PosStatus.EMP_NO;
            string url = DataLocals.AppConfig.PosComm.AspURLServer + "/pos/login/store?accessTkn=" + token + "&staffNo=" + empNo;
            var OMG = DialogHelper.MessageBox("웹 영업정보 시스템으로 로그인하시겠습니까 ?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (OMG == MessageBoxResult.OK)
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Packing and PackingAll - 포장 & 전체포장

        if (mapKey.ItemArea.StartsWith("PackItem"))
        {
            if (mapKey.ItemArea.Equals("PackItem"))
            {
                if (SelectedItemIndex < 0)
                {
                    return;
                }
                if (OrderItems[SelectedItemIndex].DLV_PACK_FLAG == "2")
                {
                    OrderItems[SelectedItemIndex].DLV_PACK_FLAG = "0";
                    OrderItems[SelectedItemIndex].UpdateRemarkItem("PACK", "", false);

                }
                else
                {
                    OrderItems[SelectedItemIndex].DLV_PACK_FLAG = "2";
                    OrderItems[SelectedItemIndex].UpdateRemarkItem("PACK", "포장", true);
                }
            }
            else
            {
                for (int i = 0; i < OrderItems.Count; i++)
                {
                    if (OrderItems[i].DLV_PACK_FLAG == "2")
                    {
                        OrderItems[i].DLV_PACK_FLAG = "0";
                        OrderItems[i].UpdateRemarkItem("PACK", "", false);
                    }
                    else
                    {
                        OrderItems[i].DLV_PACK_FLAG = "2";
                        OrderItems[i].UpdateRemarkItem("PACK", "포장", true);
                    }
                }
            }
        }

        #endregion

        #region 교환 - ReturnItem
        if (mapKey.ItemArea.StartsWith("ReturnItem"))
        {
            if (SelectedItemIndex < 0)
            {
                return;
            }
            OrderItems[SelectedItemIndex].SALE_QTY = -1 * OrderItems[SelectedItemIndex].SALE_QTY;
            OrderItems[SelectedItemIndex].UpdateRemarkItem("RETURN", OrderItems[SelectedItemIndex].SALE_QTY < 0 ? "교환" : "", false);
            var childItems = GetChildItems(OrderItems[SelectedItemIndex].PRD_CODE, SelectedItemIndex, 0).Item1;
            foreach (var ci in childItems)
            {
                ci.SALE_QTY = -1 * ci.SALE_QTY;
                ci.UpdateRemarkItem("RETURN", ci.SALE_QTY < 0 ? "교환" : "", false);
            }
            UpdateTRSummary();
        }
        #endregion

        #region 환전 - OpenCashDrawer

        if ("OpenCashDrawer".Equals(mapKey.ItemArea))
        {
            pOSPrintService?.OpenCashDrawer(true);
        }

        #endregion

        #region Receipt Return - 반품

        if (mapKey.ItemArea == "ReceiptReturn" && mapKey.IsPopup == FkMapActionTypes.Action)
        {
            this.ActiveForm("ActiveItemR", "OrderPayReceiptReturnViewModel", ValidateOnChildActivated, csParams);
        }

        #endregion

        #region PrevBill - 직전영수증

        if (mapKey.ItemArea == "PrintPrevBill" && mapKey.IsPopup == FkMapActionTypes.Action)
        {
            pOSPrintService.PrintPrevBill();
        }

        #endregion

        #region 단순현금 - 바로결제

        if (mapKey.ItemArea == "PayCashDirect" && mapKey.IsPopup == FkMapActionTypes.Action)
        {
            string textInput = IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text;
            decimal payAmt = string.IsNullOrEmpty(textInput) ? 0 : Convert.ToDecimal(textInput);
            if (payAmt <= 0)
            {
                payAmt = orderSumInfo.EXP_PAY_AMT;
            }

            var paySeq = (payTenders.Count > 0 ? payTenders.Max(p => p.PAY_SEQ_NO) : 0);
            AddPaymentCashTR(null, null, paySeq, payAmt);
            UpdateTRSummary();
            ProcessTRComplete();
        }

        #endregion

        #region 주방주문서 출력 = PrintKitchenOrder

        if (mapKey.ItemArea == "PrintKitchenOrder" && mapKey.IsPopup == FkMapActionTypes.Action)
        {
            pOSPrintService.PrintKitchenOrder(new OrderInfo()
            {

            });
        }
        #endregion

        #region Table Order
        if (mapKey.ItemArea == "TableOrder" && mapKey.IsPopup == FkMapActionTypes.Action)
        {
            if (_OrderItemCount > 0 && TableCd != null)
            {
                var res = DialogHelper.MessageBox("정말 주문을 마치시겠어요?.)", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.OK)
                {
                    orderPayService.ReplayOverlappedOrder(TableCd);
                    ProcessHoldTR(true);
                }
                else return;
            }
            ClosePage(new string[] { "ActiveItem", "ActiveItemR", "ActiveItemLB" });
        }
        #endregion
    }

    void ChangeGridPrice(ORDER_GRID_ITEM item, int newPrice)
    {
        if (newPrice > 0)
        {
            var oldPrice = item.NORMAL_UPRC;
            item.NORMAL_UPRC = newPrice;
            StatusMessage = item.PRD_NAME + "의 상품이 단가변경: " + oldPrice.ToString("#,#0") + " => " + newPrice.ToString("#,#0");
            UpdateTRSummary();
        }
    }

    public Task HandleAsync(MemberInfoPass message, CancellationToken cancellationToken)
    {
        MemberInfo = message.memberInfo;
        if (OrderItems.Count > 0) { MemberDiscount(memberInfo, true); }
        return Task.CompletedTask;
    }


    #region Print_toggle_button

    public ICommand ToggleButtonCommand => new RelayCommand<ToggleButton>(button =>
    {
        switch (button.Tag.ToString())
        {
            case "Kitchen":
                PrintKitchen = !PrintKitchen;
                ChangeButtonBg(button, PrintKitchen);
                break;
            case "KitchenText":
                PrintKitchen = !PrintKitchen;
                ChangeButtonBg(_view.kitchenText, PrintKitchen);
                break;
            case "Customer":
                PrintCustOrder = !PrintCustOrder;
                ChangeButtonBg(button, PrintCustOrder);
                break;
            case "CustomerText":
                PrintCustOrder = !PrintCustOrder;
                ChangeButtonBg(_view.customerText, PrintCustOrder);
                break;
            case "Bill":
                PrintBill = !PrintBill;
                ChangeButtonBg(button, PrintBill);
                break;
            case "BillText":
                PrintBill = !PrintBill;
                ChangeButtonBg(_view.billText, PrintBill);
                break;
            case "Item":
                PrintItem = !PrintItem;
                ChangeButtonBg(button, PrintItem);
                break;
            case "ItemText":
                PrintBill = !PrintBill;
                ChangeButtonBg(_view.itemText, PrintBill);
                break;
            default:
                break;
        }
    });

    private void ChangeButtonBg(ToggleButton button, bool enable)
    {
        if (enable)
        {
            button.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GoPOS.Resources;component/resource/images/btnPrint2.png", UriKind.RelativeOrAbsolute)));
        }
        else
        {
            button.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GoPOS.Resources;component/resource/images/btnUnprint2.png", UriKind.RelativeOrAbsolute)));

        }
    }

    public bool PrintKitchen
    {
        get; set;
    } = true;
    public bool PrintCustOrder
    {
        get; set;
    } = true;
    public bool PrintBill
    {
        get; set;
    } = true;
    public bool PrintItem
    {
        get; set;
    } = true;

    public Visibility ShowKitchenOrderPrint
    {
        get
        {
            return DataLocals.AppConfig.PosOption.KitOrderPrintOpt == "1" ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public Visibility ShowCustOrderPrint
    {
        get
        {
            return DataLocals.AppConfig.PosOption.CustOrderPrintOpt == "1" ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Read from option
    /// </summary>
    public Visibility ShowPrintItem
    {
        get
        {
            return DataLocals.AppConfig.PosOption.BillItemsPrintOpt == "1" ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public ImageSource DefaultPrintOptionImg
    {
        get
        {
            return new BitmapImage(new Uri("pack://application:,,,/GoPOS.Resources;component/resource/images/btnPrint2.png", UriKind.RelativeOrAbsolute));
        }
    }


    #endregion

}

