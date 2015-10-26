using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace Lynex.Common.Model.AspNet.Identity.Internal
{
	[ExcludeFromCodeCoverage]
	internal class EntityStore<TEntity> : IEntityStore<TEntity>
	where TEntity : class
	{
		private readonly ISession _session;

		public EntityStore(ISession session)
		{
			_session = session;
		}

		public async Task Delete(TEntity item)
		{
			await Task.Run(() => {
				_session.Delete(item);
				_session.Flush();
			});
		}

		public async Task Delete(IEnumerable<TEntity> items)
		{
			await Task.Run(() => {
				using (ITransaction transaction = _session.BeginTransaction())
				{
					foreach (TEntity item in items)
					{
						_session.Delete(item);
					}
					transaction.Commit();
				}
			});
		}

		public async Task<IQueryable<TEntity>> Records()
		{
			IQueryable<TEntity> tEntities = await Task.Run(() => _session.Query<TEntity>().AsQueryable());
			return tEntities;
		}

		public async Task Save(TEntity item)
		{
			await Task.Run(() => {
				_session.Clear();
				_session.SaveOrUpdate(item);
				_session.Flush();
			});
		}

		public async Task Save(IEnumerable<TEntity> items)
		{
			await Task.Run(() => {
				_session.Clear();
				using (ITransaction transaction = _session.BeginTransaction())
				{
					foreach (TEntity item in items)
					{
						_session.SaveOrUpdate(item);
					}
					transaction.Commit();
				}
			});
		}
	}
}