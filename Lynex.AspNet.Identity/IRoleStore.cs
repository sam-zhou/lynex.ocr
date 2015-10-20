using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IRoleStore<TRole, in TKey> : IDisposable where TRole : IRole<TKey>
	{
		Task CreateAsync(TRole role);

		Task UpdateAsync(TRole role);

		Task DeleteAsync(TRole role);

		Task<TRole> FindByIdAsync(TKey roleId);

		Task<TRole> FindByNameAsync(string roleName);
	}
	public interface IRoleStore<TRole> : IRoleStore<TRole, string>, IDisposable where TRole : IRole<string>
	{
	}
}
