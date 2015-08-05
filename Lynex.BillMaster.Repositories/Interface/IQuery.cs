using System.Collections.Generic;
using NHibernate;

namespace WCC.Repositories.Interface
{
    internal interface IGetQuery<out TEntity> where TEntity : class
    {
        TEntity Execute(ISession session);
    }

    internal interface IGetItemsQuery<out TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    internal interface IExecuteQuery
    {
        void Execute(ISession session);
    }
}
