using GoPOS.Common.Views;
using GoPOS.Interface;
using GoShared.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{
    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : UCViewBase, ILoginView
    {
        private bool passwordFocused;

        public LoginView() : base()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
        }

        public TextBox txtEMP_NO => this.EMP_NO;

        public TextBox txtEMP_PWD => this.EMP_PWD;

        public bool IsPasswordFocused()
        {
            return pPassword.IsFocused;
        }

        public bool PasswordFocused
        {
            get => passwordFocused; set
            {
                passwordFocused = value;
                if (value)
                    pPassword.Focus();
            }
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.txtEMP_NO.Text.Length >= 4)
            {
                PasswordFocused = true;
            }
            else
            {
                this.txtEMP_NO.Focus();
            }

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordFocused = true;
        }
    }
}
