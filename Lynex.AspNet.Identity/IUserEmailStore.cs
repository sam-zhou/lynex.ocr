using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserEmailStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task SetEmailAsync(TUser user, string email);

		Task<string> GetEmailAsync(TUser user);

		Task<bool> GetEmailConfirmedAsync(TUser user);

		Task SetEmailConfirmedAsync(TUser user, bool confirmed);

		Task<TUser> FindByEmailAsync(string email);
	}
	public interface IUserEmailStore<TUser> : IUserEmailStore<TUser, string>, IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
	{
	}
}
