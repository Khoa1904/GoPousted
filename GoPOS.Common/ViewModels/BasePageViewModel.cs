using Caliburn.Micro;
using GoShared.Helpers;
using static GoShared.Events.GoPOSEventHandler;
using System.Windows;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Input;
using System.Windows.Controls;
using GoPOS.Models.Common;
using GoShared.Contract;
using System.ComponentModel;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoShared.Events;
using GoPOS.Helpers;
using System.Windows.Markup;
using System.IO;
using System.Reflection;
using GoShared;
using System.Net.NetworkInformation;
using Microsoft.IdentityModel.Tokens;
using System.Windows.Threading;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using System.IO.Packaging;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Security;

namespace GoPOS.Common.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    ///        
    public class BasePageViewModel : Conductor<IScreen>.Collection.AllActive, IPageViewModel,
        INotifyPropertyChanged, IHandle<PageItemEventArgs>
    {
        protected readonly IWindowManager _windowManager;
        protected readonly IEventAggregator _eventAggregator;
        private Dictionary<string, Stack<IScreen>> _activeStacks;

        [DllImport("psapi")]
        static extern int EmptyWorkingSet(IntPtr handle);
        [DllImport("kernel32")]
        static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

        public BasePageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _activeStacks = new Dictionary<string, Stack<IScreen>>();
            _eventAggregator.SubscribeOnUIThread(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            var sview = view as IView;
            if (sview != null)
            {
                SetIView(sview);
            }
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
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
            ResetUsingMemory();
        }

        protected IScreen GetScreen(string areaName)
        {
            Stack<IScreen> st;
            if (!_activeStacks.ContainsKey(areaName))
            {
                st = new Stack<IScreen>();
                _activeStacks.Add(areaName, st);
            }
            else
            {
                st = _activeStacks[areaName] ?? new Stack<IScreen>();
            }

            IScreen scr = null;
            if (st.Count != 0)
            {
                st.Pop();
                scr = st.IsNullOrEmpty() ? null : st.Pop();
            }

            _activeStacks[areaName] = st;
            return scr;
        }

        protected void PutScreen(string areaName, IScreen scr)
        {
            Stack<IScreen> st;
            if (!_activeStacks.ContainsKey(areaName))
            {
                st = new Stack<IScreen>();
                _activeStacks.Add(areaName, st);
            }
            else
            {
                st = _activeStacks[areaName] ?? new Stack<IScreen>();
            }

            if (scr == null)
            {
                st.Clear();
            }
            else
            {
                st.Push(scr);
            }

            _activeStacks[areaName] = st;
        }

        private IScreen? _ActiveItem = null;
        public IScreen? ActiveItem
        {
            get
            {
                return _ActiveItem;
            }
            set
            {
                //_activeStacks[nameof(ActiveItem)] = _ActiveItem;
                PutScreen(nameof(ActiveItem), _ActiveItem);
                _ActiveItem = value;
                _ActiveItem?.ActivateAsync();

                NotifyOfPropertyChange(() => ActiveItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(PageItemEventArgs message, CancellationToken cancellationToken)
        {
            if (message == null)
                return Task.CompletedTask;
            if (!this.GetType().Name.Equals(message.ParentType)) return Task.CompletedTask;

            switch (message.EventType)
            {
                case PageItemEventTypes.DataEvent:
                    if (message.ItemName == "EmployeeChanged")
                    {
                        NotifyOfPropertyChange(nameof(EmployeeName));
                    }
                    break;
                case PageItemEventTypes.ActiveItem:
                    var viewModelType = (Type)message.EventData[0];

                    // Check Property name by ItemName
                    var pi = this.GetType().GetProperty(message.ItemName, System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance);
                    pi.SetValue(this, (IScreen)IoC.GetInstance(viewModelType, null));
                    break;
                case PageItemEventTypes.ClosePage:
                    ClosePageReturn();
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }

        private void ClosePageReturn()
        {
            this.TryCloseAsync(true);
            _eventAggregator.PublishOnUIThreadAsync(new BasePageClosedEventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="formItemType"></param>
        public void ActiveForm(string formName, Type vmType)
        {
            // Check Property name by ItemName
            var pi = this.GetType().GetProperty(formName, System.Reflection.BindingFlags.Public | BindingFlags.Instance);
            var scr = (Screen)IoC.GetInstance(vmType, null);
            scr.Parent = this;
            pi?.SetValue(this, scr);

            var pin = scr.GetType().GetProperty("AreaName", BindingFlags.Instance | BindingFlags.Public);
            pin?.SetValue(scr, formName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="scr"></param>
        public void ActiveForm(string formName, IScreen scr, params bool[] invokeLoaded)
        {
            // Check Property name by ItemName
            var pi = this.GetType().GetProperty(formName, System.Reflection.BindingFlags.Public | BindingFlags.Instance);
            ((Screen)scr).Parent = this;
            pi?.SetValue(this, scr);

            var pin = scr.GetType().GetProperty("AreaName", BindingFlags.Instance | BindingFlags.Public);
            pin?.SetValue(scr, formName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="formItemTypeName"></param>
        public void ActiveForm(string formName, string formItemTypeName)
        {
            ActiveForm(formName, formItemTypeName, null, null);
        }

        public void ActiveForm(string formName, string formItemTypeName, params object[] csParams)
        {
            ActiveForm(formName, formItemTypeName, null, csParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="formItemTypeName"></param>
        /// <param name="areaActivatedEventHandler"></param>
        public void ActiveForm(string formName, string formItemTypeName, OPAreaActivatedEventHandler areaActivatedEventHandler, params object[] csParams)
        {
            // Check Property name by ItemName
            foreach (var ass in Extensions.GoPOSAssemblies)
            {
                var vmType = ass.GetTypes().Where(type => type.IsClass).FirstOrDefault(type => type.Name.Equals(formItemTypeName));
                if (vmType != null)
                {
                    var pi = this.GetType().GetProperty(formName, System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                    var scr = (Screen)IoC.GetInstance(vmType, null);
                    scr.Parent = this;

                    ChildActivatedEventArgs e = null;
                    if (areaActivatedEventHandler != null)
                    {
                        e = new ChildActivatedEventArgs()
                        {
                            AreaName = formName,
                            ChildType = vmType,
                            ChildViewModel = scr,
                            ChildVMType = formItemTypeName,
                            Cancelled = false,
                            CSData = csParams
                        };

                        areaActivatedEventHandler(this, e);

                        if (e.Cancelled)
                        {
                            return;
                        }
                    }

                    pi?.SetValue(this, scr);

                    //if ((e != null && e.CSData != null))
                    {
                        var setDataMethod = scr.GetType().GetMethod("SetData", BindingFlags.Instance | BindingFlags.Public);
                        if (e == null || e.CSData == null)
                        {
                            List<object> pams = new List<object>();
                            pams.AddRange(csParams);
                            setDataMethod?.Invoke(scr, new object[] { pams.ToArray() });
                        }
                        else
                        {
                            setDataMethod?.Invoke(scr, new object[] { e.CSData });
                        }
                    }

                    var pin = scr.GetType().GetProperty("AreaName", BindingFlags.Instance | BindingFlags.Public);
                    pin?.SetValue(scr, formName);
                    break;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public bool ActiveFormIs(string formName, Type viewModelType)
        {
            var pi = this.GetType().GetProperty(formName, System.Reflection.BindingFlags.Public | BindingFlags.Instance);
            var scr = pi.GetValue(this, null);
            return scr.GetType() == viewModelType;
        }

        /// <summary>
        /// Close child items
        /// </summary>
        /// <param name="childItems"></param>
        public void ClosePage(string[] childItems)
        {
            foreach (var areaName in childItems)
            {
                var pi = this.GetType().GetProperty(areaName, System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                var scr = (Screen)pi?.GetValue(this, null);
                scr?.TryCloseAsync(false);
                //pi?.SetValue(this, null);
            }

            _activeStacks.Clear();
            ClosePageReturn();
        }

        public virtual bool SetIView(IView view)
        {
            return true;
        }

        public void SetData(object data)
        {
        }

        public virtual void ChildActivated(string areaName, bool activated, object data)
        {

        }

        protected void ResetUsingMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            //SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        #region Public propertis

        public string CommunicationStatus
        {
            get { return "정상"; }
        }

        public string SalesDateString
        {
            get
            {
                string sd = DataLocals.PosStatus.SALE_DATE;
                var sdd = DateTime.ParseExact(sd, "yyyyMMdd", CultureInfo.CurrentCulture);
                return sdd.ToString("yyyy-MM-dd");
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

                return string.Format("{0:yyyy}.{0:MM}.{0:dd} [{1}]", DateTime.Today, dn);
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

        #endregion

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
    }

    public delegate void OPAreaActivatedEventHandler(object sender, ChildActivatedEventArgs e);

    public class ChildActivatedEventArgs : EventArgs
    {
        public Type ChildType { get; set; }
        public object ChildViewModel { get; set; }
        public string ChildVMType { get; set; }
        public string AreaName { get; set; }
        public bool Cancelled { get; set; }

        /// <summary>
        /// Optional to provide data to VM before loading,
        /// popup or Active
        /// </summary>
        public object CSData { get; set; }
    }
}
