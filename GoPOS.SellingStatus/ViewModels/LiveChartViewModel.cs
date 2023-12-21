using GoPOS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.SellingStatus.Interface;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using GoPOS.Common.Interface.Model;
using GoPOS.Service;
using LiveCharts.Helpers;

namespace GoPOS.SellingStatus.ViewModels
{
    public class LiveChartViewModel : BaseItemViewModel
    {
        private ILiveChart _view;

        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;



        public LiveChartViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {

            _eventAggregator = eventAggregator;
            _windowManager = windowManager;

            _eventAggregator.SubscribeOnUIThread(this);

            XPointer = -5;
            YPointer = -5;

            //the formatter or labels property is shared 
            Formatter = x => x.ToString("N0");

            CharValues = new ChartValues<double>();
        }

        private double _xPointer;
        private double _yPointer;

        public double XPointer
        {
            get { return _xPointer; }
            set
            {
                _xPointer = value;
                NotifyOfPropertyChange(nameof(XPointer));
            }
        }

        public double YPointer
        {
            get { return _yPointer; }
            set
            {
                _yPointer = value;
                NotifyOfPropertyChange(nameof(YPointer));
            }
        }

        public List<string> listNameColumn;
        public List<string> ListNameColumn
        {
            get { return listNameColumn; }
            set
            {
                if (value!=null && value.Any())
                {
                    listNameColumn = value;

                    NotifyOfPropertyChange(() => ListNameColumn);
                }
            }

        }

        private ChartValues<double> _charValues;
        public ChartValues<double> CharValues
        {
            get => _charValues;
            set
            {
                if (value.Count>20)
                {
                    List<string> listColumn = new List<string>();
                    for (int i = 1; i <= value.Count(); i++)
                    {
                        listColumn.Add(i.ToString());
                    }
                    ListNameColumn = listColumn;


                }
                _charValues = value;
                NotifyOfPropertyChange(() => CharValues);
            }
        }

        public Func<double, string> Formatter { get; set; }

        public override bool SetIView(IView view)
        {
            _view = (ILiveChart)view;
            _view.Chart.MouseMove += Chart_MouseMove;
            return base.SetIView(view);
        }

        private void Chart_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                var chart = (LiveCharts.Wpf.CartesianChart)sender;

                //lets get where the mouse is at our chart
                var mouseCoordinate = e.GetPosition(chart);

                //now that we know where the mouse is, lets use
                //ConverToChartValues extension
                //it takes a point in pixes and scales it to our chart current scale/values
                var p = chart.ConvertToChartValues(mouseCoordinate);
                if (p.X == 0 && p.Y == 0) return;
                //in the Y section, lets use the raw value
                var series = chart.Series[0];
                var closetsPoint = series.ClosestPointTo(p.X, AxisOrientation.X);
                this.YPointer = closetsPoint.Y;

                //for X in this case we will only highlight the closest point.
                //lets use the already defined ClosestPointTo extension
                //it will return the closest ChartPoint to a value according to an axis.
                //here we get the closest point to p.X according to the X axis

                this.XPointer = closetsPoint.X;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
              
            }
            

        }
    }
    public class ColumnData
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
