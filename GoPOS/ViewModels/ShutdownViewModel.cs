using System.Windows;
using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Views;
using GoPOS.Services;
using GoPOS.Common.ViewModels;
using GoPOS.Interface;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using static GoShared.Events.GoPOSEventHandler;
using GoShared.Events;
using GoPOS.Common.Interface.Model;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using GoPOS.Helpers;
using System.Threading;
using System.Collections.Generic;
using GoShared.Helpers;

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>
    public class ShutdownViewModel : BaseItemViewModel, IDialogViewModel
    {
        public ShutdownViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandImpl);

        public Dictionary<string, object> DialogResult
        {
            get; set;
        }

        private void ButtonCommandImpl(Button button)
        {
            switch (button.Tag)
            {
                case "ShutDown":
                    //Application.Current.Shutdown();
                    Application.Current.MainWindow.Close();
                    break;

                case "Restart":
                    Application.Current.MainWindow.Close();
                    SystemHelper.ProgramRestart();
                    break;

                case "Close":
                    this.TryCloseAsync(true);
                    break;

                default:
                    NotifyChangePage((string)button.Tag);
                    break;

            }
        }

        public Task HandleAsync(GoShared.Events.CloseEventArgs message, CancellationToken cancellationToken)
        {
            return this.TryCloseAsync();
        }
    }
}
