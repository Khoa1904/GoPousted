using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Common.Views.Controls
{
    public partial class ProgressBarView : Window
    {
        public ProgressBarView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
        public ProgressBar ProgressBarStats { get; set; }


    }
}
