using System;
using System.Security.Claims;

namespace Lynex.AspNet.Identity.Owin
{
	public static class SignInManagerExtensions
	{
		public static ClaimsIdentity CreateUserIdentity<TUser, TKey>(this SignInManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CreateUserIdentityAsync(user));
		}

		public static void SignIn<TUser, TKey>(this SignInManager<TUser, TKey> manager, TUser user, bool isPersistent, bool rememberBrowser) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AsyncHelper.RunSync(() => manager.SignInAsync(user, isPersistent, rememberBrowser));
		}

		public static bool SendTwoFactorCode<TUser, TKey>(this SignInManager<TUser, TKey> manager, string provider) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SendTwoFactorCodeAsync(provider));
		}

		public static TKey GetVerifiedUserId<TUser, TKey>(this SignInManager<TUser, TKey> manager) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(manager.GetVerifiedUserIdAsync);
		}

		public static bool HasBeenVerified<TUser, TKey>(this SignInManager<TUser, TKey> manager) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(manager.HasBeenVerifiedAsync);
		}

		public static SignInStatus TwoFactorSignIn<TUser, TKey>(this SignInManager<TUser, TKey> manager, string provider, string code, bool isPersistent, bool rememberBrowser) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser));
		}

		public static SignInStatus ExternalSignIn<TUser, TKey>(this SignInManager<TUser, TKey> manager, ExternalLoginInfo loginInfo, bool isPersistent) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ExternalSignInAsync(loginInfo, isPersistent));
		}

		public static SignInStatus PasswordSignIn<TUser, TKey>(this SignInManager<TUser, TKey> manager, string userName, string password, bool isPersistent, bool shouldLockout) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.PasswordSignInAsync(userName, password, isPersistent, shouldLockout));
		}
	}
}
