using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoShared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace GoPOS.Common.Service
{
    public class SelectboxviewService : ISelectboxviewService
    {
        public async Task<List<MST_COMM_CODE>> GetCommonCode(string code)
        {
            try
            {
                string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetCommonCode");

                DynamicParameters param = new DynamicParameters();
                param.Add("@COM_CODE_FLAG", code, DbType.Int32, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);

                List<MST_COMM_CODE> result = await DapperORM.ReturnListAsync<MST_COMM_CODE>(SQL, param);
                    
                return result;

            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<MST_COMM_CODE>();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<MST_COMM_CODE>();
            }
        }

        public async Task<List<MST_INFO_POS>> GetCommonPos()
        {
            try
            {
                string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetCommonPos");
                List<MST_INFO_POS> result = await DapperORM.ReturnListAsync<MST_INFO_POS>(SQL, new string[]
                    {
                    "@SHOPCODE",
                    },
                    new object[]
                    {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    });

                return result;

            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<MST_INFO_POS>();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<MST_INFO_POS>();
            }
        }
        public async Task<List<SETT_POSACCOUNT>> GetSEQ()
        {
            try
            {
                string SQL = @"
                    select REGI_SEQ from SETT_POSACCOUNT WHERE
                SHOP_CODE = @SHOPCODE and
                SALE_DATE = @SALEDATE and
                POS_NO    = @POSNO
                ORDER BY REGI_SEQ
                ";
                List<SETT_POSACCOUNT> result = await DapperORM.ReturnListAsync<SETT_POSACCOUNT>(SQL, new string[]
                    {
                    "@SHOPCODE",
                    "@SALEDATE",
                    "@POSNO"
                    },
                    new object[]
                    {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.PosStatus.SALE_DATE,
                    DataLocals.AppConfig.PosInfo.PosNo
                    });

                return result;

            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<SETT_POSACCOUNT>();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new List<SETT_POSACCOUNT>();
            }
        }

        public void OmniRender(List<MST_COMM_CODE> common)
        {
            throw new NotImplementedException();
        }

        public void POSRender(List<MST_INFO_POS> Pos)
        {
            throw new NotImplementedException();
        }
    }

    public class test
    {
        public string COM_CODE_FLAG { get; set; }
        public string COM_CODE { get; set; }
        public string COM_CODE_NAME { get; set; }
    }
}
