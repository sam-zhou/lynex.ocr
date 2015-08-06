using System.Collections.Generic;
using System.Reflection;
using NHibernate;

namespace Lynex.Common.Database.DefaultDataFactory
{
    public interface IDefaultDataFactory
    {
        void Populate();
    }


    public abstract class DefaultDataFactoryBase<TEntity> : IDefaultDataFactory where TEntity : class
    {
        private readonly ISession _session;
        private readonly Assembly _assembly;

        protected DefaultDataFactoryBase(ISession session, Assembly assembly)
        {
            _session = session;
            _assembly = assembly;
        }

        protected abstract IEnumerable<TEntity> GetData(Assembly assembly = null);

        public void Populate()
        {
            var items = GetData(_assembly);

            foreach (var entity in items)
            {
                _session.Save(entity);
            }
        }
    }
}
