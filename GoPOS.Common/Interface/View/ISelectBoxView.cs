using GoPOS.Common.Interface.Model;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.View
{
    public interface ISelectBoxView : IView
    {
        void SEQRender(SETT_POSACCOUNT[] Req, int currentPage);
        void POSRender(MST_INFO_POS[] Pos, int currentPage);

        void COMMONRender(MST_COMM_CODE[] Common, int currentPage);
    }
}
