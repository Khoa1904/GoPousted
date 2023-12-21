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

namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for PopupMemberSearchView.xaml
    /// </summary>
    public partial class PopupMemberSearchView : UCViewBase
    {
        public PopupMemberSearchView()
        {
            InitializeComponent();
            Loaded += Popup_viewload;
        }

        private void Popup_viewload(object sender, RoutedEventArgs e)
        {
            this.txtTel.Focus();
        }
    }
}
