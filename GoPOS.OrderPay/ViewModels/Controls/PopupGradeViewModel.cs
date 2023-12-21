using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Common.ViewModels;
using GoPOS.Common.Interface.View;
using GoPOS.Service;
using GoPOS.Common.Interface.Model;
using System.Globalization;
using GoPOS.Models.Common;
using GoPOS.Service.Common;
using GoShared.Helpers;
using GoPOS.ViewModels;
using static AutoMapper.Internal.ExpressionFactory;
using GoPOS.Payment.Services;


/*
 공통 > 달력 컨트롤

 */

namespace GoPOS.OrderPay.ViewModels.Controls
{

    public class PopupGradeViewModel : BaseItemViewModel, IDialogViewModel
    {
        private int _index = -1;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }
        private readonly IOrderPayService orderPayService;
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public ObservableCollection<MEMBER_GRADE> MemberGrade { get; set; }
        public PopupGradeViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
             IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += PopupGradeViewModel_ViewInitialized;
            this.ViewLoaded += PopupGradeViewModel_ViewLoaded;
            this.orderPayPointStampService = orderPayPointStampService;
        }

        private void PopupGradeViewModel_ViewInitialized(object? sender, EventArgs e)
        {
        }

        private void PopupGradeViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            GetMemberGradeList();

        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCtrl);

        public Dictionary<string, object> DialogResult { get; set; }

        private void ButtonCommandCtrl(Button btn)
        {
            switch (btn.Tag)
            {
                case "Previous":

                    break;
                case "Next":

                    break;
                default:
                    break;
            }
        }

        public override bool SetIView(IView view)
        {
            return false;
        }

        public override void SetData(object data)
        {
            object[] ds = data as object[];
            if (ds.Count() > 0)
            {
                MemberGrade = (ObservableCollection<MEMBER_GRADE>)ds[0];
            }

            base.SetData(data);
        }
        public void SetGrades(string code) // Catapul data to member register screen
        {
            if (code == "")
            {
                return;
            }
            IoC.Get<OrderPayMemberRegistViewModel>().MemberGrade = MemberGrade.FirstOrDefault(x => x.grdCode == code);
            this.DeactivateAsync(true);
        }

        private async void GetMemberGradeList()
        {
            if (DataLocals.MemberGrades == null)
            {
                string errorMessage = string.Empty;
                var results = await orderPayPointStampService.GetMEMBER_GRADEs();
                if (!string.IsNullOrEmpty(results.Item2))
                {
                    DialogHelper.MessageBox(results.Item2, GMessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    DataLocals.MemberGrades = results.Item1;
                }
            }

            this.MemberGrade = new ObservableCollection<MEMBER_GRADE>(DataLocals.MemberGrades);
            NotifyOfPropertyChange(() => MemberGrade);
        }
    }
}