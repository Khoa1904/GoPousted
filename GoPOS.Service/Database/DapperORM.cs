using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoShared.Helpers;
using GoPOS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Xml;
using GoPOS.Service;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoPOS.Database;

public static class DapperORM
{
    public const string GOPOS_DBNAME = "GoPOS.fdb";

    public static string Firebird()
    {
        string directory = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
        FbConnectionStringBuilder connectionBuilder = new()
        {
            ConnectionLifeTime = 40,
            ServerType = FbServerType.Embedded,
            ClientLibrary = directory + @"\libs\Firebird4\fbclient.dll",
            Database = directory + @"\data\" + GOPOS_DBNAME,
            UserID = "SYSDBA",
            Password = "masterkey",
            Charset = FbCharset.Utf8.ToString()
        };

        return connectionBuilder.ToString();
    }

    public static string DbFilePath(bool backUpPath)
    {
        string directory = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
        directory = directory + @"\data\" + (backUpPath ? @"backup\" : string.Empty);
        return directory + (backUpPath ? string.Format("GoPOS_{0:yyyyMMdd}.fdb", DateTime.Today) : GOPOS_DBNAME);
    }

    public static async Task<T> ReturnSingleAsync<T>(string sqlOrProcedure, DynamicParameters? parameters) where T : class, new()
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            return await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        else
        {
            return await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.Text);
        }
    }

    public static async Task<T> ReturnSingleAsync<T>(string sqlOrProcedure, string[] paramNames, object[] paramValues) where T : class, new()
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());

        var parameters = new DynamicParameters();
        for (int i = 0; i < paramNames.Length; i++)
        {
            parameters.Add(paramNames[i], paramValues[i]);
        }

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            return await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        else
        {
            return await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.Text);
        }
    }

    public static async Task<List<T>> ReturnListAsync<T>(string sqlOrProcedure, DynamicParameters? parameters) where T : class, new()
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            return (await DapperFB.QueryAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure)).ToList();
        }
        else
        {
            return (await DapperFB.QueryAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.Text)).ToList();
        }
    }

    public static async Task<List<T>> ReturnListAsync<T>(string sqlOrProcedure, string[] paramNames, object[] paramValues) where T : class, new()
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());

        var parameters = new DynamicParameters();
        for (int i = 0; i < paramNames.Length; i++)
        {
            parameters.Add(paramNames[i], paramValues[i]);
        }

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            return (await DapperFB.QueryAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure)).ToList();
        }
        else
        {
            return (await DapperFB.QueryAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.Text)).ToList();
        }
    }

    public static async Task<T> ExecuteAsync<T>(string sqlOrProcedure, DynamicParameters? parameters) where T : class, new()
    {
        using TransactionScope scope = new();
        using IDbConnection DapperFB = new FbConnection(Firebird());

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            T result = await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure);
            scope.Complete();
            return result;
        }
        else
        {
            T result = await DapperFB.QuerySingleOrDefaultAsync<T>(sqlOrProcedure, parameters, commandType: CommandType.Text);
            scope.Complete();
            return result;
        }
    }

    public static async Task<int> ExecuteAsync(string sqlOrProcedure, DynamicParameters? parameters)
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());

        if (string.Equals(sqlOrProcedure.Left(2), "SP", StringComparison.OrdinalIgnoreCase))
        {
            return await DapperFB.ExecuteAsync(sqlOrProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        else
        {
            return await DapperFB.ExecuteAsync(sqlOrProcedure, parameters, commandType: CommandType.Text);
        }
    }
    public static async Task<int> ExecuteNonQueryAsync(string sqlOrProcedure)
    {
        var record = 0;
        var list = sqlOrProcedure.Split(";\r\n");
        if(list != null && list.Any())
        {
            using (var context = new DataContext())
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item)) {
                        record += context.Database.ExecuteSqlCommand(item);
                    }
                }
            }
        }
        return record;
    }
    public static (T, SpResult) ReturnResult<T>(T t, EResultType resultType, string resultMessage) where T : class, new()
    {
        string resultCode = string.Empty;
        int resultCount = 0;

        if (resultType == EResultType.SUCCESS)
        {
            resultCode = "0000";
            resultCount = 1;
        }
        else if (resultType == EResultType.EMPTY)
        {
            resultCode = "1000";
        }
        else if (resultType == EResultType.ERROR)
        {
            resultCode = "9000";
        }

        if (t.IsGenericList())
        {
            resultCount = (t as IList)?.Count ?? 0;
        }

        SpResult spResult = new()
        {
            ResultType = resultType,
            ResultCode = resultCode,
            ResultMessage = resultType.ToString() + " : " + resultMessage,
            ResultCount = resultCount
        };

        return (t, spResult);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool DBConnectionTest()
    {
        using IDbConnection DapperFB = new FbConnection(Firebird());
        int retryCount = 3;
    _RETRY:
        try
        {
            DapperFB.Open();
            DapperFB.Close();
            return true;
        }
        catch (Exception ex)
        {
            DapperFB.Close();
            if (retryCount > 0)
            {
                retryCount--;
                goto _RETRY;
            }
            LogHelper.Logger.Error(ex);
            return false;
        }
    }
}