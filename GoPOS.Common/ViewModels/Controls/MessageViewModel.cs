using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.Common.Interface.Model;


/*
 공통 > 메시지 박스

 */

namespace GoPOS.Common.ViewModels.Controls
{

    public class MessageViewModel : BaseItemViewModel, IViewModel
    {
        public MessageBoxResult result = MessageBoxResult.Cancel;

        public MessageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            Init();
        }

        private async void Init()
        {
            IoC.Get<MessageView>().txtMsg.Text = "";
            await Task.Delay(100);
        }

        static string OK_TEXT = Application.Current.FindResource("0172") as string;
        static string CANCEL_TEXT = Application.Current.FindResource("0144") as string;
        public void MsgSet(string msg, GMessageBoxButton type, params string[] buttonCaps)
        {
            ////Q 질문식 //N 확인
            if (type == GMessageBoxButton.OK)
                IoC.Get<MessageView>().btnCancel.Visibility = Visibility.Collapsed;
            else
                IoC.Get<MessageView>().btnCancel.Visibility = Visibility.Visible;

            if (buttonCaps.Length > 0)
            {
                (IoC.Get<MessageView>().btnConfirm.Content as TextBlock).Text = buttonCaps[0];
                if (buttonCaps.Length > 1)
                {
                    (IoC.Get<MessageView>().btnCancel.Content as TextBlock).Text = buttonCaps[1];
                }
            }
            else
            {
                (IoC.Get<MessageView>().btnConfirm.Content as TextBlock).Text = OK_TEXT;
                (IoC.Get<MessageView>().btnCancel.Content as TextBlock).Text = CANCEL_TEXT;

            }
            IoC.Get<MessageView>().txtMsg.Text = msg;
        }

        public MessageBoxResult DialogResult()
        {
            return result;
        }

        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            if (sValue == "Y")
            {
                result = MessageBoxResult.OK;
            }
            else
            {
                result = MessageBoxResult.Cancel;
            }

            this.TryCloseAsync(true);
        }
    }
}