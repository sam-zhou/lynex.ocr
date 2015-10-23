using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserSecurityStampStore<TUser, in TKey> : IUserStore<TUser, TKey> where TUser : class, IUser<TKey>
	{
		Task SetSecurityStampAsync(TUser user, string stamp);

		Task<string> GetSecurityStampAsync(TUser user);
	}
	public interface IUserSecurityStampStore<TUser> : IUserSecurityStampStore<TUser, string> where TUser : class, IUser<string>
	{
	}
}
