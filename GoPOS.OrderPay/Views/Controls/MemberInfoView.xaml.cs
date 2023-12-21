﻿using GoPOS.Common.Views;
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

namespace GoPOS.OrderPay.Views.Controls
{
    /// <summary>
    /// Interaction logic for PaymentInfoView.xaml
    /// </summary>
    public partial class MemberInfoView : UCViewBase
    {
        public MemberInfoView()
        {
            InitializeComponent();
        }
        public bool ButtonClick { get; set; } = true;
        private void DenyClick(object sender, RoutedEventArgs e)
        {
            if (!ButtonClick) return;
        }
    }
}