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
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using GoShared.Helpers.Nini.Ini;
using System.Windows.Threading;


/*
 공통 > 메시지 박스

 */

namespace GoPOS.Common.ViewModels.Controls
{

    public class ProgressBarViewModel : BaseItemViewModel, IViewModel
    {
        private DispatcherTimer timer;
        private decimal _progressPerc;
        public decimal ProgressPerc
        {
            get => _progressPerc;
            set
            {
                _progressPerc = value;
                NotifyOfPropertyChange(() => ProgressPerc);
            }
        }
        private string _title { get; set; }
        public string TITLE
        {
            get => !string.IsNullOrEmpty(_title) ? _title : "Executing process:";
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => TITLE);
            }
        }
        private int _process { get; set; }
        public int PROCESS
        {
            get => _process;
            set
            {
                _process = value;
                NotifyOfPropertyChange(() => PROCESS);
            }
        }
        private int _overall { get; set; }
        public int OVERALL
        {
            get => _overall;
            set
            {
                _overall = value;
                NotifyOfPropertyChange(() => OVERALL);
            }
        }

        public ProgressBarViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            Init();
        }

        private async void Init()
        { // Create an illusion that progress bar is actually running
            PROCESS = 0;
            OVERALL = 12;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.4);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ProgressPerc = Convert.ToInt32(100 * (Convert.ToDouble(PROCESS) / this.OVERALL));
            PROCESS++;
            // Stop progress running value if it reaches maximum
            if (PROCESS > OVERALL)
                PROCESS = PROCESS;
        }
    }
}