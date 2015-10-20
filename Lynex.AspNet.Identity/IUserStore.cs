using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserStore<TUser, in TKey> : IDisposable where TUser : class, IUser<TKey>
	{
		Task CreateAsync(TUser user);

		Task UpdateAsync(TUser user);

		Task DeleteAsync(TUser user);

		Task<TUser> FindByIdAsync(TKey userId);

		Task<TUser> FindByNameAsync(string userName);
	}
	public interface IUserStore<TUser> : IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
	{
	}
}
