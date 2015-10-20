using System;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserTokenProvider<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		Task<string> GenerateAsync(string purpose, UserManager<TUser, TKey> manager, TUser user);

		Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, TKey> manager, TUser user);

		Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user);

		Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user);
	}
}
