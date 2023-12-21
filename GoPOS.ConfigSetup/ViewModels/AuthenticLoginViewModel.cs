using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Common.Interface.View;
using GoPOS.Service.Service;
using GoPOS.Common.Service;
using GoPOS.ConfigSetup.Interface.View;
using GoPOS.Models.Custom.API;
using GoPOS.Services;
using GoShared.Events;
using GoPOS.Views;
using GoPOS.Common.Interface.Model;
using System.ComponentModel;
using GoPOS.Models.Common;
using System.Security;
using GoPOS.Helpers;
using System.Windows.Markup;

namespace GoPOS.ViewModels
{
    public class AuthenticLoginViewModel : BaseItemViewModel, IDialogViewModel
    {
        private IAuthenticLoginService _service;

        private readonly IWindowManager windowManager;
        private readonly IPOSInitService initService;
        private readonly IPOSPrintService pOSPrintService;

        private IAuthenticLoginView _view;


        public AuthenticLoginViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IAuthenticLoginService service) : base(windowManager, eventAggregator)
        {
            eventAggregator.SubscribeOnUIThread(this);
            this.windowManager = windowManager;
            this.initService = initService;
            _service = service;
        }

        public IScreen ActiveItem { get; set; }

        private string _userId;
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                NotifyOfPropertyChange(() => UserId);
            }
        }


        private string _password;
        public string PassWord
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => PassWord);
            }
        }
        public ICommand CloseCommand => new RelayCommand<Button>(ButtonCommandCenter);

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);

        public Dictionary<string, object> DialogResult
        {
            get; set;
        }

        private async void ButtonCommandCenter(Button button)
        {
            try
            {
                if (button.Tag == null || button == null)
                    return;
                switch (button.Tag)
                {
                    case "Login":
                        CheckUser();
                        break;
                    case "Close":
                        DialogResult = new Dictionary<string, object>
                        {
                            { "RETURN_DATA", new Tuple<bool, ResponseModel>(false, null) }
                        };

                        await this.TryCloseAsync(true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public override bool SetIView(IView view)
        {
            _view = (IAuthenticLoginView)view;
            _view.Password.KeyDown += Password_KeyDown;
            return base.SetIView(view);
        }

        private async void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CheckUser();
            }
        }

        private async void CheckUser()
        { 
            try
            {
                var data = await _service.Login(UserId, PassWord.ToString());
                if (data == null || data.status == null)
                {
                    DialogHelper.MessageBox("로그인 실패하였습니다. 사용자아이디 및 비밀번호를 확인 하십시오.");
                    UserId = "";
                    PassWord = "";
                    _view.UserId.Focus();
                    return;
                }

                switch (data.status)
                {
                    case "200":
                        DialogResult = new Dictionary<string, object>
                        {
                            { "RETURN_DATA", new Tuple<bool, ResponseModel>(true, data) }
                        };
                        await this.TryCloseAsync(true);
                        break;
                    case "8990001":
                        var dialog = DialogHelper.MessageBox("설치 정보가 없습니다.");
                        if (dialog == System.Windows.MessageBoxResult.OK)
                        {
                            DialogResult = new Dictionary<string, object>
                                    {
                                        { "RETURN_DATA", new Tuple<bool, ResponseModel>(true, data) }
                                    };
                            await this.TryCloseAsync(true);
                        }
                        else
                        {
                            UserId = "";
                            PassWord = "";
                            _view.UserId.Focus();
                        }

                        break;
                    case "8250008":
                        DialogHelper.MessageBox("로그인 실패하였습니다. 사용자아이디 및 비밀번호를 확인 하십시오.");
                        UserId = "";
                        PassWord = "";
                        _view.UserId.Focus();
                        break;
                    default:
                        DialogHelper.MessageBox("로그인 실패하였습니다. 사용자아이디 및 비밀번호를 확인 하십시오.");
                        UserId = "";
                        PassWord = "";
                        _view.UserId.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("로그인 실패하였습니다. 사용자아이디 및 비밀번호를 확인 하십시오.");
                UserId = "";
                PassWord = "";
                _view.UserId.Focus();

            }

        }
    }
}
