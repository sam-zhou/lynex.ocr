using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class TotpSecurityStampBasedTokenProvider<TUser, TKey> : IUserTokenProvider<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		public virtual Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult(0);
		}

		public virtual Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return Task.FromResult(manager.SupportsUserSecurityStamp);
		}

		public virtual async Task<string> GenerateAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			SecurityToken securityToken = await manager.CreateSecurityTokenAsync(user.Id).WithCurrentCulture();
			string modifier = await GetUserModifierAsync(purpose, manager, user).WithCurrentCulture();
			return Rfc6238AuthenticationService.GenerateCode(securityToken, modifier).ToString("D6", CultureInfo.InvariantCulture);
		}

		public virtual async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, TKey> manager, TUser user)
		{
			int code;
			bool result;
			if (!int.TryParse(token, out code))
			{
				result = false;
			}
			else
			{
				SecurityToken securityToken = await manager.CreateSecurityTokenAsync(user.Id).WithCurrentCulture();
				string modifier = await GetUserModifierAsync(purpose, manager, user).WithCurrentCulture();
				result = (securityToken != null && Rfc6238AuthenticationService.ValidateCode(securityToken, code, modifier));
			}
			return result;
		}

		public virtual Task<string> GetUserModifierAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult(string.Concat("Totp:", purpose, ":", user.Id));
		}
	}
}
