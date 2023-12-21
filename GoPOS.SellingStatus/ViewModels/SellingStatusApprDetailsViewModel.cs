using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Sales.Interface.View;
using GoPOS.SellingStatus.Interface;
using GoPOS.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Models.Custom.SellingStatus;
using GoPOS.Views;
using AutoMapper.Mappers;
using static GoPOS.Function;

namespace GoPOS.ViewModels
{
    public class SellingStatusApprDetailsViewModel : BaseItemViewModel, IDialogViewModel
    {
        private ISellingStatusService saleService;
        private List<SELLING_APPR>? _approval = null;
        private ISellingStatusApprDetailsView _view = null;
        private SETT_POSACCOUNT setAccount { get; set; }
        private TRN_GIFT gift { get; set; }
        private TRN_EGIFT egift { get; set; }
        public SellingStatusApprDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISellingStatusService service) 
            : base(windowManager, eventAggregator)
        {
            this.saleService = service;
            this.ViewLoaded += viewLoad;
        }
        public Dictionary<string, object> DialogResult { get; set; }
        private Tuple<string, bool> headerSelect = new Tuple<string, bool>("", false);


        public string RADIOH
        {
            get {
                if ((bool)_view.iRadio1.IsChecked)
                {
                    return _view.iRadio1.Tag.ToString();
                }
                else 
                {
                    return _view.iRadio2.Tag.ToString();
                }
            }
        }

        public string CHECK
        {
            get {
                if ((bool)_view.iCheck1.IsChecked)
                {
                    return _view.iCheck1.Tag.ToString();
                }
                else if ((bool)_view.iCheck2.IsChecked)
                {
                    return _view.iCheck2.Tag.ToString();
                }
                else 
                {
                    return _view.iCheck3.Tag.ToString();
                }
            }
        }
        
        public List<SELLING_APPR>? APPROVAL
        {
            get => _approval;
            set
            {
                _approval = value;
                NotifyOfPropertyChange(() => APPROVAL);
            }
        }
        private ObservableCollection<SELLING_APPR> _CollectionAPPR;
        public ObservableCollection<SELLING_APPR> CollectionAPPR
        {
            get => _CollectionAPPR;
            set
            {
                _CollectionAPPR = value;
                NotifyOfPropertyChange(() => CollectionAPPR);
            }
        }
        public override bool SetIView(IView view)
        {
            _view = view as ISellingStatusApprDetailsView;
            return base.SetIView(view);
        }
        public ICommand RadioCommand => new RelayCommand<CheckBox>(RadioCommandCenter);
        public ICommand CheckboxCommand => new RelayCommand<CheckBox>(CheckboxCommandCenter);

        private void RadioCommandCenter(CheckBox rb)
        {
            switch (rb.Tag)
            {
                case "Credit":
                    APPROVAL = saleService.GetMiddleCardAppr(CHECK == "A" ? null : CHECK, setAccount.REGI_SEQ == "전체" ? null : setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.CollectionAPPR = new ObservableCollection<SELLING_APPR>(APPROVAL);
                    break;

                case "Cash":
                    APPROVAL = saleService.GetMiddleCashAppr(CHECK, setAccount.REGI_SEQ == "전체" ? null : setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.CollectionAPPR = new ObservableCollection<SELLING_APPR>(APPROVAL);
                    break;
            }
        }

        public ICommand HeaderCommand => new RelayCommand<GridViewColumnHeader>(HeaderCommandCtrl);

        private async void HeaderCommandCtrl(GridViewColumnHeader obj)
        {
            IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = null;

            switch (obj.Tag)
            {
                case "posNo":
                    if (headerSelect.Item1 == "posNo" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => ChangeToNumber(x.POS_NO)).ToList();
                        CollectionAPPR =new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("posNo", true);

                        break;
                    }
                    var xx = CollectionAPPR.OrderByDescending(x => ChangeToNumber(x.POS_NO)).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("posNo", false);
                    break;
                case "billNo":
                    if (headerSelect.Item1 == "billNo" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => ChangeToNumber(x.BILL_NO)).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("billNo", true);

                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => ChangeToNumber(x.BILL_NO)).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("billNo", false);
                    break;
                case "eqmType":
                    if (headerSelect.Item1 == "eqmType" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => x.EQM_TYPE).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("eqmType", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => x.EQM_TYPE).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("eqmType", false);
                    break;
                case "apprIDTNO":
                    if (headerSelect.Item1 == "apprIDTNO" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => x.APPR_IDT_NO).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("apprIDTNO", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => x.APPR_IDT_NO).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("apprIDTNO", false);
                    break;
                case "apprIDTNAME":
                    if (headerSelect.Item1 == "apprIDTNAME" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => x.APPR_IDT_NAME).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("apprIDTNAME", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => x.APPR_IDT_NAME).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("apprIDTNAME", false);
                    break;
                case "apprAMT":
                    if (headerSelect.Item1 == "apprAMT" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => x.APPR_AMT).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("apprAMT", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => x.APPR_AMT).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("apprAMT", false);
                    break;
                case "apprNo":
                    if (headerSelect.Item1 == "apprNo" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => ChangeToNumber(x.APPR_NO)).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("apprNo", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => ChangeToNumber(x.APPR_NO)).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("apprNo", false);
                    break;
                case "apprPROCNAME":
                    if (headerSelect.Item1 == "apprPROCNAME" && headerSelect.Item2 == false)
                    {
                        var x = CollectionAPPR.OrderBy(x => x.APPR_PROC_NAME).ToList();
                        CollectionAPPR = new ObservableCollection<SELLING_APPR>(x);
                        headerSelect = new Tuple<string, bool>("apprPROCNAME", true);
                        break;
                    }
                    xx = CollectionAPPR.OrderByDescending(x => x.APPR_PROC_NAME).ToList();
                    CollectionAPPR = new ObservableCollection<SELLING_APPR>(xx);
                    headerSelect = new Tuple<string, bool>("apprPROCNAME", false);
                    break;
                case "totAMT":
                  
                    break;
                case "occRATE":
                    //if (headerSelect.Item1 == "occRATE" && headerSelect.Item2 == false)
                    //{
                    //    MainList = MainList.OrderBy(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) =>
                    //    {
                    //        item.NO = (index + 1).ToString();
                    //        return item;
                    //    }).ToList();
                    //    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    //    headerSelect = new Tuple<string, bool>("occRATE", true);
                    //    break;
                    //}
                    //MainList = MainList.OrderByDescending(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    //IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    //headerSelect = new Tuple<string, bool>("occRATE", false);
                    break;
            }

        }

        private void CheckboxCommandCenter(CheckBox cb)
        {
            switch (cb.Tag)
            {
                case "A":
                    GiftListType(RADIOH, null);
                    break;

                case "Y":
                    GiftListType(RADIOH, "Y");
                    break;

                case "N":
                    GiftListType(RADIOH, "N");
                    break;

                default: break;
            }
        }
        private void GiftListType(string type, string? saleYN)
        {
            if (type == "Credit")
            {
                APPROVAL = saleService.GetMiddleCardAppr(saleYN, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                this.CollectionAPPR = new ObservableCollection<SELLING_APPR>(APPROVAL);
            }
            else
            {
                APPROVAL = saleService.GetMiddleCashAppr(saleYN, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG, */setAccount.SALE_DATE).Result.Item1;
                this.CollectionAPPR = new ObservableCollection<SELLING_APPR>(APPROVAL);
            }
        }
        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            setAccount = (SETT_POSACCOUNT)datas[0];
        }

        private void viewLoad(object sender, EventArgs e)
        {
            GiftListType("Credit", null);
        }
    }
}
