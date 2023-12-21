using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoPOS.Models;

namespace GoPOS.Services
{
    public interface ISalesEmpDclzViewService
    {
        public bool AddEmpInoutHistory(EMP_INOUT_HISTORY emp_inout_history);
    }
}
