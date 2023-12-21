using GoPOS.Common.Views;
using GoPOS.Interface;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{
    /// <summary>
    /// 
    /// </summary>
    public partial class InitView : UCViewBase, IInitPOSView
    {
        public InitView()
        {
            InitializeComponent();
        }

        public ListBox ProgressMessages => this.lstMessages;
    }
}
