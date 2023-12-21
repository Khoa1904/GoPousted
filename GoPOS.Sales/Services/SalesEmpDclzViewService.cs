using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using FirebirdSql.Data.FirebirdClient;

using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;

namespace GoPOS.Services
{
    public class SalesEmpDclzViewService : ISalesEmpDclzViewService
    {
        public bool AddEmpInoutHistory(EMP_INOUT_HISTORY emp_inout_history)
        {
            const string SQL = @"INSERT INTO EMP_INOUT_HISTORY " +
                "(SHOP_CODE, EMP_IO_DT, EMP_NO, SALE_DATE, EMP_IO_FLAG, EMP_NAME, POS_NO, SEND_FLAG, SEND_DT, EMP_IO_REMARK)" +
                " VALUES " +
                "(@SHOP_CODE, @EMP_IO_DT, @EMP_NO, @SALE_DATE, @EMP_IO_FLAG, @EMP_NAME, @POS_NO, @SEND_FLAG, @SEND_DT, @EMP_IO_REMARK)";

            DynamicParameters param = new();
            param.Add("@SHOP_CODE", emp_inout_history.SHOP_CODE, DbType.String, ParameterDirection.Input, emp_inout_history.SHOP_CODE.Length);
            param.Add("@EMP_IO_DT", emp_inout_history.EMP_IO_DT, DbType.String, ParameterDirection.Input, emp_inout_history.EMP_IO_DT.Length);
            param.Add("@EMP_NO", emp_inout_history.EMP_NO, DbType.String, ParameterDirection.Input, emp_inout_history.EMP_NO.Length);
            param.Add("@SALE_DATE", emp_inout_history.SALE_DATE, DbType.String, ParameterDirection.Input, emp_inout_history.SALE_DATE.Length);
            param.Add("@EMP_IO_FLAG", emp_inout_history.EMP_IO_FLAG, DbType.String, ParameterDirection.Input, emp_inout_history.EMP_IO_FLAG.Length);

            param.Add("@EMP_NAME", emp_inout_history.EMP_NAME, DbType.String, ParameterDirection.Input, emp_inout_history.EMP_NAME.Length);
            param.Add("@POS_NO", emp_inout_history.POS_NO, DbType.String, ParameterDirection.Input, emp_inout_history.POS_NO.Length);
            param.Add("@SEND_FLAG", emp_inout_history.SEND_FLAG, DbType.String, ParameterDirection.Input, emp_inout_history.SEND_FLAG.Length);
            param.Add("@SEND_DT", emp_inout_history.SEND_DT, DbType.String, ParameterDirection.Input, emp_inout_history.SEND_DT.Length);
            param.Add("@EMP_IO_REMARK", emp_inout_history.EMP_IO_REMARK, DbType.String, ParameterDirection.Input, emp_inout_history.EMP_IO_REMARK.Length);

            bool ret = Execute(SQL, param);

            return ret;
        }


        private static bool Execute(string sqlOrProcedure, DynamicParameters? parameters)
        {
            return false;
        }
    }
}
