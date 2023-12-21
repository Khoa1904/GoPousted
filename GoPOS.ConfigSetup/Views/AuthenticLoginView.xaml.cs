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
using GoPOS.ConfigSetup.Interface.View;

namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for AuthenticLoginView.xaml
    /// </summary>
    public partial class AuthenticLoginView : UCViewBase, IAuthenticLoginView
    {
        public AuthenticLoginView()
        {
            InitializeComponent();
        }

        public TextBox UserId => userId;
        public PasswordBox Password => passWord;

        public Button Confirm => confirm;
    }
}