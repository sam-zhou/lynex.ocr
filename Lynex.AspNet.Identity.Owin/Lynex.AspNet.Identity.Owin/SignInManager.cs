using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace Lynex.AspNet.Identity.Owin
{
	public class SignInManager<TUser, TKey> : IDisposable where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		private string _authType;

		public string AuthenticationType
		{
			get
			{
				return _authType ?? "ApplicationCookie";
			}
			set
			{
				_authType = value;
			}
		}

		public UserManager<TUser, TKey> UserManager
		{
			get;
			set;
		}

		public IAuthenticationManager AuthenticationManager
		{
			get;
			set;
		}

		public SignInManager(UserManager<TUser, TKey> userManager, IAuthenticationManager authenticationManager)
		{
			if (userManager == null)
			{
				throw new ArgumentNullException(nameof(userManager));
			}
			if (authenticationManager == null)
			{
				throw new ArgumentNullException(nameof(authenticationManager));
			}
			UserManager = userManager;
			AuthenticationManager = authenticationManager;
		}

		public virtual Task<ClaimsIdentity> CreateUserIdentityAsync(TUser user)
		{
			return UserManager.CreateIdentityAsync(user, AuthenticationType);
		}

		public virtual string ConvertIdToString(TKey id)
		{
			return Convert.ToString(id, CultureInfo.InvariantCulture);
		}

		public virtual TKey ConvertIdFromString(string id)
		{
			if (id == null)
			{
				return default(TKey);
			}
			return (TKey)Convert.ChangeType(id, typeof(TKey), CultureInfo.InvariantCulture);
		}

		public virtual async Task SignInAsync(TUser user, bool isPersistent, bool rememberBrowser)
		{
			ClaimsIdentity claimsIdentity = await CreateUserIdentityAsync(user).WithCurrentCulture();
			AuthenticationManager.SignOut("ExternalCookie", "TwoFactorCookie", "ApplicationCookie");
			if (rememberBrowser)
			{
				ClaimsIdentity claimsIdentity2 = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(ConvertIdToString(user.Id));
			    AuthenticationProperties authenticationProperties = new AuthenticationProperties {IsPersistent = isPersistent};
			    AuthenticationManager.SignIn(authenticationProperties, claimsIdentity, claimsIdentity2);
			}
			else
			{
			    AuthenticationProperties authenticationProperties2 = new AuthenticationProperties {IsPersistent = isPersistent};
			    AuthenticationManager.SignIn(authenticationProperties2, claimsIdentity);
			}
		}

		public virtual async Task<bool> SendTwoFactorCodeAsync(string provider)
		{
			TKey tKey = await GetVerifiedUserIdAsync().WithCurrentCulture();
			bool result;
			if (tKey == null)
			{
				result = false;
			}
			else
			{
				string token = await UserManager.GenerateTwoFactorTokenAsync(tKey, provider).WithCurrentCulture();
				await UserManager.NotifyTwoFactorTokenAsync(tKey, provider, token).WithCurrentCulture();
				result = true;
			}
			return result;
		}

		public async Task<TKey> GetVerifiedUserIdAsync()
		{
			AuthenticateResult authenticateResult = await AuthenticationManager.AuthenticateAsync("TwoFactorCookie").WithCurrentCulture();
			TKey result;
			if (authenticateResult != null && authenticateResult.Identity != null && !string.IsNullOrEmpty(authenticateResult.Identity.GetUserId()))
			{
				result = ConvertIdFromString(authenticateResult.Identity.GetUserId());
			}
			else
			{
				result = default(TKey);
			}
			return result;
		}

		public async Task<bool> HasBeenVerifiedAsync()
		{
			return await GetVerifiedUserIdAsync().WithCurrentCulture() != null;
		}

		public virtual async Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
		{
			TKey tKey = await GetVerifiedUserIdAsync().WithCurrentCulture();
			SignInStatus result;
			if (tKey == null)
			{
				result = SignInStatus.Failure;
			}
			else
			{
				TUser tUser = await UserManager.FindByIdAsync(tKey).WithCurrentCulture();
				if (tUser == null)
				{
					result = SignInStatus.Failure;
				}
				else if (await UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture())
				{
					result = SignInStatus.LockedOut;
				}
				else if (await UserManager.VerifyTwoFactorTokenAsync(tUser.Id, provider, code).WithCurrentCulture())
				{
					await UserManager.ResetAccessFailedCountAsync(tUser.Id).WithCurrentCulture();
					await SignInAsync(tUser, isPersistent, rememberBrowser).WithCurrentCulture();
					result = SignInStatus.Success;
				}
				else
				{
					await UserManager.AccessFailedAsync(tUser.Id).WithCurrentCulture();
					result = SignInStatus.Failure;
				}
			}
			return result;
		}

		public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent)
		{
			TUser tUser = await UserManager.FindAsync(loginInfo.Login).WithCurrentCulture();
			SignInStatus result;
			if (tUser == null)
			{
				result = SignInStatus.Failure;
			}
			else if (await UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture())
			{
				result = SignInStatus.LockedOut;
			}
			else
			{
				result = await SignInOrTwoFactor(tUser, isPersistent).WithCurrentCulture();
			}
			return result;
		}

		private async Task<SignInStatus> SignInOrTwoFactor(TUser user, bool isPersistent)
		{
			string text = Convert.ToString(user.Id);
			SignInStatus result;
			if (await UserManager.GetTwoFactorEnabledAsync(user.Id).WithCurrentCulture() && (await UserManager.GetValidTwoFactorProvidersAsync(user.Id).WithCurrentCulture()).Count > 0 && !(await AuthenticationManager.TwoFactorBrowserRememberedAsync(text).WithCurrentCulture()))
			{
				ClaimsIdentity claimsIdentity = new ClaimsIdentity("TwoFactorCookie");
				claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", text));
				AuthenticationManager.SignIn(claimsIdentity);
				result = SignInStatus.RequiresVerification;
			}
			else
			{
				await SignInAsync(user, isPersistent, false).WithCurrentCulture();
				result = SignInStatus.Success;
			}
			return result;
		}

		public virtual async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
		{
			SignInStatus result;
			if (UserManager == null)
			{
				result = SignInStatus.Failure;
			}
			else
			{
				TUser tUser = await UserManager.FindByNameAsync(userName).WithCurrentCulture();
				if (tUser == null)
				{
					result = SignInStatus.Failure;
				}
				else if (await UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture())
				{
					result = SignInStatus.LockedOut;
				}
				else if (await UserManager.CheckPasswordAsync(tUser, password).WithCurrentCulture())
				{
					await UserManager.ResetAccessFailedCountAsync(tUser.Id).WithCurrentCulture();
					result = await SignInOrTwoFactor(tUser, isPersistent).WithCurrentCulture();
				}
				else
				{
					if (shouldLockout)
					{
						await UserManager.AccessFailedAsync(tUser.Id).WithCurrentCulture();
						if (await UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture())
						{
							result = SignInStatus.LockedOut;
							return result;
						}
					}
					result = SignInStatus.Failure;
				}
			}
			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
