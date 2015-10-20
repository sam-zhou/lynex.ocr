using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

namespace Lynex.AspNet.Identity.Owin
{
	public static class SecurityStampValidator
	{
		public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser>(TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentity) where TManager : UserManager<TUser, string> where TUser : class, IUser<string>
		{
			return SecurityStampValidator.OnValidateIdentity<TManager, TUser, string>(validateInterval, regenerateIdentity, (ClaimsIdentity id) => id.GetUserId());
		}

		public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser, TKey>(TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentityCallback, Func<ClaimsIdentity, TKey> getUserIdCallback) where TManager : UserManager<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (getUserIdCallback == null)
			{
				throw new ArgumentNullException("getUserIdCallback");
			}
			return async delegate(CookieValidateIdentityContext context)
			{
				DateTimeOffset utcNow = DateTimeOffset.UtcNow;
				if (context.Options != null && context.Options.SystemClock != null)
				{
					utcNow = context.Options.SystemClock.UtcNow;
				}
				DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;
				bool flag = !issuedUtc.HasValue;
				if (issuedUtc.HasValue)
				{
					TimeSpan t = utcNow.Subtract(issuedUtc.Value);
					flag = (t > validateInterval);
				}
				if (flag)
				{
					TManager userManager = context.OwinContext.GetUserManager<TManager>();
					TKey tKey = getUserIdCallback(context.Identity);
					if (userManager != null && tKey != null)
					{
						TUser tUser = await userManager.FindByIdAsync(tKey).WithCurrentCulture<TUser>();
						bool flag2 = true;
						if (tUser != null && userManager.SupportsUserSecurityStamp)
						{
							string a = context.Identity.FindFirstValue("AspNet.Identity.SecurityStamp");
							if (a == await userManager.GetSecurityStampAsync(tKey).WithCurrentCulture<string>())
							{
								flag2 = false;
								if (regenerateIdentityCallback != null)
								{
									ClaimsIdentity claimsIdentity = await regenerateIdentityCallback(userManager, tUser).WithCurrentCulture<ClaimsIdentity>();
									if (claimsIdentity != null)
									{
										context.Properties.IssuedUtc = null;
										context.Properties.ExpiresUtc = null;
										context.OwinContext.Authentication.SignIn(context.Properties, new ClaimsIdentity[]
										{
											claimsIdentity
										});
									}
								}
							}
						}
						if (flag2)
						{
							context.RejectIdentity();
							context.OwinContext.Authentication.SignOut(new string[]
							{
								context.Options.AuthenticationType
							});
						}
					}
				}
			};
		}
	}
}
