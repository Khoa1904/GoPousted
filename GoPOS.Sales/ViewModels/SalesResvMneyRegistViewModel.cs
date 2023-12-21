using AutoMapper;
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GoPOS.Function;


/*
 영업관리 > 준비금등록
 */
namespace GoPOS.ViewModels
{
    
    public class SalesResvMneyRegistViewModel : BaseItemViewModel
    {
        private readonly ISalesResvMneyRegistService salesResvMneyRegistService;

        private string _saleDate;
        public string SaleDate
        {
            get { return _saleDate; }
            set
            {
                if (_saleDate != value)
                {
                    _saleDate = value;
                    NotifyOfPropertyChange(() => SaleDate);
                }
            }
        }

        private string _regiSeq;
        public string RegiSeq
        {
            get { return _regiSeq; }
            set
            {
                if (_regiSeq != value)
                {
                    _regiSeq = value;
                    NotifyOfPropertyChange(() => RegiSeq);
                }
            }
        }

        private string _empNo;
        public string EmpNo
        {
            get { return _empNo; }
            set
            {
                if (_empNo != value)
                {
                    _empNo = value;
                    NotifyOfPropertyChange(() => EmpNo);
                }
            }
        }

        private decimal _posReadyAmt;
        public decimal PosReadyAmt
        {
            get { return _posReadyAmt; }
            set
            {
                if (_posReadyAmt != value)
                {
                    _posReadyAmt = value;
                    NotifyOfPropertyChange(() => PosReadyAmt);
                }
            }
        }

        public SalesResvMneyRegistViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,ISalesResvMneyRegistService salesResvMneyRegistService) :
            base(windowManager, eventAggregator)
        {
            this.salesResvMneyRegistService = salesResvMneyRegistService;
            this.ViewLoaded += SalesResvMneyRegistViewModel_ViewLoaded;
            Init();
        }

        private void SalesResvMneyRegistViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            Init();
        }

        public async void Init()
        {
            await GetResvMney();
        }

        POS_SETTLEMENT_DETAIL ResvMney = null;
        private async Task GetResvMney()
        {
            try
            {
                ResvMney = await salesResvMneyRegistService.GetResvMney();
                if (ResvMney == null) return;
                SetData();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("시재입출금 List 가져오기 오류 : " + ex.Message);
            }
        }

        private void SetData()
        {
            DateTime saleDate = DateTime.ParseExact(ResvMney.SALE_DATE, "yyyyMMdd", null);
            SaleDate = saleDate.ToString("yyyy-MM-dd");
            RegiSeq = ResvMney.REGI_SEQ;
            //EmpNo = ResvMney.EMP_NO + "-" + ResvMney.EMP_NAME
            EmpNo = DataLocals.Employee.EMP_NO + "-" + DataLocals.Employee.EMP_NAME;
        }

        private async void Insert()
        {
            if (ResvMney == null) return;
            try
            {

                if (DialogHelper.MessageBox("준비금을 등록하시겠습니까?") != MessageBoxResult.OK)
                {
                    return;
                }

                SpResult spResult = await salesResvMneyRegistService.SaveSalesResvMneyRegist(ResvMney.SALE_DATE, ResvMney.REGI_SEQ, PosReadyAmt);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    LogHelper.Logger.Error("Insert" + spResult.ResultType);
                    return;
                }

                DialogHelper.MessageBox("준비금을 등록하였습니다.");
            }
            catch (Exception a)
            {
                LogHelper.Logger.Error(a.Message);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "INSERT":
                    Insert();
                    break;
                default:
                    break;
            }
        });

    }
}