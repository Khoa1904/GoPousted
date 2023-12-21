using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GoShared.Helpers;

namespace GoPOS.Views
{
    public partial class OrderPayCardCompListView : UCViewBase, IOrderPayCardCompListView
    {
        public OrderPayCardCompListView()
        {
            InitializeComponent();
        }



        public void RenderCreditCardDeck(MST_INFO_CARD[] CrCard)  
        {
            throw new NotImplementedException();
            //var deck = CrCard.ToArray(); cứ để lại sau này xài
            //if(deck.Count() > 9)
            //{
            //    btnPrev.Visibility = Visibility.Visible;
            //    btnNext.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    btnPrev.Visibility = Visibility.Hidden;
            //    btnNext.Visibility = Visibility.Hidden;
            //}

            //var KardList = Cardlist.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("BtnIcon")).ToList();
            //for (int i = 0; i < KardList.Count; i++)
            //{
            //    ImageBrush IBrush = new ImageBrush();
            //    IBrush.ImageSource = new BitmapImage(new Uri("/GoPOS.Resources;component/resource/Images/Icon/CreditCard/{CrCard[i].CARD_ICON_PATH}"));
            //    KardList[i].Background = IBrush;
            //    KardList[i].Tag = CrCard[i].CARD_ICON_PATH.Replace(".png", string.Empty);
            //}
        }
    }
}
