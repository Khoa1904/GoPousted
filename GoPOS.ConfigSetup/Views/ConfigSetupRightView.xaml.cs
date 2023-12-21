using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.ConfigSetup.Interface.View;
using GoPOS.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GoPOS.Common.Interface.Model;

namespace GoPOS.Views
{
    public partial class ConfigSetupRightView : UCViewBase, IConfigSetupRightView
    {
        public ConfigSetupRightView()
        {
            InitializeComponent();
            Unloaded += ViewUnLoaded;            
        }

        private GoButton lastClicked = null;

        public void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys)
        {
            var btns = buttonList.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("btnMenu")).ToList();
            for (int i = 0; i < btns.Count; i++)
            {
                var tb = (TextBlock)btns[i].Content;
                if (tb != null)
                {
                    tb.Text = i < funcKeys.Length ? funcKeys[i].FK_NAME : string.Empty;
                    tb.Foreground = new SolidColorBrush(Colors.Black);
                }

                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.StartPoint = new Point(0, 1);
                gradientBrush.EndPoint = new Point(0, 0);
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(222, 222, 222), 0.45));
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(235, 235, 235), 0.55));
                btns[i].Background = gradientBrush;
                btns[i].Tag = i < funcKeys.Length ? funcKeys[i] : null;
                btns[i].Click += BtnClick;

                ((GoButton)btns[i]).NormalBG = btns[i].Background;
                ((GoButton)btns[i]).NormalFG = tb.Foreground;
            }

            if (btns.Count > 0)
            {
                BtnClick(btns[0], new RoutedEventArgs());
                ((IMainMgrPageRightViewModel)ViewModel).SelectFirstExtMenu(btns[0].Tag as ORDER_FUNC_KEY);
            }
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            if (lastClicked != null)
            {
                lastClicked.Selected = false;
            }

            GoButton btn = (GoButton)sender;
            lastClicked = btn;
            lastClicked.Selected = true;
        }

        private void ViewUnLoaded(object? sender, EventArgs e)
        {
            if (lastClicked != null) lastClicked.Selected = false;
            lastClicked = null;
        }
    }
}
