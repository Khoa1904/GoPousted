using Caliburn.Micro;
using GoShared.Helpers;
using static GoShared.Events.GoPOSEventHandler;
using System.Windows;
using GoPOS.Models.Common;
using System.Windows.Controls;
using GoShared.Contract;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using System.Globalization;
using GoPOS.Helpers;
using GoShared.Events;
using System.Printing;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GoPOS.Common.ViewModels
{
    public class BaseItemViewModel : Screen, IViewModel, IHandle<PageItemEventArgs>
    {
        [DllImport("psapi")]
        static extern int EmptyWorkingSet(IntPtr handle);
        [DllImport("kernel32")]
        static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

        public event EventHandler? OnClosedCommand;

        //public IPageViewModel ParentViewModel { get; set; }
        protected IView FormView;

        #region Constractor
        public BaseItemViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _eventAggregator.SubscribeOnUIThread(this);
        }

        #endregion

        //**-------------------------------------------------------------------

        #region Member
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IWindowManager _windowManager;
        private IScreen _activeItem;

        #endregion

        //**-------------------------------------------------------------------

        #region Public propertis

        public string CommunicationStatus
        {
            get { return "정상"; }
        }

        public string SalesDateString
        {
            get
            {
                if (DataLocals.PosStatus == null)
                {
                    return string.Empty;
                }
                string sd = DataLocals.PosStatus.SALE_DATE;
                DateTime sdd;
                var res = DateTime.TryParseExact(sd, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite, out sdd);
                return res ? sdd.ToString("yyyy-MM-dd") : string.Empty;
            }
        }

        public string CurrentDateString
        {
            get
            {
                return DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        public string CurrentDateWithDayText
        {
            get
            {
                var ci = System.Threading.Thread.CurrentThread.CurrentCulture;

                string[] names = ci.DateTimeFormat.AbbreviatedDayNames;
                string dn = names[(int)ci.Calendar.GetDayOfWeek(DateTime.Today)];

                return string.Format("{0:yyyy}.{0:MM}.{0:dd} [{1}]", DateTime.Today, dn);
            }
        }

        public string CurrentDateWithDayTextLong

        {
            get
            {
                var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                string[] names = ci.DateTimeFormat.DayNames;
                string dn = names[(int)ci.Calendar.GetDayOfWeek(DateTime.Today)];

                return string.Format(@"{0:yyyy}.{0:MM}.{0:dd} {1}", DateTime.Today, dn);
            }
        }

        public string? ShopName
        {
            get
            {
                return DataLocals.ShopInfo.SHOP_NAME;
            }
        }

        public string? EmployeeName
        {
            get
            {
                return DataLocals.Employee.EMP_NAME;
            }
        }


        public string? UserFlagPosition
        {
            get
            {
                if (DataLocals.Employee.EMP_NO.StartsWith("0"))
                    return "점주";
                else if (DataLocals.Employee.EMP_NO.StartsWith("1"))
                    return "판매원";
                else if (DataLocals.Employee.EMP_NO.StartsWith("2"))
                    return "주문";
                else if (DataLocals.Employee.EMP_NO.StartsWith("3"))
                    return "배달";
                return "";
            }
        }


        public event EventHandler ViewInitialized;
        public void InvokeViewInitialized()
        {
            ViewInitialized?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ViewLoaded;
        public void InvokeViewLoaded()
        {
            ViewLoaded?.Invoke(this, EventArgs.Empty);
        }


        public event EventHandler ViewUnloaded;
        public void InvokeViewUnloaded()
        {
            ViewUnloaded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public & Overrides

        public virtual bool SetIView(IView view)
        {
            FormView = view;
            return true;
        }

        public virtual void SetData(object data)
        {

        }

        protected void DeactivateClose(bool close)
        {
            OnClosedCommand?.Invoke(this, EventArgs.Empty);
            this.DeactivateAsync(true);
            ResetUsingMemory();
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);// base.CanCloseAsync(cancellationToken);
        }

        public void ActivatePageItemYN(Type pageType, string formName, Type itemType, bool activate = true)
        {
            _eventAggregator.PublishOnUIThreadAsync(new PageItemEventArgs(pageType.Name, formName, activate ?
                PageItemEventTypes.ActiveItem : PageItemEventTypes.DeactiveItem, itemType));
        }

        public void ActivatePageItemYN(Type pageType, string formName, string viewModelName, bool activate = true)
        {
            var itemType = Type.GetType("GoPOS.ViewModels." + viewModelName + ", GoPOS");
            _eventAggregator.PublishOnUIThreadAsync(new PageItemEventArgs(pageType.Name, formName, activate ?
                PageItemEventTypes.ActiveItem : PageItemEventTypes.DeactiveItem, itemType));
        }

        public void NotifyChangePage(string viewModelName)
        {
            _eventAggregator.PublishOnUIThreadAsync(new PageItemEventArgs("ShellViewModel", "ActiveItem", PageItemEventTypes.ActiveItem, viewModelName));
        }

        public void NotifyClosePage(Type pageType)
        {
            _eventAggregator.PublishOnUIThreadAsync(new PageItemEventArgs(pageType.Name, null, PageItemEventTypes.ClosePage, null));
        }

        #endregion

        //**-------------------------------------------------------------------

        #region Protected

        public ICommand CloseCommand => new RelayCommand<Button>(ButtonClose);

        public virtual IScreen ActiveItem
        {
            get
            {
                return _activeItem;
            }
            set
            {
                _activeItem = value;
                NotifyOfPropertyChange(() => ActiveItem);
            }
        }

        public virtual void ButtonClose(Button button)
        {
            DeactivateClose(true);
        }

        protected void ResetUsingMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            //SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool ValidateSaleDate()
        {
            if (DataLocals.PosStatus == null || string.IsNullOrEmpty(DataLocals.PosStatus.SALE_DATE))
            {
                DialogHelper.MessageBox("영업일자가 설정 되지 않습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public Task HandleAsync(PageItemEventArgs message, CancellationToken cancellationToken)
        {
            switch (message.EventType)
            {
                case PageItemEventTypes.ActiveItem:
                    break;
                case PageItemEventTypes.DeactiveItem:
                    break;
                case PageItemEventTypes.ClosePage:
                    break;
                case PageItemEventTypes.DataEvent:
                    if (message.ItemName == "EmployeeChanged")
                    {
                        NotifyOfPropertyChange(nameof(EmployeeName));
                    }
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
