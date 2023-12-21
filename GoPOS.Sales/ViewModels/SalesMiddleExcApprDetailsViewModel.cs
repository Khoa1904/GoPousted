using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Sales.Interface.View;
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

namespace GoPOS.ViewModels
{
    public class SalesMiddleExcApprDetailsViewModel : BaseItemViewModel, IDialogViewModel
    {
        private ISalesGiftSaleService saleService;
        private List<SALES_APPR>? _approval = null;
        private ISaleMiddleApprDetailsPopup _view = null;
        private SETT_POSACCOUNT setAccount { get; set; }
        private TRN_GIFT gift { get; set; }
        private TRN_EGIFT egift { get; set; }
        public SalesMiddleExcApprDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISalesGiftSaleService service) 
            : base(windowManager, eventAggregator)
        {
            this.saleService = service;
            this.ViewLoaded += viewLoad;
        }
        public Dictionary<string, object> DialogResult { get; set; }
        private string _radio = "";
        private string _check = "";

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
                    return null; //_view.iCheck1.Tag.ToString();
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
        
        public List<SALES_APPR>? APPROVAL
        {
            get => _approval;
            set
            {
                _approval = value;
                NotifyOfPropertyChange(() => APPROVAL);
            }
        }
        private ObservableCollection<SALES_APPR> _CollectionAPPR;
        public ObservableCollection<SALES_APPR> CollectionAPPR
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
            _view = view as ISaleMiddleApprDetailsPopup;
            return base.SetIView(view);
        }
        public ICommand RadioCommand => new RelayCommand<CheckBox>(RadioCommandCenter);
        public ICommand CheckboxCommand => new RelayCommand<CheckBox>(CheckboxCommandCenter);

        private void RadioCommandCenter(CheckBox rb)
        {
            switch (rb.Tag)
            {
                case "Credit":
                    APPROVAL = saleService.GetMiddleCardAppr(CHECK, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.CollectionAPPR = new ObservableCollection<SALES_APPR>(APPROVAL);
                    break;

                case "Cash":
                    APPROVAL = saleService.GetMiddleCashAppr(CHECK, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.CollectionAPPR = new ObservableCollection<SALES_APPR>(APPROVAL);
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
                this.CollectionAPPR = new ObservableCollection<SALES_APPR>(APPROVAL);
            }
            else
            {
                APPROVAL = saleService.GetMiddleCashAppr(saleYN, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG, */setAccount.SALE_DATE).Result.Item1;
                this.CollectionAPPR = new ObservableCollection<SALES_APPR>(APPROVAL);
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
