using GoPOS.Common.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoPOS.Common.Views.Controls
{
    /// <summary>
    /// Interaction logic for InputKeyPad.xaml
    /// </summary>
    public partial class NumericKeyPadView : UserControl
    {
        public event EventHandler? OnBack;
        public event EventHandler? OnClear;

        public NumericKeyPadView()
        {
            InitializeComponent();
        }

        protected void KeyClicked(object sender, RoutedEventArgs e)
        {
            this.KeyClicked(sender, e, OnClear, OnBack, null);
        }
    }    
}
