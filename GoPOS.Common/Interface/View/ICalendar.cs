using GoPOS.Common.Interface.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.View
{
    public interface ICalendar : IView
    {
        void RenderCalendarDays(ObservableCollection<int> CalendarDays);

    }
}
