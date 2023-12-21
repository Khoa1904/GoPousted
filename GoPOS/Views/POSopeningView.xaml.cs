using GoPOS.Common.Views;
using GoPOS.Interface;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for POSopeningView.xaml
    /// </summary>
    public partial class POSOpeningView : UCViewBase, IPOSOpeningView
    {
        public POSOpeningView()
        {
            InitializeComponent();
        }
    }
}
