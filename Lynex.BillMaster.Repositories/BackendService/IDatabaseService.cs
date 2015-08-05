using System;
using System.Collections.Generic;
using WCC.Model.Domain.DbModels.Interface;
using WCC.Repositories.Interface;

namespace WCC.Repositories.BackendService
{
    internal interface IDatabaseService : IDisposable
    {
        void ResetDatabase();

        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();

        TEntity Get<TEntity>(IGetQuery<TEntity> query) where TEntity : class;
        IEnumerable<TEntity> Get<TEntity>(IGetItemsQuery<TEntity> query) where TEntity : class;
        void ExecuteQuery(IExecuteQuery query);


        TEntity Get<TEntity>(long entityId) where TEntity : class, IBaseEntity;
        TEntity Get<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        void Save<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    }
}
