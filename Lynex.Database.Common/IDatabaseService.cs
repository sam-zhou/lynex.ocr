﻿using System;
using System.Collections.Generic;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Common.Database
{
    public interface IDatabaseService: IDisposable
    {
        void ResetDatabase();

        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();

        TEntity Get<TEntity>(IGetItemQuery<TEntity> itemQuery);
        IEnumerable<TEntity> Get<TEntity>(IGetItemsQuery<TEntity> query);
        void ExecuteQuery(IExecuteQuery query);


        TEntity Get<TEntity>(object entityId) where TEntity : class, IDbModel;
        TEntity Get<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        void Save<TEntity>(TEntity entity) where TEntity : class, IDbModel;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IDbModel;
    }
}
