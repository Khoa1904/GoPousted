using AutoMapper.Mappers;
using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Custom.SellingStatus;
using GoPOS.Sales.Interface.View;
using GoPOS.Services;
using GoPOS.Views;
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
using static GoPOS.Function;

namespace GoPOS.ViewModels
{
    public class SellingStatusGiftCertDetailsViewModel : BaseItemViewModel, IDialogViewModel
    {
        private ISellingStatusService saleService;
        private List<SALES_GIFT_SALE2> _GiftCard ;
        private ISaleMiddleGftTkPopup _view = null;
        private SETT_POSACCOUNT setAccount { get; set; }
        private TRN_GIFT gift { get; set; }
        private TRN_EGIFT egift { get; set; }
        private Tuple<string, bool> headerSelect = new Tuple<string, bool>("", false);

        public SellingStatusGiftCertDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISellingStatusService service) 
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
                    GiftCard = saleService.GetMiddleGiftCard1(CHECK, setAccount.REGI_SEQ == "전체" ? null : setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard);
                    break;

                case "Former":
                    GiftCard = saleService.GetMiddleGiftCard2(CHECK, setAccount.REGI_SEQ == "전체" ? null : setAccount.REGI_SEQ, /*setAccount.CLOSE_FLAG,*/ setAccount.SALE_DATE).Result.Item1;
                    this.Collection = new ObservableCollection<SALES_GIFT_SALE2>(GiftCard);
                    break;
            }
        }
        public ICommand HeaderCommand => new RelayCommand<GridViewColumnHeader>(HeaderCommandCtrl);

        private async void HeaderCommandCtrl(GridViewColumnHeader obj)
        {

            switch (obj.Tag)
            {
                case "saleType":
                    if (headerSelect.Item1 == "saleType" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => ChangeToNumber(x.SALE_TYPE)).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("saleType", true);

                        break;
                    }
                    var xx = Collection.OrderByDescending(x => ChangeToNumber(x.SALE_TYPE)).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("saleType", false);
                    break;
                case "pos":
                    if (headerSelect.Item1 == "pos" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => ChangeToNumber(x.POS)).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("pos", true);

                        break;
                    }
                    xx = Collection.OrderByDescending(x => ChangeToNumber(x.POS)).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("pos", false);
                    break;
                case "billNo":
                    if (headerSelect.Item1 == "billNo" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.BILL_NO).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("billNo", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.BILL_NO).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("billNo", false);
                    break;
                case "tkFKTName":
                    if (headerSelect.Item1 == "tkFKTName" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.TK_GFT_NAME).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("tkFKTName", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.TK_GFT_NAME).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("tkFKTName", false);
                    break;
                case "tkGFTUamt":
                    if (headerSelect.Item1 == "tkGFTUamt" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.TK_GFT_UAMT).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("tkGFTUamt", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.TK_GFT_UAMT).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("tkGFTUamt", false);
                    break;
                case "tkGFTcnt":
                    if (headerSelect.Item1 == "tkGFTcnt" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.TK_GFT_CNT).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("tkGFTcnt", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.TK_GFT_CNT).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("tkGFTcnt", false);
                    break;
                case "totalTKGFTUAMT":
                    if (headerSelect.Item1 == "totalTKGFTUAMT" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.TOTAL_TK_GFT_UAMT).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("totalTKGFTUAMT", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.TOTAL_TK_GFT_UAMT).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("totalTKGFTUAMT", false);
                    break;
                case "remark":
                    if (headerSelect.Item1 == "remark" && headerSelect.Item2 == false)
                    {
                        var x = Collection.OrderBy(x => x.REMARK).ToList();
                        Collection = new ObservableCollection<SALES_GIFT_SALE2>(x);
                        headerSelect = new Tuple<string, bool>("remark", true);
                        break;
                    }
                    xx = Collection.OrderByDescending(x => x.REMARK).ToList();
                    Collection = new ObservableCollection<SALES_GIFT_SALE2>(xx);
                    headerSelect = new Tuple<string, bool>("remark", false);
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
