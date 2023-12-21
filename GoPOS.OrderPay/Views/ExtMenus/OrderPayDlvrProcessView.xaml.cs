using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPayDlvrProcessView : UserControl
    {
        public OrderPayDlvrProcessView()
        {
            InitializeComponent();

            this.Loaded += OrderPayDlvrProcessView_Loaded;
        }

        private void OrderPayDlvrProcessView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
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
}
