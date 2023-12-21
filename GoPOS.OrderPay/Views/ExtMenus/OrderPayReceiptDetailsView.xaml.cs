using GoPOS.Common.Views;
using GoPOS.OrderPay.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for OrderpayReceiptDetails.xaml
    /// </summary>
    public partial class OrderPayReceiptDetailsView : UCViewBase, IOrderPayReceiptDetailsView
    {
        public OrderPayReceiptDetailsView()
        {
            InitializeComponent();
        }

        public void SetGridPreview(List<Dictionary<string, string>> data)
        {
            for (int i = GridRow.RowDefinitions.Count - 1; i >= 0; i--)
            {
                RowDefinition rowDefinition = GridRow.RowDefinitions[i];
                GridRow.RowDefinitions.Remove(rowDefinition);
            }
            GridRow.Children.Clear();
            GridRow.UpdateLayout();
            int j = 0;
            if (data == null) return;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i]["Left"] == "" && data[i]["Left"] == "")
                {
                    j++;
                    continue;
                }
                if (data[i]["Left"] == " 부       가      세")
                {
                    data[i]["Left"] = " 부      가     세";
                }
                if (data[i]["Left"] == "신용카드")
                {
                    data[i]["Left"] = "신  용 카  드";
                }
                if (data[i]["Left"] == "상품명               단가 수량       금액")
                {
                    data[i]["Left"] = "상품명                단가   수량       금액";
                }

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(20);
                GridRow.RowDefinitions.Add(rowDefinition);

                Encoding encoding949 = Encoding.GetEncoding(949); // EUC-KR
                int byteCount949 = encoding949.GetByteCount(data[i]["Left"].TrimEnd());

                TextBox textBoxLeft = new TextBox
                {
                    Text = data[i]["Left"],
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ececec")),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ececec")),
                    FontFamily = new FontFamily("Cascadia Mono"),
                    //FontFamily = new FontFamily("DotumChe"),
                    //FontSize = 9.75,
                    IsReadOnly = true,
                    //FontSize = 14
                    //FontWeight = FontWeights.Bold

                };

                if (byteCount949 > 42)
                {
                    Grid.SetColumnSpan(textBoxLeft, 2);
                    textBoxLeft.TextWrapping = TextWrapping.Wrap;
                    rowDefinition.Height = new GridLength(40);
                }

                if (i == 2)
                {
                    rowDefinition.Height = new GridLength(30);
                    textBoxLeft.Text = data[i]["Left"].Trim();
                    textBoxLeft.FontWeight = FontWeights.Bold;
                    textBoxLeft.FontSize = 20;
                    textBoxLeft.HorizontalAlignment = HorizontalAlignment.Center;
                }
                GridRow.Children.Add(textBoxLeft);
                Grid.SetRow(textBoxLeft, i-j);
                Grid.SetColumn(textBoxLeft, 0);
                var right = data[i]["Right"];
                if(right == "")
                {
                    Grid.SetColumnSpan(textBoxLeft, 2);
                }
                else
                {
                    TextBox textBoxRight = new TextBox
                    {
                        Text = data[i]["Right"],
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ececec")),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ececec")),
                        FontFamily = new FontFamily("Cascadia Mono"),
                        //FontFamily = new FontFamily("DotumChe"),
                        //FontSize = 9.75,
                        IsReadOnly = true
                        //FontWeight = FontWeights.Bold
                        //FontSize = 14
                    };
                    GridRow.Children.Add(textBoxRight);
                    Grid.SetRow(textBoxRight, i - j);
                    Grid.SetColumn(textBoxRight, 1);
                }

            }
        }

        public void RenderReceiptPreview(string receiptText)
        {
            // txtReceiptContents
            txtReceiptContents.Text = receiptText;
        }
    }
}
