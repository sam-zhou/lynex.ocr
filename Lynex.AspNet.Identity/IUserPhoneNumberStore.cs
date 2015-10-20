using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserPhoneNumberStore<TUser, in TKey> : IUserStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
	{
		Task SetPhoneNumberAsync(TUser user, string phoneNumber);

		Task<string> GetPhoneNumberAsync(TUser user);

		Task<bool> GetPhoneNumberConfirmedAsync(TUser user);

		Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed);
	}
	public interface IUserPhoneNumberStore<TUser> : IUserPhoneNumberStore<TUser, string>, IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
	{
	}
}
