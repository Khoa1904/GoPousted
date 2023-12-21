using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoPOS.Services;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPayMemberRegistView : UCViewBase, IOrderPayMemberRegistView
    {
        public CheckBox iCheck1 => this.check1;

        public CheckBox iCheck2 => this.check2;

        public CheckBox iLuna => this.luna;

        public CheckBox iSolar => this.solar;

        public OrderPayMemberRegistView()
        {
            InitializeComponent();
            Loaded += onLoad;

        }
        private void Radiotic_Checkbox(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { check1, check2 })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }
        private void onLoad (object sender, RoutedEventArgs e)
        {
            this.TxtTelNo.Text = "";
            this.txtCardNo.Text = "";
            this.txtMbrName.Text = "";
            this.txtDOB.Text = "";
            this.check1.IsChecked = false;
            this.check2.IsChecked = false;
            this.solar.IsChecked = false;
            this.luna.IsChecked = false;
        }
        private void NotNull_Checkbox(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (check1.IsChecked == false && check2.IsChecked == false )
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }

        private void Luna_Solar(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { luna, solar })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }

        private void NotNull_LunaSolar(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (luna.IsChecked == false && solar.IsChecked == false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }

        public void SearchMember(string Telno, string Cardno, string name)
        {
            throw new NotImplementedException();
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ButtonPost = GetPointOfButton(btn);
        }
        private Point GetPointOfButton(UIElement element)
        {
            Point relativePoint = new Point();
            if (element is Button)
            {
                Button btn = element as Button;
                relativePoint = btn.TranslatePoint(new Point(0, 0), this);
                Point screenPoint = GetParentPoint();
                relativePoint.X += screenPoint.X + btn.Width;
                relativePoint.Y += screenPoint.Y;

                return relativePoint;
            }
            return relativePoint;
        }
        public Point ButtonPost { get; set; }
        private Point GetParentPoint()
        {
            Point ParentPoint = this.TranslatePoint(new Point(0, 0), (UIElement)this.Parent);
            Point screenPoint = this.PointToScreen(ParentPoint);
            return screenPoint;
        }
    }
}
