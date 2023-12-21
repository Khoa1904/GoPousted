using Dapper;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Services
{
    public interface IEmployeeService : IDataService<MST_EMP_FUNC_KEY> 
    {
        Task<(List<MST_EMP_FUNC_KEY>, SpResult)> GetEmpFuncKey(DynamicParameters param);
    }
}
