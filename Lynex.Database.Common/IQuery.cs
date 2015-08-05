using System.Collections.Generic;
using NHibernate;

namespace Lynex.Database.Common
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
