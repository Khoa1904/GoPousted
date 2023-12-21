using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayDutchPayTab1View : UserControl
    {
        public OrderPayDutchPayTab1View()
        {
            InitializeComponent();

            List<DuchList1> items = new List<DuchList1>();
            items.Add(new DuchList1() { No = "1", Title1 = "희얌커피", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "" });
            items.Add(new DuchList1() { No = "2", Title1 = "▶ICE", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "속성" });
            items.Add(new DuchList1() { No = "3", Title1 = "희얌커피", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "" });
            items.Add(new DuchList1() { No = "4", Title1 = "▶ICE", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "속성" });
            items.Add(new DuchList1() { No = "5", Title1 = "▶HOT", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "속성" });
            items.Add(new DuchList1() { No = "6", Title1 = "비엔나커피", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "" });
            items.Add(new DuchList1() { No = "7", Title1 = "아메리카노", Title2 = "4,000", Title3 = "3", Title4 = "100", Title5 = "3,900", Title6 = "" });
            items.Add(new DuchList1() { No = "8", Title1 = "카페모카", Title2 = "4,000", Title3 = "4", Title4 = "0", Title5 = "4,000", Title6 = "" });
            items.Add(new DuchList1() { No = "9", Title1 = "케익", Title2 = "4,000", Title3 = "1", Title4 = "0", Title5 = "4,000", Title6 = "" });
            testListView.ItemsSource = items;
            testListView2.ItemsSource = items;
        }

        //[Category("ImageUrl"), Description("이미지Url")]
        //public System.Windows.Media.ImageSource ImageUrl
        //{
        //    get
        //    {
        //        return this.img.Source;
        //    }
        //    set
        //    {
        //        this.img.Source = value;
        //    }
        //}
    }

    public class DuchList1
    {
        public string No { get; set; }

        public string Title1 { get; set; }

        public string Title2 { get; set; }

        public string Title3 { get; set; }
        public string Title4 { get; set; }
        public string Title5 { get; set; }
        public string Title6 { get; set; }
    }
}
