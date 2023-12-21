using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Service
{
    public interface ISelectboxviewService
    {
        void OmniRender(List<MST_COMM_CODE> common);
        void POSRender(List<MST_INFO_POS> Pos);
         Task<List<SETT_POSACCOUNT>> GetSEQ();
        Task<List<MST_INFO_POS>> GetCommonPos();
        Task<List<MST_COMM_CODE>> GetCommonCode(string code);
    }
}
