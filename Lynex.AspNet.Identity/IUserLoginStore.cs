using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserLoginStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task AddLoginAsync(TUser user, UserLoginInfo login);

		Task RemoveLoginAsync(TUser user, UserLoginInfo login);

		Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);

		Task<TUser> FindAsync(UserLoginInfo login);
	}
	public interface IUserLoginStore<TUser> : IUserLoginStore<TUser, string>, IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
	{
	}
}
