using GoPOS.Common.Views;
using GoPOS.OrderPay.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPaySaleStatusView : UCViewBase, IOrderPaySaleStatusView
    {
        public CheckBox iPro1   => this.Pro_All;

        public CheckBox iPro2   => this.Pro_Done;
        public CheckBox iPro3   => this.Pro_Undone;
        public CheckBox iStats1 => this.Stats_all;
        public CheckBox iStats2 => this.Stats_done;
        public CheckBox iStats3 => this.Stats_process;
        public CheckBox iStats4 => this.Stats_undone;
        public OrderPaySaleStatusView()
        {
            InitializeComponent();
            Loaded += OnScreenLoad;
        }

        private void Radiotic_Checkbox1(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { Pro_All, Pro_Undone, Pro_Done })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }
        private void OnScreenLoad(object sender, RoutedEventArgs e)
        {
            this.Pro_All.IsChecked = true;
            this.Stats_all.IsChecked = true;
        }
        private void NotNull_Checkbox1(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (Pro_All.IsChecked == false && Pro_Done.IsChecked == false && Pro_Undone.IsChecked == false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }
        private void Radiotic_Checkbox2(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { Stats_all, Stats_process, Stats_undone, Stats_done })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }

        private void NotNull_Checkbox2(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (Stats_all.IsChecked == false && Stats_process.IsChecked == false && Stats_undone.IsChecked == false && Stats_done.IsChecked == false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }
    }
}
