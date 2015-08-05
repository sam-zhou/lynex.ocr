using System.Collections.Generic;
using NHibernate;

namespace WCC.Repositories.DefaultDataFactory
{
    internal interface IDefaultDataFactory
    {
        void Populate();
    }


    internal abstract class DefaultDataFactoryBase<TEntity> : IDefaultDataFactory where TEntity : class
    {
        private readonly ISession _session;

        protected DefaultDataFactoryBase(ISession session)
        {
            _session = session;
        }

        protected abstract IEnumerable<TEntity> GetData();

        public void Populate()
        {
            var items = GetData();

            foreach (var entity in items)
            {
                _session.Save(entity);
            }
        }
    }
}
