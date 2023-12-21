using GoPOS.Models;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GoPOS.Services
{
    public interface IInfoEmpService : IDataService<MST_INFO_EMP>
    {
        MST_INFO_EMP GetEmpInfo(string empNo);
        MST_INFO_EMP GetEmpInfo(string userId, string password);
    }
}
