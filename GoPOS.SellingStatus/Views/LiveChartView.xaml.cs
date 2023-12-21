using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GoPOS.Common.Views;
using GoPOS.SellingStatus.Interface;
using LiveCharts.Configurations;

namespace GoPOS.SellingStatus.Views
{
    /// <summary>
    /// Interaction logic for LiveChart.xaml
    /// </summary>
    public partial class LiveChartView : UCViewBase, ILiveChart
    {

        public LiveChartView()
        {
            InitializeComponent();
        }
        
        public CartesianChart Chart => chart;
    }
}