using GoPOS.Common.Views;
using GoShared.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{
    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PermissionMngView : UCViewBase
    {
        public PermissionMngView() : base()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
        }
        public TextBox txtEMP_PWD => this.EMP_PWD;

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtEMP_PWD.Focus();
        }
    }
}
