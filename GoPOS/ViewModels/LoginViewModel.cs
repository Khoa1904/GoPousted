using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Views;
using GoPOS.Services;
using GoPOS.Common.ViewModels;
using GoPOS.Interface;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using GoPOS.Helpers;
using System.Security;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using GoShared.Events;
using GoPOS.Common.Interface.Model;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using GoPOS.Service.Service.MST;
using GoPOS.Service.Common;
using AutoMapper;
using System;
using GoPOS.Properties;
using GoPOS.Common;
using System.Threading;
using System.Windows;
using Google.Protobuf.WellKnownTypes;
using System.Dynamic;
using System.Linq;
using GoShared.Helpers;

namespace GoPOS.ViewModels
{
    public class LoginViewModel : BaseItemViewModel, IHandle<KeyboardEventData>, IHandle<LoggedInUserChange>
    {
        //**-----------------------------------------------------------

        #region Member
        private IInfoEmpService _empMService;
        private ILoginView _view;
        private string eMP_NO;
        private bool saveEmpNo;

        #endregion

        //**-----------------------------------------------------------

        #region Property

        public InputKeyPadViewModel? InputKeyPad { get; set; }

        public ICommand LoginCommand => new RelayCommand<Button>(LoginClicked);
        public ICommand CloseProgram => new RelayCommand<Button>(ClozeProgram);

        public string EMP_NO
        {
            get => eMP_NO; set
            {
                eMP_NO = value;
                NotifyOfPropertyChange(nameof(EMP_NO));
            }
        }
        public string EMP_PWD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveEmpNo
        {
            get => saveEmpNo; set
            {
                saveEmpNo = value;
                NotifyOfPropertyChange(() => SaveEmpNo);
            }
        }

        #endregion


        //**-----------------------------------------------------------

        #region Constractor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="empMService"></param>
        public LoginViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IInfoEmpService empMService) :
            base(windowManager, eventAggregator)
        {
            this._empMService = empMService;
            this.InputKeyPad = IoC.Get<InputKeyPadViewModel>();
            this.ViewLoaded += LoginViewModel_ViewLoaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Settings.Default.SaveEmpNo))
            {
                this.EMP_NO = Settings.Default.SaveEmpNo;
                this.SaveEmpNo = true;
                _view.PasswordFocused = true;
            }
            else
            {
                this.SaveEmpNo = false;
            }

            if (DataLocals.AppConfig.PosOption.DualMonitorUseYN != "0")
            {
                bool hasSecondScr = false;
                var allScreens = System.Windows.Forms.Screen.AllScreens.ToList();
                hasSecondScr = System.Windows.Forms.Screen.AllScreens.Length > 1;
                var position = allScreens.FirstOrDefault(t => t.Primary == false);
                if (position == null) position = allScreens.FirstOrDefault();

                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.Manual;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.WindowStyle = WindowStyle.None;

                settings.Width = SystemHelper.MainWidth;
                settings.Height = SystemHelper.MainHeight;
                settings.WindowState = WindowState.Normal;
                settings.ShowInTaskbar = true;
                
                if (position != null)
                {
                    var direction = 1;
                    if (position.Bounds.X < 0) direction = -1;
                    settings.Top = (position?.Bounds.Height - SystemHelper.MainHeight) / 2;
                    settings.Left = hasSecondScr ? position?.Bounds.Left + (position?.Bounds.Width - SystemHelper.MainWidth) / 2 : position.Bounds.Width / 2;
                }

                //_windowManager.ShowWindowAsync(IoC.Get<DualScreenMainViewModel>(), null, settings);
                //Dual 모니터 셋팅이 되어있어도 연결된 모니터가 1개일때는 메인만 나오게 한다. 2023.11.10
                var monitorCnt = System.Windows.Forms.SystemInformation.MonitorCount;
                if (monitorCnt > 1)
                {
                    _windowManager.ShowWindowAsync(IoC.Get<DualScreenMainViewModel>(), null, settings);
                }
            }
        }


        private void TestDownloadMaster()
        {
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer, 
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    DataLocals.TokenInfo.LICENSE_ID,
                    DataLocals.TokenInfo.LICENSE_KEY);
            _apiRequest.LoginAsync(false).ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    if (t.Result.IsLogined)
                    {
                        var MSTCOMMCODE = new BaseDataService<MST_COMM_CODE>(_apiRequest);
                        MSTCOMMCODE.SynchronizedData(null);


                        //var MSTCOMMCODESHOP = new BaseDataService<MST_COMM_CODE_SHOP>(_apiRequest);
                        //MSTCOMMCODESHOP.SynchronizedData();

                        //var xxx = new BaseDataService<xxx>(_apiRequest);
                        //xxx.SynchronizedData();
                    }
                }
            });

        }

        public override bool SetIView(IView view)
        {
            _view = view as ILoginView;
            return base.SetIView(view);
        }
        #endregion

        //**-----------------------------------------------------------

        #region Public Method
        public async void LoginClicked(Button button)
        {
            string empNo = _view.txtEMP_NO.Text;
            string empPwd = _view.txtEMP_PWD.Text;
            bool ret = this._empMService.CheckExist(t => t.EMP_NO == empNo && t.EMP_PWD == empPwd);
            if (ret)
            {
                MST_INFO_EMP data = this._empMService.TryGet(t => t.EMP_NO == empNo);

                if (!SaveEmpNo)
                {
                    Settings.Default.SaveEmpNo = string.Empty;
                }
                else
                {
                    Settings.Default.SaveEmpNo = empNo;
                }

                Settings.Default.Save();

                // TODO : 아이디 저장 로직 구현 (Registry Key 값 저장 예정)
                DataLocals.UserLogIn(data);
                await this.TryCloseAsync(false);

                ActivatePageItemYN(typeof(MainPageViewModel), "ActiveItem", typeof(LoggedInViewModel));
            }
            else
            {
                DialogHelper.MessageBox("입력하신 사원번호, 비밀번호로 사원 정보를 조회할 수 없습니다.");
            }
        }

        public void ClozeProgram(Button btn)
        {
            this.TryCloseAsync();
            Application.Current.Shutdown();
        }

        public Task HandleAsync(KeyboardEventData message, CancellationToken cancellationToken)
        {
            if (message.FocusedControl.GetType().Name == "PasswordBox")
            {
                LoginClicked(null);
            }

            return Task.CompletedTask;
        }

        public Task HandleAsync(LoggedInUserChange message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Settings.Default.SaveEmpNo))
            {
                Settings.Default.SaveEmpNo = string.Empty;
            }
            else
            {
                Settings.Default.SaveEmpNo = message.ChangedEmpNo;
            }

            Settings.Default.Save();

            return Task.CompletedTask;
        }

        #endregion

        //**-----------------------------------------------------------
    }
}
