using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using Grpc.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.Common.Views
{
    public partial class CalendarView : UCViewBase, ICalendar
    {
        public CalendarView()
        {
            InitializeComponent();
        }
        //private void abcd (object sender, RoutedEventArgs e)
        //{
        //    //     this.NAME
        //    LastSelected = null;
        //    var ooo = DataLocals.PosStatus.SALE_DATE.Substring(6);
        //    var btns = CalendarGrid.FindVisualChildren<Button>().ToList();
        //    Button  X = (Button)btns.FirstOrDefault(p => p.Content.ToString() == ooo);
        //    LastSelected = X;
        //}
        public void RenderCalendarDays(ObservableCollection<int> CalendarDays)
        {
            var allDays = CalendarDays.ToArray();
            int Coords = 0;
            Button btn = null;
            foreach (var value in allDays)
            {
                var day = CalendarDays[Coords];
                // Set Colspan , RowSpan
                int col = Coords % 7;
                int row = ((Coords / 7) * 2) + 1;

                var stackPanel = CalendarGrid.Children.OfType<StackPanel>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

                if (stackPanel != null)
                {
                    btn = stackPanel.Children.OfType<Button>().FirstOrDefault();
                    if (btn == null) { continue; }
                }

                if ((Grid.GetRow(stackPanel) == 1 && day > 8) || (Grid.GetRow(stackPanel) == 2 && day > 8) || (Grid.GetRow(stackPanel) == 11 && day < 14) || (Grid.GetRow(stackPanel) == 9 && day < 14))
                {
                    btn.Tag = "btnDay_" + day;
                    btn.Content = day.ToString();
                    btn.Foreground = new SolidColorBrush(Colors.LightGray);
                }
                else
                {
                    btn.Tag = "btnDay_" + day;
                    btn.Content = day.ToString();
                    if (((Grid.GetColumn(stackPanel) + 1) % 7 == 1))
                    {
                        btn.Foreground = new SolidColorBrush(Color.FromRgb(250, 117, 20));
                    }
                    else if (((Grid.GetColumn(stackPanel) + 1) % 7 == 0))
                    {
                        btn.Foreground = new SolidColorBrush(Color.FromRgb(126, 136, 207));
                    }
                    else
                    {
                        btn.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
                btn.Click += Last_Click;
                Coords++;
            }
        }
        private Button LastSelected = null;
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            LastSelected = null;
            Button btn = (Button)sender;
            var btns = CalendarGrid.FindVisualChildren<Button>().ToList();
            LastSelected = btn;
            foreach (var Button in btns)
            {
                Button.Background = new SolidColorBrush(Colors.White);
            }
            if (LastSelected != null)
            {
                LastSelected.Background = new SolidColorBrush(Colors.LightCoral);
            }


        }
        private void ValidationRule(object sender, TextCompositionEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            string input = e.Text;
            string pattern = @"^\d{4}-(0[1-9]|1[0-2])$"; // only accept yyyy-MM
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(input, pattern);

            if (!isValid)
            {
                e.Handled = true;
                textBox.Text = DateTime.Now.ToString("yyyy-MM");
            }
        }
    }
}
