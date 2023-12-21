using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.ViewModels;
using SixLabors.ImageSharp;
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
    /// Interaction logic for TableManagermentView.xaml
    /// </summary>
    public partial class PostPayTableManagermentView : UCViewBase, IPostPayTableManagementView
    {
        private int _currentPage { get; set; }
        private int maxPage { get; set; }
        private LinearGradientBrush bgOff = new LinearGradientBrush()
        {
            StartPoint = new System.Windows.Point(0, 1),
            GradientStops = new GradientStopCollection
    {
        new GradientStop(System.Windows.Media.Color.FromRgb(57, 71, 101), 0.48),
        new GradientStop(System.Windows.Media.Color.FromRgb(26, 42, 77), 0.52),
    }
        };
        public string table_code { get; set; }
        public PostPayTableManagermentView()
        {
            InitializeComponent();
            Loaded += onLoaded;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            this.basicBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            this.basicBtn.Background = bgOff;
        }
        public void RenderFuncButtons(List<ORDER_FUNC_KEY> FuncBtn)
        {
            var ButtonList = FunctionButton.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < (FuncBtn.Count < 10 ? FuncBtn.Count : 10); i++)
            {
                // Set button properties
                Button btn = ButtonList[i];
                btn.Tag = FuncBtn[i];
                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = FuncBtn[i].FK_NAME.Replace("<br/>", Environment.NewLine);
            }

            //turn remaining button into void
            var iCheck = ButtonList.Length - FuncBtn.Count;
            if (iCheck > 0)
            {
                for (int i = FuncBtn.Count; i < ButtonList.Length; i++)
                {
                    Button btn = ButtonList[i];
                    btn.Tag = null;
                    TextBlock tb = (TextBlock)btn.Content;
                    tb.Text = "";
                }
            }

        }

        public void Btn_Click(object sender, RoutedEventArgs e)
        {
            var bgOn = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_base_orange.png", UriKind.RelativeOrAbsolute));
            if (sender is Button btn)
            {
                if (btn.Name == "basicBtn")
                {
                    btn.Background = new ImageBrush(bgOn);
                    this.deliverBtn.Background = bgOff;
                }
                else if (btn.Name == "deliverBtn")
                {
                    btn.Background = new ImageBrush(bgOn);
                    this.basicBtn.Background = bgOff;
                }
            }
        }

        public void GenDynamicTables(List<MST_TABLE_INFO> tables)
        {
            // sort tables to be generated
            var tablews = tables.Where(z => z.SHAPE_FLAG == "1" || z.SHAPE_FLAG == "2").ToList();
        }

        private void BeginOrder(object sender, MouseButtonEventArgs e)
        {
            string tableCode = ((Grid)sender).Tag.ToString();
            IoC.Get<PostPayTableManagermentViewModel>().MakeTableOrderAsync(tableCode);
        }

        public void RenderTables(List<TABLE_THR> tables)
        {
            var tableSpace = TableGround.FindVisualChildren<Grid>().Where(z => z.Name.StartsWith("Table")).ToList();

            //set coords and property for table

            for (int i = 0; i < tables.Count; i++)
            {
                tableSpace[i].Margin = new Thickness(tables[i].X, tables[i].Y, 0, 0);
                tableSpace[i].Tag = tables[i].TABLE_CODE;
                // cái này chưa y/c sau này sẽ làm
                //  tableSpace[i].Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GoPOS.Resources;component/resource/images/RoundTable.png", UriKind.RelativeOrAbsolute)));

                int rowLimit = (int)Math.Floor((decimal)(tables[i].HEIGHT - 25) / 20);
                var tableGen = tableSpace[i].FindVisualChildren<ListView>().FirstOrDefault();
                if (tableGen != null)
                {
                    var tbTitle = tableSpace[i].FindVisualChildren<TextBlock>().FirstOrDefault(p => p.Name == ("Title" + i));
                    if (tbTitle != null) tbTitle.Text = tables[i].TABLE_NAME;

                    var tbTotal = tableSpace[i].FindVisualChildren<TextBlock>().FirstOrDefault(p => p.Name == ("Total" + i));
                    if (tbTotal != null) tbTotal.Text = tables[i].ORDER_ITEMS.Sum(p => p.DCM_SALE_AMT).ToString("N0");


                    var productList = tables[i].ORDER_ITEMS;
                    var productRem = (productList.Count() - rowLimit).ToString();
                    int qtyRem = 0;
                    //remove items that exceeds row limits
                    if (productList.Count() > rowLimit)
                    {
                        for (int u = rowLimit; u < productList.Count();)
                        {
                            qtyRem += (int)productList[u].SALE_QTY;
                            productList.RemoveAt(rowLimit);
                        }
                        productList[rowLimit - 1].PRD_NAME = "외" + productRem + "메뉴";
                        productList[rowLimit - 1].SALE_QTY = 0;
                    }
                    tableGen.ItemsSource = productList;
                }
            }

            // hidden all unplanned tables
            for (int n = tables.Count(); n < tableSpace.Count(); n++)
            {
                tableSpace[n].Visibility = Visibility.Collapsed;
            }
        }

        public void ShowOrderDetails(bool check)
        {
            var tableSpace = TableGround.FindVisualChildren<Grid>().Where(z => z.Name.StartsWith("Table")).ToList();
            if (check)
            {
                for (int i = 0; i < tableSpace.Count; i++)
                {
                    tableSpace[i].FindVisualChildren<ListView>().FirstOrDefault().Visibility = Visibility.Visible;
                }
            }
            else
            {
                for (int i = 0; i < tableSpace.Count; i++)
                {
                    tableSpace[i].FindVisualChildren<ListView>().FirstOrDefault().Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
