using Caliburn.Micro;
using FirebirdSql.Data.Services;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Payment.Interface.View;
using GoPOS.Services;
using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/// <summary>
/// 화면명 : 카드결제
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>

namespace GoPOS.ViewModels
{
    public class OrderPayCardCompListViewModel : OrderPayChildViewModel
    {
        public OrderPayCardCompListViewModel(IWindowManager windowManager,
            IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
        }

        public string CARD_NO { get; set; }
        public string APPR_NO { get; set; }

        public ICommand ChooseCardComp => new RelayCommand<Button>(button =>
        {

            var cardCode = button.Tag.ToString();
            cardCode = cardCode.Substring(cardCode.IndexOf("-") + 1);

            _eventAggregator.PublishOnUIThreadAsync(new OrderPayCardCardCompReturnEventArgs()
            {
                TempCardPay = new TRN_CARD()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,
                    APPR_NO = string.IsNullOrEmpty(APPR_NO) ? "" : APPR_NO,
                    CRD_CARD_NO = string.IsNullOrEmpty(CARD_NO) ? "" : CARD_NO,
                    CRDCP_CODE = cardCode
                }
            });

            this.DeactivateClose(true);
        });
    }
}