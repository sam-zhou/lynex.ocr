using System.Collections.Generic;
using NHibernate;

namespace Lynex.Common.Database
{
    public interface IGetQuery<out TEntity> where TEntity : class
    {
        TEntity Execute(ISession session);
    }

    public interface IGetItemsQuery<out TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public interface IExecuteQuery
    {
        void Execute(ISession session);
    }
}
