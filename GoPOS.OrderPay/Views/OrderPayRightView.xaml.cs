using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GoShared.Helpers;
using System.Security.RightsManagement;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Media.Media3D;
using GoPOS.ViewModels;
using System.Windows.Data;
using static System.Net.WebRequestMethods;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.Common.Interface.Model;

namespace GoPOS.Views
{
    public partial class OrderPayRightView : UCViewBase, IOrderPayRightView
    {
        public OrderPayRightView()
        {
            InitializeComponent();
            lastClsSelected = null;
        }


        #region Touch Classes

        private Int16 tuchClsPage = 0;
        private List<MST_TUCH_CLASS> m_tuClasses;
        private decimal m_maxTuClsPage;
        private Button lastClsSelected = null;
        private int usedSpace = 0;
        //OrderPayRightViewModel rightViewModel;
        private int blocks = 0;

        public short TuchClsPage
        {
            get => tuchClsPage; set
            {
                tuchClsPage = value;
                usedSpace = 0;
                if (tuchClsPage > m_maxTuClsPage)
                {
                    tuchClsPage = (Int16)m_maxTuClsPage;
                }
                if (tuchClsPage <= 1)
                {
                    tuchClsPage = 1;
                }

                lastClsSelected = null;

                var showClasses = m_tuClasses.Where(p => p.TU_PAGE == tuchClsPage.ToString()).OrderBy(p => p.POSITION_NO).ToList();
                //var DeleteList = m_tuClasses.Where(p => p.TU_PAGE != tuchClsPage.ToString()).OrderBy(p => p.POSITION_NO).ToList();

                var deleteList = gridTouchClass.FindVisualChildren<StackPanel>().Where(p => p.Tag != null).ToArray();

                int btnHeight = 63;
                int btnWidth = 94;

                #region Precalculate used space
                foreach (var tuc in m_tuClasses)
                {
                    usedSpace += int.Parse(tuc.WIDTH) * int.Parse(tuc.HEIGHT);
                }
                #endregion

                bool hasPrevNext = usedSpace > 10;
                var allSps = gridTouchClass.FindVisualChildren<StackPanel>().Where(p => p.Tag != null).ToArray();
                foreach (var item in allSps)
                {
                    item.Visibility = Visibility.Visible;
                    Grid.SetColumnSpan(item, 1);
                    Grid.SetRowSpan(item, 1);

                    Button btn = (Button)item.Children[0];
                    btn.Width = 93;
                    btn.Height = 64;
                }

                foreach (var tuc in showClasses)
                {
                    var position = tuc.POSITION_NO.ToInt32();
                    if (position > (hasPrevNext ? 9 : 10))
                    {
                        continue;
                    }

                    //int row = position > 5 ? 2 : 0;
                    //int col = 2 * (position % 5);
                    //var stp = gridTouchClass.Children.Cast<StackPanel>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
                    var stp = gridTouchClass.Children.Cast<StackPanel>().FirstOrDefault(e => Convert.ToInt32(e.Tag) == position);

                    if (stp == null)
                    {
                        continue;
                    }

                    int col = Grid.GetColumn(stp);
                    int row = Grid.GetRow(stp);

                    // Set Colspan , RowSpan
                    int colspan = Math.Max(tuc.WIDTH.ToInt32() * 2 - 1, 1);
                    int rowspan = Math.Max(tuc.HEIGHT.ToInt32() * 2 - 1, 1);

                    Grid.SetColumnSpan(stp, colspan);
                    Grid.SetRowSpan(stp, rowspan);

                    // Exclude all occupied cells
                    for (int i = col; i < col + colspan; i++)
                    {
                        for (int j = row; j < row + rowspan; j++)
                        {
                            if (j % 2 != 0 || i % 2 != 0 || (i == col && j == row))
                            {
                                continue;
                            }

                            var esp = gridTouchClass.FindVisualChildren<StackPanel>().FirstOrDefault(p => Grid.GetColumn(p) == i && Grid.GetRow(p) == j);


                            if (esp != null) esp.Visibility = Visibility.Collapsed;
                        }
                    }

                    // Set button properties
                    Button btn = (Button)stp.Children[0];
                    TextBlock tb = (TextBlock)btn.Content;
                    tb.Foreground = new SolidColorBrush(Colors.White);
                    btn.Background = new SolidColorBrush(Color.FromRgb(149, 149, 149));
                    if (lastClsSelected == null)
                    {
                        lastClsSelected = btn;
                        var z = btn.Tag;
                        var bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_o.png", UriKind.RelativeOrAbsolute));
                        btn.Background = new ImageBrush(bg);
                        tb.Foreground = new SolidColorBrush(Colors.White);
                    }


                    btn.Click += ClsBtn_Click;
                    tb.Text = tuc.TU_CLASS_NAME;

                    btn.Tag = tuc;
                    var w = colspan > 1 ? btnWidth * ((colspan + 1) / 2) + 5 * (colspan / 2) : btnWidth;
                    var h = rowspan > 1 ? btnHeight * ((rowspan + 1) / 2) + 5 * (rowspan / 2) : btnHeight;
                    btn.Width = w;
                    btn.Height = h;
                }

                if (lastClsSelected != null)
                {
                    ((IOrderPayRightViewModel)this.ViewModel).TouchClsClicked(lastClsSelected.Tag as MST_TUCH_CLASS);
                }
            }
        } // tạm dừng

        private void ClsBtn_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bg = null;
            if (lastClsSelected != null)
            {
                //    bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_w.png", UriKind.RelativeOrAbsolute));
                lastClsSelected.Background = new SolidColorBrush(Color.FromRgb(149, 149, 149));

                ((TextBlock)lastClsSelected.Content).Foreground = new SolidColorBrush(Colors.White);
            }

            bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_o.png", UriKind.RelativeOrAbsolute));
            Button btn = (Button)sender;
            btn.Background = new ImageBrush(bg);

            TextBlock tb = (TextBlock)btn.Content;
            tb.Foreground = new SolidColorBrush(Colors.White);

            lastClsSelected = btn;
        }

        public void BindTouchClasses(List<MST_TUCH_CLASS> tuClasses)
        {
            int blocks = 0;
            m_tuClasses = tuClasses;
            m_maxTuClsPage = tuClasses.Any() ? (tuClasses.Select(p =>
                string.IsNullOrEmpty(p.TU_PAGE) ? (decimal)0 : Convert.ToDecimal(p.TU_PAGE)).Max()) : 0;
            // Precalculate used space
            foreach (var tuc in tuClasses)
            {
                blocks += int.Parse(tuc.WIDTH) * int.Parse(tuc.HEIGHT);
            }
            spLastTuClass.Visibility = blocks <= 10 || m_maxTuClsPage < 2 ? Visibility.Visible : Visibility.Collapsed;
            spTouchPreNext.Visibility = blocks > 10 || m_maxTuClsPage > 1 ? Visibility.Visible : Visibility.Collapsed;
            lastClsSelected = null;
            TuchClsPage = 1;
        }

        #endregion

        #region Touch Products

        private decimal m_maxTuProdPage = 1;
        private short tuchProdPage = 0;
        private List<ORDER_TU_PRODUCT> m_tuProducts;

        public void BindTouchProdList(List<ORDER_TU_PRODUCT> tuProducts)
        {
            blocks = 0;
            m_tuProducts = tuProducts;
            m_maxTuProdPage = tuProducts.Max(p => p?.TU_PAGE) ?? 0;
            foreach (var product in m_tuProducts)
            {
                blocks += (product.WIDTH * product.HEIGHT);
            }
            spLastTuProd.Visibility = blocks <= 30 || m_maxTuClsPage < 2 ? Visibility.Visible : Visibility.Hidden;
            spProdPrevNext.Visibility = blocks >= 30 || m_maxTuProdPage > 1 ? Visibility.Visible : Visibility.Hidden;

            TuchProdPage = 1;
        }

        public short TuchProdPage
        {

            get => tuchProdPage; set
            {
                if (m_tuProducts == null) { return; }
                tuchProdPage = value;
                if (tuchProdPage > m_maxTuProdPage)
                {
                    tuchProdPage = (Int16)m_maxTuProdPage;
                }
                if (tuchProdPage <= 1)
                {
                    tuchProdPage = 1;
                }

                int prodIdx = 0;
                var showProds = m_tuProducts.Where(p => p.TU_PAGE == tuchProdPage).OrderBy(p => p.TU_KEY_CODE).ToList();
                var allSps = gridTouchProduct.FindVisualChildren<StackPanel>().Where(p => p.Tag != null).ToArray();

                foreach (var item in allSps)
                {
                    item.Visibility = Visibility.Visible;
                    Grid.SetColumnSpan(item, 1);
                    Grid.SetRowSpan(item, 1);
                }

                int btnHeight = 63;
                int btnWidth = 93;
                foreach (var stp in allSps)
                {
                    if (stp.Visibility != Visibility.Visible)
                    {
                        continue;
                    }

                    int pos = Convert.ToInt32(stp.Tag);
                    var tup = showProds.FirstOrDefault(p => Convert.ToInt32(p.TU_KEY_CODE) == pos);

                    Button btn = (Button)stp.Children[0];
                    StackPanel child = (StackPanel)btn.Content;
                    TextBlock tbName = (TextBlock)child.Children[0];
                    TextBlock tbPrice = (TextBlock)child.Children[1];
                    var colspan = 1;
                    var rowspan = 1;

                    int col = Grid.GetColumn(stp);
                    int row = Grid.GetRow(stp);

                    // Set Colspan , RowSpan
                    if (tup != null)
                    {
                        colspan = tup.WIDTH * 2 - 1;
                        rowspan = tup.HEIGHT * 2 - 1;

                        Grid.SetColumnSpan(stp, colspan);
                        Grid.SetRowSpan(stp, rowspan);

                        // Exclude all occupied cells
                        if (colspan > 1)
                        {
                            for (int i = col + 1; i < col + colspan; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    continue;
                                }
                                var esp = gridTouchProduct.FindVisualChildren<StackPanel>().FirstOrDefault(p => Grid.GetColumn(p) == i &&
                                            Grid.GetRow(p) == row);
                                if (esp != null) esp.Visibility = Visibility.Collapsed;
                            }
                        }

                        if (rowspan > 1)
                        {
                            for (int i = row + 1; i < row + rowspan; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    continue;
                                }

                                var esp = gridTouchProduct.FindVisualChildren<StackPanel>().FirstOrDefault(p => Grid.GetColumn(p) == col &&
                                            Grid.GetRow(p) == i);
                                if (esp != null) esp.Visibility = Visibility.Collapsed;
                            }
                        }
                        // Set button properties
                        btn.Tag = tup;
                        var w = colspan > 1 ? btnWidth * ((colspan + 1) / 2) + 5 * (colspan / 2) : btnWidth;
                        var h = rowspan > 1 ? btnHeight * ((rowspan + 1) / 2) + 5 * (rowspan / 2) : btnHeight;
                        btn.Width = w;
                        btn.Height = h;

                        tbName.Text = tup?.PRD_NAME ?? string.Empty;
                        tbName.Width = colspan > 1 ? 93 * ((colspan + 1) / 2) + 5 * (colspan / 2) : 93;
                        tbName.Height = rowspan > 1 ? 30 * ((rowspan + 1) / 2) + 5 * (rowspan / 2) : 30;
                        tbPrice.Text = tup?.SALE_UPRC == null ? string.Empty : string.Format("{0:#,##0}", tup.SALE_UPRC);
                        tbPrice.Height = rowspan > 1 ? 15 * ((rowspan + 1) / 2) + 5 * (rowspan / 2) : 15;

                        if (tup.SET_PRD_FLAG == "1" && tup.SIDE_MENU_YN == "N")
                        {
                            btn.Style = (Style)FindResource("TouchBtnBlue");
                        }
                        else if (tup.SIDE_MENU_YN == "N" && tup.SET_PRD_FLAG == "0")
                        {
                            // btn.Background = new SolidColorBrush(Colors.White);
                            btn.Style = (Style)FindResource("TouchBtnWht");
                        }
                        else if (tup.SIDE_MENU_YN == "Y" && tup.SET_PRD_FLAG == "0")
                        {
                            btn.Style = (Style)FindResource("TouchBtn");
                            //LinearGradientBrush gradientBrush = new LinearGradientBrush();
                            //gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(199, 225, 194), 0));
                            //gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(233, 243, 230), 1));
                            //gradientBrush.StartPoint = new Point(0, 0);
                            //gradientBrush.EndPoint = new Point(0, 1);
                            //btn.Background = gradientBrush;
                        }
                        //                    blocks += (colspan * rowspan);
                    }
                    else
                    {
                        var nullSpace = gridTouchProduct.FindVisualChildren<StackPanel>().FirstOrDefault(p => Grid.GetColumn(p) == col &&
                                            Grid.GetRow(p) == row);
                        nullSpace.Visibility = Visibility.Collapsed;
                    }


                }
            }
        }

        #endregion

        #region Pay Buttons

        public void RenderFuncButtons(List<ORDER_FUNC_KEY> funcKeys)
        {
            int btnHeight = 59;
            int btnWidth = 94;
            var allSps = gridPayButtons.FindVisualChildren<StackPanel>().ToArray();
            int stpIdx = 0;
            foreach (var stp in allSps)
            {
                if (stp.Visibility != Visibility.Visible)
                {
                    continue;
                }
                int col = Grid.GetColumn(stp);
                int row = Grid.GetRow(stp);
                var tuc = funcKeys.FirstOrDefault(p => p.COL_NUM * 2 == col && p.ROW_NUM * 2 == row);
                if (tuc == null && stpIdx < funcKeys.Count)
                {
                    tuc = funcKeys[stpIdx];
                }

                if (tuc != null)
                {
                    // Set Colspan , RowSpan
                    int colSpan = tuc.WIDTH_NUM * 2 - 1;
                    int rowSpan = tuc.HEIGHT_NUM * 2 - 1;
                    if (colSpan > 1)
                    {
                        for (int i = colSpan - 1; i > col; i -= 2)
                        {
                            var hideStp = gridPayButtons.Children.Cast<StackPanel>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == i);
                            if (hideStp != null)
                            {
                                hideStp.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        colSpan = 1;
                    }

                    if (rowSpan > 1)
                    {
                        for (int i = rowSpan - 1; i > row; i -= 2)
                        {
                            var hideStp = gridPayButtons.Children.Cast<StackPanel>().FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == col);
                            if (hideStp != null)
                            {
                                hideStp.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        rowSpan = 1;
                    }

                    int width = tuc.WIDTH_NUM == 0 ? 1 : tuc.WIDTH_NUM;
                    int height = tuc.HEIGHT_NUM == 0 ? 1 : tuc.HEIGHT_NUM;
                    Grid.SetColumnSpan(stp, width * 2 - 1);
                    Grid.SetRowSpan(stp, height * 2 - 1);

                    // Set button properties
                    Button btn = (Button)stp.Children[0];
                    btn.Width = btnWidth * width + (width - 1) * 5; // spacing = 5
                    btn.Height = btnHeight * height + (height - 1) * 5; // spacing = 5
                    btn.Tag = tuc;

                    TextBlock tb = (TextBlock)btn.Content;
                    tb.Text = tuc.FK_NAME.Replace("<br/>", Environment.NewLine);
                }

                stpIdx++;
            }
        }

        private void RemoveButton(int row, int column)
        {
            // Loop through the children of the Grid (myGrid)
            for (int i = gridTouchProduct.Children.Count - 1; i >= 0; i--)
            {
                UIElement element = gridTouchProduct.Children[i];

                // Check if the element is a Button and if it matches the specified row and column
                if (element is Button button && Grid.GetRow(button) == row && Grid.GetColumn(button) == column)
                {
                    // Remove btn
                    gridTouchProduct.Children[i].Visibility = Visibility.Collapsed;
                    break;
                }
            }
        }

        #endregion

        #region View related

        #endregion
    }
}
