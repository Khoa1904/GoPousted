using Caliburn.Micro;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoPOS.Common.Views.Controls
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DialogWindowView : Window
    {
        public DialogWindowView()
        {
            InitializeComponent();
            this.Loaded += DialogWindowView_Loaded;
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        public DialogWindowView(Type childViewType) : this()
        {
            ActiveItem.Content = (DependencyObject)IoC.GetInstance(childViewType, null);
        }

        private void DialogWindowView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public Type ViewType { get; }
    }
}
