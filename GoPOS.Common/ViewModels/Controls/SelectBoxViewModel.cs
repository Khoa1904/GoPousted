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
using GoPOS.Common.Service;
using GoPOS.Service;
using static GoPOS.Common.Helpers.NativeMethods;
using GoPOS.Common.Interface.Model;
using System.Diagnostics;
using GoShared.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace GoPOS.Common.ViewModels
{
    public class SelectBoxViewModel : BaseItemViewModel, IDialogViewModel, IHandle<SelectboxEvent>
    {
        private ISelectboxviewService selectboxviewServant;
        private ISelectBoxView _view;
        List<MST_INFO_POS> PosList { get; set; } = null;
        List<SETT_POSACCOUNT> SEQlist { get; set; } = null;
        private List<MST_COMM_CODE> Codelist { get; set; } = null;

        public string PosNoSelected { get; set; } = "01";

        public SelectBoxViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISelectboxviewService selectboxviewService) : base(windowManager, eventAggregator)
        {
            this._eventAggregator.SubscribeOnPublishedThread(this);
            this.ViewLoaded += SelectBoxViewModel_ViewLoaded;
            this.selectboxviewServant = selectboxviewService;
            //          Init();
        }

        private void SelectBoxViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            PosList = null;
            SEQlist = null;
            Codelist = null;
        }

        public override bool SetIView(IView view)
        {
            _view = (ISelectBoxView)view;
            return false;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCtrl);
        private void ButtonCommandCtrl(Button btn)
        {
            if (btn == null || btn.Tag == null)
            {
                this.TryCloseAsync();
            }
            switch (btn.Tag)
            {
                case "ButtonP":
                    if (CurrentPage == 1)
                        break;
                    CurrentPage--;
                    break;
                case "ButtonN":
                    if (CurrentPage == totalPage)
                        break;
                    CurrentPage++;
                    break;
                case string s when s.StartsWith("POS"):
                    PosNoSelected = btn.Tag.ToString().Substring(btn.Tag.ToString().Length - 2);

                    PosNoSelected = PosNoSelected == "00" ? "전체" : PosNoSelected;
                    _eventAggregator.PublishOnUIThreadAsync(new SelectPosEventArgs()
                    {
                        EventType = "ExtButton",
                        PosNo = PosNoSelected
                    });
                    this.TryCloseAsync();
                    break;
                case string s when s.StartsWith("SEQ"):
                    PosNoSelected = btn.Tag.ToString().Substring(btn.Tag.ToString().Length - 2);

                    PosNoSelected = PosNoSelected == "00" ? "전체" : PosNoSelected;
                    _eventAggregator.PublishOnUIThreadAsync(new SelectSEQEventArgs()
                    {
                        EventType = "ExtButton",
                        SEQNo = PosNoSelected
                    });
                    this.TryCloseAsync();
                    break;
                case string s when s.StartsWith("Common"):
                    var tb = btn.Content as TextBlock;
                    var ComCode = btn.Tag.ToString().Substring(btn.Tag.ToString().Length - 7);
                    ComCode = ComCode == "00" ? "전체" : ComCode;
                    _eventAggregator.PublishOnUIThreadAsync(new SelectCommonCodeArgs()
                    {
                        EventType = "ExtButton",
                        CommonCode = ComCode,
                        ComcodeName = tb.Text
                    }); ; ;
                    this.TryCloseAsync();
                    break;
                case string ss when ss.IsNullOrEmpty():
                    this.TryCloseAsync();
                    break;
                default:
                    break;
            }
        }

        public Dictionary<string, object> DialogResult
        {
            get; set;
        }
        private int totalPage = 0;
        private int _currentPage = -1;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                if (_currentPage < 0)
                {
                    _currentPage = 0;
                }
                if (_currentPage > totalPage - 1)
                {
                    _currentPage = totalPage - 1;
                }
                if (!PosList.IsNullOrEmpty())
                {
                    var RegList = PosList.Page(6, _currentPage).ToArray();
                    _view.POSRender(RegList, value);
                }
                else if (!SEQlist.IsNullOrEmpty())
                {
                    var RegList = SEQlist.Page(6, _currentPage).ToArray();
                    _view.SEQRender(RegList, value);
                }
                else if (!Codelist.IsNullOrEmpty())
                {
                    var CodeList = Codelist.Page(6, _currentPage).ToArray();
                    _view.COMMONRender(CodeList, value);
                }

                NotifyOfPropertyChange(() => CurrentPage);
            }
        }

        private void SetTicket()
        {

        }
        public void RenderProtocol(string type)
        {
            if (type == "POS")
            {
                PosList = selectboxviewServant.GetCommonPos().Result;
                if (PosList != null) { totalPage = PosList.Count / 6 + (PosList.Count % 10 == 0 ? 0 : 1); }
                CurrentPage = 0;
            }
            else if (type == "COMCODE")
            {
                Codelist = selectboxviewServant.GetCommonCode("038").Result;
                if (Codelist != null) { totalPage = Codelist.Count / 6 + (Codelist.Count % 10 == 0 ? 0 : 1); }
                CurrentPage = 0;
            }
            else if (type == "SEQ")
            {
                SEQlist = selectboxviewServant.GetSEQ().Result;
                if (SEQlist != null) { totalPage = SEQlist.Count / 6 + (SEQlist.Count % 10 == 0 ? 0 : 1); }
                CurrentPage = 0;
            }
        }

        public Task HandleAsync(SelectboxEvent message, CancellationToken cancellationToken)
        {
            RenderProtocol(message.Type);
            return Task.CompletedTask;
        }
    }
}