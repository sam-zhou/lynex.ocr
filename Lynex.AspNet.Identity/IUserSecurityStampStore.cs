using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserSecurityStampStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task SetSecurityStampAsync(TUser user, string stamp);

		Task<string> GetSecurityStampAsync(TUser user);
	}
	public interface IUserSecurityStampStore<TUser> : IUserSecurityStampStore<TUser, string>, IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
	{
	}
}
