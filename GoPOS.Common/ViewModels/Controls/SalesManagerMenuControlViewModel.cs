using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using GoPOS.Views;

namespace GoPOS.Common.ViewModels.Controls
{
    public class SalesManagerMenuControlViewModel : Screen
    {
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        public SalesManagerMenuControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, string selectedMenu)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            SelectedMenu = selectedMenu;

            if (SelectedMenu.Equals("SellerChangeViewModel"))
            {
                SellerChangeViewImage = MenuOnImage;
                EmpDclzViewModelImage = MenuOffImage;
            }
            else if (SelectedMenu.Equals("EmpDclzViewModel"))
            {
                SellerChangeViewImage = MenuOffImage;
                EmpDclzViewModelImage = MenuOnImage;
            }

            _eventAggregator.SubscribeOnUIThread(this);
        }

        public string SelectedMenu
        {
            get; set;
        } 

        public string SellerChangeViewImage
        {
            get; set;
        } = string.Empty;

        public string EmpDclzViewModelImage
        {
            get; set;
        } = string.Empty;

        public string MenuOnImage
        {
            get { return "pack://application:,,,/GoPOS.Resources;component/resource/images/btn_main_right_on.png"; }
        }
        public string MenuOffImage
        {
            get { return "pack://application:,,,/GoPOS.Resources;component/resource/images/btn_main_right.png"; }
        }

    }
}
