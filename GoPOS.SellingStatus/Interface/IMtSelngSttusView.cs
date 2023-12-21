using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Models;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Interface
{
    public interface IMtSelngSttusView : IView
    {
        void RenderCalendarDays(ObservableCollection<int> CalendarDays,List<SELLING_STATUS_INFO> infos,string date);
        void GenerateWeekdaySale();

        TextBlock day1 { get; }
        TextBlock day8 { get; }
        TextBlock day15 { get; }
        TextBlock day22 { get; }
        TextBlock day29 { get; }

        TextBlock day36 { get; }
        Point buttonPosition { get; set; }

    }
}
