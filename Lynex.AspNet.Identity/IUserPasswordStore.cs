using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserPasswordStore<TUser, in TKey> : IUserStore<TUser, TKey> where TUser : class, IUser<TKey>
	{
		Task SetPasswordHashAsync(TUser user, string passwordHash);

		Task<string> GetPasswordHashAsync(TUser user);

		Task<bool> HasPasswordAsync(TUser user);
	}
	public interface IUserPasswordStore<TUser> : IUserPasswordStore<TUser, string> where TUser : class, IUser<string>
	{
	}
}
