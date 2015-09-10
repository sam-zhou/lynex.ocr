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
			this._session = session;
		}

		public async Task Delete(TEntity item)
		{
			await Task.Run(() => {
				this._session.Delete(item);
				this._session.Flush();
			});
		}

		public async Task Delete(IEnumerable<TEntity> items)
		{
			await Task.Run(() => {
				using (ITransaction transaction = this._session.BeginTransaction())
				{
					foreach (TEntity item in items)
					{
						this._session.Delete(item);
					}
					transaction.Commit();
				}
			});
		}

		public async Task<IQueryable<TEntity>> Records()
		{
			IQueryable<TEntity> tEntities = await Task.Run<IQueryable<TEntity>>(() => LinqExtensionMethods.Query<TEntity>(this._session).AsQueryable<TEntity>());
			return tEntities;
		}

		public async Task Save(TEntity item)
		{
			await Task.Run(() => {
				this._session.Clear();
				this._session.SaveOrUpdate(item);
				this._session.Flush();
			});
		}

		public async Task Save(IEnumerable<TEntity> items)
		{
			await Task.Run(() => {
				this._session.Clear();
				using (ITransaction transaction = this._session.BeginTransaction())
				{
					foreach (TEntity item in items)
					{
						this._session.SaveOrUpdate(item);
					}
					transaction.Commit();
				}
			});
		}
	}
}