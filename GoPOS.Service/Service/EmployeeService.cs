using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service.Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Microsoft.AspNetCore.Server.IIS.Core;
using GoPOS.Models._0_Common;

namespace GoPOS.Services
{
    public class EmployeeService : BaseDataService<MST_EMP_FUNC_KEY>, IEmployeeService
    {
        public async Task<(List<MST_EMP_FUNC_KEY>, SpResult)> GetEmpFuncKey(DynamicParameters param)
        {
            try
            {

                string SQL = @"
                                SELECT K.FK_NO	FROM MST_EMP_FUNC_KEY K	LEFT JOIN	MST_INFO_EMP E  
				                						ON			E.SHOP_CODE = K.SHOP_CODE
				                						AND			E.EMP_FLAG	= K.EMP_FLAG
				                WHERE	E.SHOP_CODE = @SHOPCODE
				                AND		E.EMP_NO	= @EMPNO
                           ";
                List<MST_EMP_FUNC_KEY>  result = await DapperORM.ReturnListAsync<MST_EMP_FUNC_KEY>(SQL, param);

                return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return DapperORM.ReturnResult(new List<MST_EMP_FUNC_KEY>(), EResultType.ERROR, ex.Message.ReplacePlainText());
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
                return DapperORM.ReturnResult(new List<MST_EMP_FUNC_KEY>(), EResultType.ERROR, ex.Message.ReplacePlainText());
            }
        }
    }
}
