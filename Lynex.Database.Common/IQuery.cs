using System.Collections.Generic;
using NHibernate;

namespace Lynex.Common.Database
{
    public interface IGetItemQuery<out TEntity>
    {
        TEntity Execute(ISession session);
    }

    public interface IGetItemsQuery<out TEntity>
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public interface IExecuteQuery
    {
        void Execute(ISession session);
    }
}
