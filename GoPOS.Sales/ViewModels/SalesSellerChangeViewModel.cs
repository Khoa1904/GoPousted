using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Caliburn.Micro;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.ViewModels.Controls;
using GoPOS.Views;
using Microsoft.Xaml.Behaviors.Media;
using GoPOS.Models.Common;
using GoPOS.Common.ViewModels;
using GoPOS.SalesMng.Interface.View;
using GoPOS.Common.Interface.View;
using GoPOS.Common;
using GoShared.Events;



/*
 영업관리 > 판매원변경
 */
namespace GoPOS.ViewModels
{
    public class SalesSellerChangeViewModel : OrderPayChildViewModel
    {
        private readonly IInfoEmpService empService;
        private string eMP_NAME;

        public MST_INFO_EMP CurEmployee
        {
            get
            {
                return DataLocals.Employee;
            }
        }

        public string EMP_NAME
        {
            get => eMP_NAME; set
            {
                eMP_NAME = value;
                NotifyOfPropertyChange(nameof(EMP_NAME));
            }
        }
        public string EMP_NO
        {
            get => eMP_NO; set
            {
                eMP_NO = value;
                NotifyOfPropertyChange(nameof(EMP_NO));
                checkEmployee = null;
            }
        }
        public string EMP_PWD { get; set; }

        private MST_INFO_EMP checkEmployee = null;
        private string eMP_NO;
        private ISellerChangeView _view;

        public SalesSellerChangeViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IInfoEmpService empService) : base(windowManager, eventAggregator)
        {
            this.empService = empService;
            this.ViewLoaded += SalesSellerChangeViewModel_ViewLoaded;
        }

        private void SalesSellerChangeViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            this.EMP_NO = string.Empty;
            this.EMP_NAME = string.Empty;
            _view.txtEMP_PWD.Text = string.Empty;
        }

        public override bool SetIView(IView view)
        {
            _view = (ISellerChangeView)view;
            return base.SetIView(view);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(button =>
        {
            if ("SearchEmployee".Equals(button.Tag.ToString()))
            {
                var exs = empService.CheckExist(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.EMP_NO == EMP_NO);
                if (!exs)
                {
                    DialogHelper.MessageBox("사원정보 존재하지 않습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                checkEmployee = empService.GetEmpInfo(EMP_NO);
                this.EMP_NAME = checkEmployee.EMP_NAME;
                exs = checkEmployee.EMP_PWD == _view.txtEMP_PWD.Text;

                if (!exs)
                {
                    DialogHelper.MessageBox("비밀번호 일치하지 않습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                if (checkEmployee == null)
                {
                    DialogHelper.MessageBox("사용자정보 조회 먼저하십시오.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var exs = checkEmployee.EMP_PWD == _view.txtEMP_PWD.Text;
                if (!exs)
                {
                    DialogHelper.MessageBox("비밀번호 일치하지 않습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var res = DialogHelper.MessageBox("판매원 변경하시겠습니까?.", GMessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }

                DialogHelper.MessageBox("사용자 변경 되었습니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
                DataLocals.Employee = checkEmployee;
                DataLocals.PosStatus.EMP_NO = checkEmployee.EMP_NO;

                _eventAggregator.PublishOnUIThreadAsync(new LoggedInUserChange()
                {
                    ChangedEmpNo = checkEmployee.EMP_NO
                });

                _eventAggregator.PublishOnUIThreadAsync(new PageItemEventArgs()
                {
                    ItemName = "EmployeeChanged",
                    EventType = PageItemEventTypes.DataEvent
                });

                this.EMP_NO = string.Empty;
                this.EMP_NAME = string.Empty;
                this.EMP_PWD = string.Empty;
                _view.txtEMP_PWD.Text = string.Empty;
                NotifyOfPropertyChange(() => CurEmployee);
            }
        });
    }
}
