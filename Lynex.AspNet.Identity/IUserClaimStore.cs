using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IUserClaimStore<TUser, in TKey> : IUserStore<TUser, TKey> where TUser : class, IUser<TKey>
	{
		Task<IList<Claim>> GetClaimsAsync(TUser user);

		Task AddClaimAsync(TUser user, Claim claim);

		Task RemoveClaimAsync(TUser user, Claim claim);
	}
	public interface IUserClaimStore<TUser> : IUserClaimStore<TUser, string> where TUser : class, IUser<string>
	{
	}
}
