using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Payment.Services;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Contract;
using GoShared.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AutoMapper.Internal.ExpressionFactory;
using static Dapper.SqlMapper;


/*
 주문 > 확장메뉴 > 회원등록

 */

namespace GoPOS.ViewModels
{

    public class OrderPayMemberProfileViewModel : OrderPayChildViewModel, IHandle<MemberInfoPass>
    {
        private MEMBER_CLASH _memberInfo;

        public OrderPayMemberProfileViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayPointStampService orderPayPointStampService)
            : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayMemberProfileViewModel_ViewLoaded;
            this.orderPayPointStampService = orderPayPointStampService;
        }

        private void OrderPayMemberProfileViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            MEMBERINFO = null;
        }

        public MEMBER_CLASH MEMBERINFO
        {
            get { return _memberInfo; }
            set
            {
                _memberInfo = value;
                NotifyOfPropertyChange(() => MEMBERINFO);
            }
        }

        public string avalidPointTitle
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "가용포인트" : "가용스탬프";
            }
        }

        public string accPointTitle
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "누적포인트" : "누적스탬프";
            }
        }

        public string usePointTitle
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "사용포인트" : "사용스탬프";
            }
        }
        

        public async override void SetData(object data)
        {
            base.SetData(data);
            if (data != null)
            {
                string memID = (data as object[])[0] as string;
                var mres = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, memID);
                if (string.IsNullOrEmpty(mres.Item1))
                {
                    MEMBERINFO = mres.Item2;
                }
                else
                {
                    DialogHelper.MessageBox(mres.Item1, GMessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private ServerTime time;
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        public void ButtonCommandCenter(Button btn)
        {
            switch (btn.Tag)
            {

                default: break;
            }
        }

        public Task HandleAsync(MemberInfoPass message, CancellationToken cancellationToken)
        {
            this.MEMBERINFO = message.memberInfo;
            return Task.CompletedTask;
        }
    }
}