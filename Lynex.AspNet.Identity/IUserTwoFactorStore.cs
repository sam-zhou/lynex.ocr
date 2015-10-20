using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserTwoFactorStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task SetTwoFactorEnabledAsync(TUser user, bool enabled);

		Task<bool> GetTwoFactorEnabledAsync(TUser user);
	}
}
