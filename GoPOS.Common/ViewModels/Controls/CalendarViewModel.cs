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


/*
 공통 > 달력 컨트롤

 */

namespace GoPOS.Common.ViewModels
{

    public class CalendarViewModel : BaseItemViewModel, IDialogViewModel
    {
        private IEventAggregator _eventAggregator;
        public bool Q = false;
        private ICalendar _view;
        public string SelectedDateType { get; set; }
        private DateTime _compare { get; set; }
        string oType = "";
        public DateTime Compare
        {
            get => _compare;
            set
            {
                _compare = value;
                NotifyOfPropertyChange(() => Compare);
            }
        }

        public CalendarViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            this.ViewInitialized += CalendarViewModel_ViewInitialized;

            this.ViewLoaded += CalendarViewModel_ViewLoaded;
            Init();
        }
        private DateTime _txtSaleDate;
        public DateTime txtSaleDate
        {
            get => _txtSaleDate;
            set
            {
                if (_txtSaleDate == value) return;
                _txtSaleDate = value;
                NotifyOfPropertyChange(() => txtSaleDate);
            }
        }
        private async void Init()
        {
            await Task.Delay(100);
        }

        private void CalendarViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            CalendarDays = new ObservableCollection<int>();
            GenerateCalendarDays();
            _view.RenderCalendarDays(CalendarDays);
        }

        private void CalendarViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            CalendarDays = new ObservableCollection<int>();
            GenerateCalendarDays();
            _view.RenderCalendarDays(CalendarDays);

            txtSaleDate = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);

        }

        public void MsgSet(string msg, bool type)
        {
            ////Q 질문식 //N 확인
            if (type)
                IoC.Get<MessageView>().btnCancel.Visibility = Visibility.Visible;
            else
                IoC.Get<MessageView>().btnCancel.Visibility = Visibility.Collapsed;

            IoC.Get<MessageView>().txtMsg.Text = msg;
        }

        public bool IsConfirm()
        {
            return Q;
        }

        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            if (sValue == "Y")
            {
                Q = true;
            }
            else
            {
                Q = false;
            }

            var name = (string)btn.Name;


            _eventAggregator.PublishOnUIThreadAsync(new CalendarEventArgs()
            {
                EventType = name,
                Date = DateTime.Now
            });

            this.TryCloseAsync();

        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    NotifyOfPropertyChange(nameof(CurrentDate));
                }
            }
        }
        private DateTime? _selectdate;
        public DateTime? SelectDate => _selectdate;
        public ObservableCollection<int> CalendarDays { get; set; }

        private void GenerateCalendarDays()
        {
            CalendarDays.Clear();
            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            DateTime startDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            int startDayOffset = (int)startDate.DayOfWeek;
            if (startDate == DateTime.MinValue) startDate = DateTime.Now;
            // Rem-day from last month
            DateTime previousMonth = startDate.AddMonths(-1);
            int previousMonthDays = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            for (int i = startDayOffset - 1; i >= 0; i--)
            {
                CalendarDays.Add(previousMonthDays - i);
            }

            // Days of current month
            for (int i = 1; i <= daysInMonth; i++)
            {
                CalendarDays.Add(i);
            }

            // Add days from the next month
            int remainingDays = 42 - (daysInMonth + startDayOffset);
            DateTime nextMonth = startDate.AddMonths(1);
            for (int i = 1; i <= remainingDays; i++)
            {
                CalendarDays.Add(i);
            }
            NotifyOfPropertyChange(() => CalendarDays);
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCtrl);

        public Dictionary<string, object> DialogResult { get; set; }

        private void ButtonCommandCtrl(Button btn)
        {
            switch (btn.Tag)
            {
                case "Previous":
                    CurrentDate = CurrentDate.AddMonths(-1);
                    GenerateCalendarDays();
                    _view.RenderCalendarDays(CalendarDays);
                    break;
                case "Next":
                    if (CurrentDate.AddHours(23) >= DateTime.Now)
                    {
                        return;
                    }
                    else
                    {
                        CurrentDate = CurrentDate.AddMonths(1);
                        GenerateCalendarDays();
                        _view.RenderCalendarDays(CalendarDays);
                    }
                    break;
                case string s when s.StartsWith("btnDay_"):
                    _selectdate = new DateTime(CurrentDate.Year, CurrentDate.Month, Convert.ToInt32(btn.Tag.ToString()[7..^0]));
                    if(oType != null && oType == "DateFrom")
                    {
                        if (_selectdate > Compare)
                        {
                            _selectdate = Compare;
                        }
                    }
                    if (oType != null && oType == "DateTo")
                    {
                        if(_selectdate < Compare)
                        {
                            _selectdate = Compare;
                        }
                    }
                        _eventAggregator.PublishOnUIThreadAsync(new CalendarEventArgs()
                    {
                        EventType = "ExtButton",
                        Date = _selectdate.GetValueOrDefault()
                    });
                    this.TryCloseAsync();
                    break;
                default:
                    break;
            }
        }

        public override bool SetIView(IView view)
        {
            _view = (ICalendar)view;
            return false;
        }

        private void PreviousButton()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            GenerateCalendarDays();
        }

        private void NextButton()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            GenerateCalendarDays();
        }

        public override void SetData(object data)
        {
            object[] ds = data as object[];
            CurrentDate = (DateTime)ds[0];
            if (ds.Count() > 2)
            {
                Compare = (DateTime)ds[1];
                oType = (string)ds[2];
            }
            base.SetData(data);
        }

    }
}