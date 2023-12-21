using System.Windows;
using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Views;
using GoPOS.Services;
using GoPOS.Common.ViewModels;
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

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>
    public class PermissionMngViewModel : BaseItemViewModel, IDialogViewModel
    {
        public PermissionMngViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
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
                case "OK":
                    this.TryCloseAsync(true);
                    break;

                case "Cancel":
                    this.TryCloseAsync(true);
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

        static void RestartProgram()
        {
            // reset
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);

            // clear environment
            Environment.Exit(0);
        }
    }
}
