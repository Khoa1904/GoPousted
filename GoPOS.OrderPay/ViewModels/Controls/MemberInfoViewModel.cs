using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Models;
using GoPOS.Service;
using GoPOS.ViewModels;
using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.OrderPay.ViewModels.Controls
{
    public class MemberInfoViewModel : BaseItemViewModel, IViewModel, IHandle<MemberInfoPass>
    {
        private string stampFlag { get; set; } = DataLocals.AppConfig.PosOption.PointStampFlag;
        private bool _buttonClick { get; set; } = true;
        private bool memberLock { get; set; } = false;
        public bool ButtonClick
        {
            get => _buttonClick;
            set
            {
                _buttonClick = value;
                NotifyOfPropertyChange(() => ButtonClick);
            }
        }
        private MEMBER_CLASH _memInfo;
        public MemberInfoViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {
            this.ViewLoaded += MemberInfoViewModel_ViewLoaded;
        }

        private void MemberInfoViewModel_ViewLoaded(object? sender, EventArgs e)
        {
        }

        public IScreen ActiveItem { get; set; }

        public void SetData(object data)
        {
        }

        public MEMBER_CLASH MemberInfo
        {
            get => _memInfo;
            set { _memInfo = value; NotifyOfPropertyChange(() => MemberInfo); }

        }
        public string MemberAccPoint
        {
            get
            {
                if(stampFlag == "0")
                {
                    return "누적포인트 :";
                }
                else
                {
                    return "누적스탬프 :";
                }
            }
        }
        public string MemberAvailPoint
        {
            get
            {
                if (stampFlag == "0")
                {
                    return "가용포인트 :";
                }
                else
                {
                    return "가용스탬프 :";
                }
            }
        }
        public string MemberBalance
        {
            get
            {
                /*
                if (stampFlag == "0")
                {
                    return "외상포인트 :";
                }
                else
                {
                    return "외상스탬프 :";
                }
                */
                return "외상미수금";
            }
        }



        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button btn)
        {
           var main= IoC.Get<OrderPayMainViewModel>();
            var count = main.OrderItems.Count();

            if (btn.Tag is null)
            {
                return;
            }
            if(!ButtonClick)
            {
                return;
            }
            switch (btn.Tag)
            {
                case "MemberSearch":
                    if (ValidatePointUseExists())
                    {
                        break;
                    }
                    
                    IoC.Get<OrderPayMainViewModel>().ActiveForm("ActiveItemR", typeof(OrderPayMemberSearchViewModel));
                    break;
                case "Reset":
                    if (ValidatePointUseExists())
                    {
                        break;
                    }

                    if (MemberInfo != null)
                    {
                        IoC.Get<OrderPayMainViewModel>().MemberDiscount(MemberInfo, false);
                    }

                    _eventAggregator.PublishOnUIThreadAsync(new MemberInfoPass()
                    {
                        memberInfo = null
                    });

                    IoC.Get<OrderPayMainViewModel>().MemberInfo = null;

                    if (count == 0)
                    {
                        IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Logo);
                    }
                    break;
                case "MemberDetails":
                    if (MemberInfo == null)
                    {
                        IoC.Get<OrderPayMainViewModel>().StatusMessage = "회원을 먼저 선택하십시오.";
                        return;
                    }
                    IoC.Get<OrderPayMainViewModel>().ActiveForm("ActiveItemR", "OrderPayMemberProfileViewModel", MemberInfo.mbrCode);
                    break;
                default:
                    break;
            }
        }


        private bool ValidatePointUseExists()
        {
            //회원적립여부구분에 따라서 메세지변경
            var nTypeNm = "";
            if (stampFlag == "0")
            {
                nTypeNm = "포인트/선결제";
            } else
            {
                nTypeNm = "스탬프/선결제";
            }

            if (IoC.Get<OrderPayMainViewModel>().payPoints.Count > 0 ||
                IoC.Get<OrderPayMainViewModel>().payPpCards.Count > 0)
            {
                IoC.Get<OrderPayMainViewModel>().StatusMessage = nTypeNm + " 사용 내역이 있어 초기화 할 수 없습니다.";
                DialogHelper.MessageBox(nTypeNm + " 사용 내역이 있어 회원정보를\r\n초기화 할 수 없습니다.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return true;
            }
            if(memberLock)
            {
                DialogHelper.MessageBox("이미 선결제한 회원이 있습니다.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return true;
            }
            return false;
        }

        public Task HandleAsync(MemberInfoPass message, CancellationToken cancellationToken)
        {
            this.MemberInfo = message.memberInfo;
            return Task.CompletedTask;
        }
    }

    
}
