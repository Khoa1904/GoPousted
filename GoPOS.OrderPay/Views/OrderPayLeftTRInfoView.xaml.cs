using Microsoft.AspNetCore.Server.IIS.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayLeftTRInfoView : UserControl
    {
        public OrderPayLeftTRInfoView()
        {
            InitializeComponent();
        }

        public bool ButtonClick { get; set; } = true;
        private void DenyClick(object sender, RoutedEventArgs e)
        {
           if(!ButtonClick) return;
        }
    }
}
