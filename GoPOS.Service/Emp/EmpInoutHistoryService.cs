using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using FirebirdSql.Data.FirebirdClient;

using GoPOS.Models;
using GoPOS.Service.Common;

namespace GoPOS.Services
{
    public class EmpInoutHistoryService : BaseDataService<MST_EMP_INOUT_HISTORY>, IEmpInoutHistoryService
    {
        public EmpInoutHistoryService() : base()
        {

        }
    }
}
