using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lynex.Common.Model.AspNet.Identity.Internal
{
	internal interface IEntityStore<TEntity>
	where TEntity : class
	{
		Task Delete(TEntity item);

		Task Delete(IEnumerable<TEntity> items);

		Task<IQueryable<TEntity>> Records();

		Task Save(TEntity item);

		Task Save(IEnumerable<TEntity> items);
	}
}