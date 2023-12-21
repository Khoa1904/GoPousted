using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPayCoprtnDscntView : UCViewBase, IOrderPayCoprtnDscntView
    {
        public OrderPayCoprtnDscntView()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtApprCardNo.Focus();
            this.txtApprCardNo.Clear();
        }

        private Button lastSelectedButton = null;

        public TextBox LimitMoney => this.txtLimitMoney;

        public void BindAffiliatedCards(List<MST_INFO_JOINCARD> affDiscCards)
        {
            if (lastSelectedButton != null)
            {
                lastSelectedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                lastSelectedButton.Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                lastSelectedButton = null;
            }

            var btns = tabel.FindVisualChildren<Button>().ToList();
            for (int j = 0; j < btns.Count; j++)
            {
                var btn = btns[j];
                btn.Click += Btn_Click;
                var tb = (TextBlock)btns[j].Content;
                if (tb != null)
                {
                    if (lastSelectedButton == null)
                    {
                        lastSelectedButton = btn;
                        lastSelectedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                        lastSelectedButton.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));
                    }
                    else
                    {
                        btn.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                        btn.Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                    }

                    if (j < affDiscCards.Count)
                    {
                        var didx = Math.Min(j, affDiscCards.Count - 1);
                        btn.Tag = affDiscCards[didx];
                        tb.Text = affDiscCards[didx].JCD_NAME;
                    }
                    else
                    {
                        btn.Tag = null;
                        tb.Text = string.Empty;
                    }
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (lastSelectedButton == btn)
            {
                return;
            }

            if (lastSelectedButton != null)
            {
                lastSelectedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                lastSelectedButton.Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
            }

            lastSelectedButton = btn;
            lastSelectedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
            lastSelectedButton.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));
        }
    }
}
