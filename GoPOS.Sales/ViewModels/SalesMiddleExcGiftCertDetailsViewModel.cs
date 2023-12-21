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
    public class SalesMiddleExcGiftCertDetailsViewModel : BaseItemViewModel, IDialogViewModel
    {
        private ISalesGiftSaleService saleService;
        private List<SALES_GIFT_SALE2> _GiftCard ;
        private ISaleMiddleGftTkPopup _view = null;
        private SETT_POSACCOUNT setAccount { get; set; }
        private TRN_GIFT gift { get; set; }
        private TRN_EGIFT egift { get; set; }
        public SalesMiddleExcGiftCertDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISalesGiftSaleService service) 
            : base(windowManager, eventAggregator)
        {
            this.saleService = service;
            this.ViewLoaded += viewLoad;
      //      this.GiftCard = new ObservableCollection<SALES_GIFT_SALE2>();
        }
        public Dictionary<string, object> DialogResult { get; set; }
        private string _radio = "";
        private string _check = "";

        public string RADIOH
        {
            get
            {
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
            get
            {
                if ((bool)_view.iCheck1.IsChecked)
                {
                    return null;
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
        

        public List<SALES_GIFT_SALE2>? GiftCard
        {
            get => _GiftCard;
            set
            {
                _GiftCard = value;
                NotifyOfPropertyChange(() => GiftCard);
            }
        }

        private ObservableCollection<SALES_GIFT_SALE2> _Collection;
        public ObservableCollection<SALES_GIFT_SALE2> Collection
        {
            get => _Collection;
            set
            {
                _Collection = value;
                NotifyOfPropertyChange(() => Collection);
            }
        }
        public override bool SetIView(IView view)
        {
            _view = view as ISaleMiddleGftTkPopup;
            return base.SetIView(view);
        }
        public ICommand RadioCommand => new RelayCommand<RadioButton>(RadioCommandCenter);
        public ICommand CheckboxCommand => new RelayCommand<CheckBox>(CheckboxCommandCenter);

        private void RadioCommandCenter(RadioButton rb)
        {

            switch (rb.Tag)
            {
                case "Common":
                    GiftCard = saleService.GetMiddleGiftCard1(CHECK, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard);
                    break;

                case "Former":
                    GiftCard = saleService.GetMiddleGiftCard2(CHECK, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard);
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
            if (type == "Common")
            {
                GiftCard = saleService.GetMiddleGiftCard1(saleYN, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                object convert = GiftCard;
                this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard);
            }
            else
            {
                GiftCard = saleService.GetMiddleGiftCard2(saleYN, setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                object convert = GiftCard;
                this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard) 
                   ;
            }
        }
        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            setAccount = (SETT_POSACCOUNT)datas[0];
        }

        private void viewLoad(object sender, EventArgs e)
        {
            GiftListType("Common", null);
        }


    }
}
