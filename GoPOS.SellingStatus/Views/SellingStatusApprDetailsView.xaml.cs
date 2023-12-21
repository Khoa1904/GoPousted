using GoPOS.Common.Views;
using GoPOS.Sales.Interface.View;
using GoPOS.SellingStatus.Interface;
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
    /// SalesMiddleExcClcView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SellingStatusApprDetailsView : UCViewBase, ISellingStatusApprDetailsView
    {
        public CheckBox iRadio1 => this.Radio1;

        public CheckBox iRadio2 => this.Radio2;
        public CheckBox iCheck1 => this.check1;
        public CheckBox iCheck2 => this.check2;
        public CheckBox iCheck3 => this.check3;
        public SellingStatusApprDetailsView()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }
        private void Radiotic_Checkbox2(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { Radio1, Radio2 })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }
            private void Radiotic_Checkbox3(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { check1, check2, check3 })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }

        private void NotNull_Checkbox3(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (check1.IsChecked == false && check2.IsChecked == false && check3.IsChecked==false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }
        private void NotNull_Checkbox2(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (Radio1.IsChecked == false && Radio2.IsChecked == false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            Radio1.IsChecked = true;
            check1.IsChecked = true;

        }
    }
}

