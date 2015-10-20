using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserLockoutStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user);

		Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd);

		Task<int> IncrementAccessFailedCountAsync(TUser user);

		Task ResetAccessFailedCountAsync(TUser user);

		Task<int> GetAccessFailedCountAsync(TUser user);

		Task<bool> GetLockoutEnabledAsync(TUser user);

		Task SetLockoutEnabledAsync(TUser user, bool enabled);
	}
}
