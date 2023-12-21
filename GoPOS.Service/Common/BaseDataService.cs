using AutoMapper.Internal;
using Dapper;
using GoPOS.Database;
using GoPOS.Models.Common;
using GoShared.Contract;
using GoShared.Events;
using GoShared.Helpers;
using GoShared.Interface;
using log4net.Appender;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using static GoShared.Events.GoPOSEventHandler;

namespace GoPOS.Service.Common
{
    public class BaseDataService<TEntity>
        where TEntity : class, IIdentifyEntity, new()
    {

        #region Constructer
        public BaseDataService()
        {
            //_apiRequest = new ApiRequest();
        }
        public BaseDataService(ApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }
        #endregion

        //**------------------------------------------------------------

        #region Member
        protected ApiRequest _apiRequest;
        public event GeneralEventHandler<IList<TEntity>> DataSynchronized;
        public event GeneralEventHandler<TEntity> Added;
       

        protected bool _isClearDataWhenSyn = true;
        #endregion

        //**------------------------------------------------------------

        #region SynchronizedData
        protected virtual async Task<ResultInfo> OnSynchronizedData(IDictionary<string, string> paras, bool singleResult, bool updateDb,object body)
        {
            ResultInfo result = new ResultInfo()
            {
                Success = false
            };

            string reqResults = string.Empty;

            try
            {
                if (_apiRequest == null)
                {
                    _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer, 
                                DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.AppConfig.PosInfo.PosNo,
                                DataLocals.TokenInfo.LICENSE_ID, DataLocals.TokenInfo.LICENSE_KEY);
                }

                var module = new TEntity();

                var f = await _apiRequest.Request(module.Resource()).GetDatasAsync<TEntity>(paras, body);

                if (f.ResultCode == ResultCode.Success)
                {
                    reqResults = Convert.ToString(f.results);
                    dynamic jsonResult;
                    if (!singleResult)
                    {
                        jsonResult = JsonHelper.JsonToModels<TEntity>(reqResults);
                    }
                    else
                    {
                        jsonResult = JsonHelper.JsonToModel<TEntity>(reqResults);
                    }

                    if (jsonResult.Item2 == ResultCode.Ok)
                    {
                        var datas = jsonResult.Item1;
                        if (datas != null)
                        {
                            result.Success = true;
                            if (updateDb)
                            {
                                bool cleanData = false;
                                    cleanData = datas != null && _isClearDataWhenSyn;
                                if (!singleResult)
                                {
                                    var list = datas as IList<TEntity>;
                                }

                                if (cleanData)
                                {
                                    OnTruncateTable();
                                }

                                ResultInfo insInfo = AddOrUpdate(datas);
                                result.Success = insInfo.Success;
                                result.Message = insInfo.Message;
                                result.Record = insInfo.Record;
                                result.TotalRecords = insInfo.Record;
                            }

                            result.Data = datas;
                            if (!singleResult)
                            {
                                result.Record = datas.Count;
                                result.TotalRecords = datas.Count;
                            }
                        }
                        else
                        {
                            result.Code = "NODATA";
                            result.Message = "No Data returned.";
                        }
                    }
                    else
                    {
                        result.Code = jsonResult.result;
                        result.Message = jsonResult.result;
                    }
                }
                else
                {
                    result.Code = f.ResultCode;
                    result.Message = f.ResultMsg;
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = reqResults + Environment.NewLine + ex.ToFormattedString();
                return result;
            }

            return result;
        }
        public virtual ResultInfo SynchronizedData(IDictionary<string, string> paras)
        {
            return SynchronizedData(paras, false, true).Result;
        }

        public virtual async Task<ResultInfo> SynchronizedData(IDictionary<string, string> paras, bool updateDb)
        {
            return await SynchronizedData(paras, true, updateDb);
        }

        public virtual async Task<ResultInfo> SynchronizedData(IDictionary<string, string> paras, bool singleResult, bool updateDb)
        {
            return await OnSynchronizedData(paras, singleResult, updateDb, null);
        }
        public virtual async Task<ResultInfo> SynchronizedData(IDictionary<string, string> paras, bool singleResult, bool updateDb, object body)
        {
            return await OnSynchronizedData(paras, singleResult, updateDb, body);
        }

        #endregion

        //**------------------------------------------------------------

        #region AddOrUpdate
        protected virtual ResultInfo OnAddOrUpdate(IList<TEntity> entities)
        {
            var result = new ResultInfo();
            try
            {
                if (entities != null && entities.Any())
                {
                    int sucCount = 0;
                    bool suc = false;
                    string errorMessage = string.Empty;
                    using (var context = new DataContext())
                    {
                        entities.ForAll(e =>
                        {
                            //context.Set<TEntity>().AddOrUpdate(e);
                            // Test
                            try
                            {
                                context.Set<TEntity>().AddOrUpdate(e);
                                context.SaveChanges();
                                sucCount++;
                            }

                            catch (DbEntityValidationException err)
                            {
                                suc = false;
                                StringBuilder valErrorMsg = new StringBuilder();
                                foreach (var eve in err.EntityValidationErrors)
                                {
                                    valErrorMsg.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    valErrorMsg.AppendLine();
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        valErrorMsg.AppendFormat("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                        valErrorMsg.AppendLine();
                                    }
                                }

                                errorMessage += valErrorMsg.ToString();
                                LogHelper.Logger.Error(valErrorMsg);
                            }
                            catch (Exception ex)
                            {
                                suc = false;
                                errorMessage += ex.ToFormattedString();
                                errorMessage += Environment.NewLine;
                                LogHelper.Logger.Error(ex.ToFormattedString());
                            }
                        });
                    }

                    result.Success = true;
                    result.Record = sucCount;
                    result.Data = entities;
                    result.Message = errorMessage;
                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = "No Data";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = ex.ToFormattedString();
                return result;
            }
        }
        public virtual ResultInfo AddOrUpdate(IList<TEntity> entities)
        {
            return OnAddOrUpdate(entities);
        }

        public virtual ResultInfo AddOrUpdate(TEntity entity)
        {
            var list = new List<TEntity>();
            list.Add(entity);
            return OnAddOrUpdate(list);
        }

        #endregion

        //**------------------------------------------------------------

        #region Add
        protected virtual void DefaultInsert(TEntity entity, string userID, object data)
        {
            entity.EntityDefValue(EEditType.Add, userID, data);
        }
        protected virtual ResultInfo OnValidateAdd(TEntity info)
        {
            return new ResultInfo() { Success = true };
        }
        protected virtual ResultInfo OnAddRefference(DataContext context, TEntity table, TEntity info)
        {
            return new ResultInfo() { Success = true };
        }
        protected virtual ResultInfo OnAdd(DataContext _context, TEntity entity, string userID, ESaveType type, object refData)
        {
            var result = new ResultInfo();
            try
            {
                result = OnValidateAdd(entity);
                if (result.Success)
                {
                    var entityTemp = JsonHelper.InfoToJson(entity);
                    DefaultInsert(entity, userID, entity);
                    _context.Set<TEntity>().Add(entity);
                    _context.SaveChanges();

                    var result1 = OnAddRefference(_context, entity, entity);
                    if (result1.Success)
                    {
                        result.Success = true;
                        result.Message = "";
                        result.Code = LSystem.CSuccess;
                        result.Data = entity;
                        result.Record = 1;
                        LogHelper.Logger.Info($"{LSystem.LInsertSuccess} {typeof(TEntity).Name}: {entity.Base_PKValue()}");
                        if (Added != null) Added(this, new GoShared.Events.EventArgs<TEntity>(entity));
                    }
                    else
                    {
                        return result1;
                    }

                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                
                LogHelper.Logger.Error(ex.ToFormattedString());
                result.Success = false;
                result.Message = ex.Message;
                result.Code = LSystem.CSystemError;
            }
            return result;
        }

        //**------------------------------------------------------------

        public virtual ResultInfo Add(TEntity info, string userID)
        {
            using (var context = new DataContext())
            {
                return OnAdd(context, info, userID, ESaveType.None, info);
            }
        }

        public virtual ResultInfo Add(DataContext context, TEntity info, string userID)
        {
            return OnAdd(context, info, userID, ESaveType.None, info);
        }
        public virtual ResultInfo Add(System.Data.Entity.DbContext context, TEntity info, string userID)
        {
            return OnAdd((DataContext)context, info, userID, ESaveType.None, info);
        }
        public virtual Task<ResultInfo> AddAsyn(TEntity info, string userID) => Task.Factory.StartNew(() => { return Add(info, userID); });
        //**------------------------------------------------------------
        public virtual ResultInfo AddTrain(TEntity info, string userID)
        {
            using (var context = new DataContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        var result = OnAdd(context, info, userID, ESaveType.None, info);
                        if (result.Success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        LogHelper.Logger.Error(ex);
                        return new ResultInfo() { Success = false, Message = ex.Message };
                    }
                }

            }
        }
        public virtual Task<ResultInfo> AddTrainAsyn(TEntity info, string userID) => Task.Factory.StartNew(() => { return AddTrain(info, userID); });
        #endregion

        //**------------------------------------------------------------

        #region Delete data
        protected virtual ResultInfo OnTruncateTable()
        {
            var modelEntityType = typeof(TEntity);

            // Retrieve a TableAttribute that is assigned to the class generated by LINQ to SQL (MSLinqToSQLGenerator)
            var tableAttribute = modelEntityType.GetCustomAttributes(typeof(TableAttribute), false)
                    .Cast<TableAttribute>()
                    .Single();

            using (var context = new DataContext())
            {
                var sche = tableAttribute.Schema;
                if (!string.IsNullOrEmpty(sche)) sche = sche + ".";
                LogHelper.Logger.Info("Truncate table: " + tableAttribute.Name);
                context.Database.ExecuteSqlCommand($"DELETE FROM {sche}{tableAttribute.Name}");
            }

            return new ResultInfo() { Success = false };
        }
        public ResultInfo TruncateTable() { return OnTruncateTable(); }

        #endregion

        //**------------------------------------------------------------

        #region Common

        #endregion
            
        //**------------------------------------------------------------

        #region Get Data
        //public virtual TEntity TryGet(object[] id)
        //{
        //    //var entry = new TEntity();
        //    //var param = Expression.Parameter(typeof(TEntity), "t");
        //    //var predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Property(param, entry.Base_PrimaryName()), Expression.Constant(id)), param);

        //    try
        //    {
        //        using (var context = new DataContext(DapperORM.Firebird()))
        //        {
        //            return context.Set<TEntity>().FilterByPrimaryKey(context, id).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}
        //public virtual Task<TEntity> TryGetAsyn(object[] id) => Task.Factory.StartNew(() => { return TryGet(id); });

        //**------------------------------------------------------------
        public virtual TEntity TryGet(Func<TEntity, bool> predicate)
        {
            using (var context = new DataContext())
            {
                var list = context.Set<TEntity>().ToArray();
                return context.Set<TEntity>().FirstOrDefault(predicate);
            }
        }
        public virtual Task<TEntity> TryGetAsyn(Func<TEntity, bool> predicate) => Task.Factory.StartNew(() => { return TryGet(predicate); });

        //**------------------------------------------------------------

        public virtual IList<TEntity> GetData(Func<TEntity, bool> predicate)
        {
            using (var context = new DataContext())
            {
                return context.Set<TEntity>().Where(predicate).ToList();
            }
        }
        //**------------------------------------------------------------

        public virtual bool CheckExist(Func<TEntity, bool> predicate)
        {
            using (var context = new DataContext())
            {
                return context.Set<TEntity>().Any(predicate);
            }
        }
        //**------------------------------------------------------------

        public virtual Task<IList<TEntity>> GetDataAsyn(Func<TEntity, bool> predicate) => Task.Factory.StartNew(() => { return GetData(predicate); });

        //**------------------------------------------------------------
        public virtual IList<TEntity> GetDataAll()
        {
            using (var context = new DataContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }
        public virtual Task<IList<TEntity>> GetDataAllAsyn(Func<TEntity, bool> predicate) => Task.Factory.StartNew(() => { return GetDataAll(); });
        #endregion

        //**------------------------------------------------------------

        #region Delete

        #endregion

        //**------------------------------------------------------------

        #region ExceMethod

        public ResultInfo ExecuteQuerySQL<T>(string tsql)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query<T>(tsql);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }
        }
        public ResultInfo ExecuteQuerySQL<T>(string tsql, DynamicParameters parameters)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query<T>(tsql, parameters, commandType: CommandType.Text);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }

        }
        public ResultInfo ExecuteQuery<T>(string tsql, DynamicParameters parameters, CommandType type)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query<T>(tsql, parameters, commandType: type);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }
        }
        public ResultInfo ExecuteQuery(string tsql)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query(tsql);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }
        }
        public ResultInfo ExecuteQuery(string tsql, DynamicParameters parameters)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query(tsql, parameters, commandType: CommandType.Text);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }


        }
        public ResultInfo ExecuteQuery(string tsql, DynamicParameters parameters, CommandType command)
        {
            var result = new ResultInfo() { Success = false };
            try
            {
                using (var connect = new SqlConnection(DapperORM.Firebird()))
                {
                    result.Data = connect.Query(tsql, parameters, commandType: command);
                    result.Success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                LogHelper.Logger.Error(result.Message);
                return result;
            }
        }
        #endregion


        //**------------------------------------------------------------
    }
}
