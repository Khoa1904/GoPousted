using GoShared.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GoShared.Events.GoPOSEventHandler;

namespace GoShared.Interface
{
    public interface IDataService<TEntity>
        where TEntity : class, IIdentifyEntity, new()
    {
        //**--------------------------------------------------------------------------
        event GeneralEventHandler<IList<TEntity>> DataSynchronized;
        event GeneralEventHandler<TEntity> Added;
        //**--------------------------------------------------------------------------
        ResultInfo SynchronizedData(IDictionary<string, string> paras);
        Task<ResultInfo> SynchronizedData(IDictionary<string, string> paras, bool singleResult, bool updateDb, object body);
        //**--------------------------------------------------------------------------
        ResultInfo Add(TEntity info, string userID);
        ResultInfo Add(DbContext context, TEntity info, string userID);
        Task<ResultInfo> AddAsyn(TEntity info, string userID);
        //TEntity TryGet(object[] id);
        //Task<TEntity> TryGetAsyn(object[] id);
        TEntity TryGet(Func<TEntity, bool> predicate);
        Task<TEntity> TryGetAsyn(Func<TEntity, bool> predicate);
        IList<TEntity> GetData(Func<TEntity, bool> predicate);
        bool CheckExist(Func<TEntity, bool> predicate);
        IList<TEntity> GetDataAll();
        //**--------------------------------------------------------------------------
    }
}
