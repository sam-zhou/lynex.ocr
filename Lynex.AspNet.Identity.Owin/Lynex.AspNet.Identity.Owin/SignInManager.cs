using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
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
				return this._authType ?? "ApplicationCookie";
			}
			set
			{
				this._authType = value;
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
			this.UserManager = userManager;
			this.AuthenticationManager = authenticationManager;
		}

		public virtual Task<ClaimsIdentity> CreateUserIdentityAsync(TUser user)
		{
			return this.UserManager.CreateIdentityAsync(user, this.AuthenticationType);
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
			return (TKey)((object)Convert.ChangeType(id, typeof(TKey), CultureInfo.InvariantCulture));
		}

		public virtual async Task SignInAsync(TUser user, bool isPersistent, bool rememberBrowser)
		{
			ClaimsIdentity claimsIdentity = await this.CreateUserIdentityAsync(user).WithCurrentCulture<ClaimsIdentity>();
			this.AuthenticationManager.SignOut(new string[]
			{
				"ExternalCookie",
				"TwoFactorCookie",
                "ApplicationCookie"
            });
			if (rememberBrowser)
			{
				ClaimsIdentity claimsIdentity2 = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(this.ConvertIdToString(user.Id));
				AuthenticationProperties authenticationProperties = new AuthenticationProperties();
			    authenticationProperties.IsPersistent = isPersistent;
                AuthenticationManager.SignIn(authenticationProperties, new ClaimsIdentity[]
				{
					claimsIdentity,
					claimsIdentity2
				});
			}
			else
			{
				AuthenticationProperties authenticationProperties2 = new AuthenticationProperties();
				authenticationProperties2.IsPersistent = isPersistent;
                AuthenticationManager.SignIn(authenticationProperties2, new ClaimsIdentity[]
				{
					claimsIdentity
				});
			}
		}

		public virtual async Task<bool> SendTwoFactorCodeAsync(string provider)
		{
			TKey tKey = await this.GetVerifiedUserIdAsync().WithCurrentCulture<TKey>();
			bool result;
			if (tKey == null)
			{
				result = false;
			}
			else
			{
				string token = await this.UserManager.GenerateTwoFactorTokenAsync(tKey, provider).WithCurrentCulture<string>();
				await this.UserManager.NotifyTwoFactorTokenAsync(tKey, provider, token).WithCurrentCulture<IdentityResult>();
				result = true;
			}
			return result;
		}

		public async Task<TKey> GetVerifiedUserIdAsync()
		{
			AuthenticateResult authenticateResult = await this.AuthenticationManager.AuthenticateAsync("TwoFactorCookie").WithCurrentCulture<AuthenticateResult>();
			TKey result;
			if (authenticateResult != null && authenticateResult.Identity != null && !string.IsNullOrEmpty(authenticateResult.Identity.GetUserId()))
			{
				result = this.ConvertIdFromString(authenticateResult.Identity.GetUserId());
			}
			else
			{
				result = default(TKey);
			}
			return result;
		}

		public async Task<bool> HasBeenVerifiedAsync()
		{
			return await this.GetVerifiedUserIdAsync().WithCurrentCulture<TKey>() != null;
		}

		public virtual async Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
		{
			TKey tKey = await this.GetVerifiedUserIdAsync().WithCurrentCulture<TKey>();
			SignInStatus result;
			if (tKey == null)
			{
				result = SignInStatus.Failure;
			}
			else
			{
				TUser tUser = await this.UserManager.FindByIdAsync(tKey).WithCurrentCulture<TUser>();
				if (tUser == null)
				{
					result = SignInStatus.Failure;
				}
				else if (await this.UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture<bool>())
				{
					result = SignInStatus.LockedOut;
				}
				else if (await this.UserManager.VerifyTwoFactorTokenAsync(tUser.Id, provider, code).WithCurrentCulture<bool>())
				{
					await this.UserManager.ResetAccessFailedCountAsync(tUser.Id).WithCurrentCulture<IdentityResult>();
					await this.SignInAsync(tUser, isPersistent, rememberBrowser).WithCurrentCulture();
					result = SignInStatus.Success;
				}
				else
				{
					await this.UserManager.AccessFailedAsync(tUser.Id).WithCurrentCulture<IdentityResult>();
					result = SignInStatus.Failure;
				}
			}
			return result;
		}

		public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent)
		{
			TUser tUser = await this.UserManager.FindAsync(loginInfo.Login).WithCurrentCulture<TUser>();
			SignInStatus result;
			if (tUser == null)
			{
				result = SignInStatus.Failure;
			}
			else if (await this.UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture<bool>())
			{
				result = SignInStatus.LockedOut;
			}
			else
			{
				result = await this.SignInOrTwoFactor(tUser, isPersistent).WithCurrentCulture<SignInStatus>();
			}
			return result;
		}

		private async Task<SignInStatus> SignInOrTwoFactor(TUser user, bool isPersistent)
		{
			string text = Convert.ToString(user.Id);
			SignInStatus result;
			if (await this.UserManager.GetTwoFactorEnabledAsync(user.Id).WithCurrentCulture<bool>() && (await this.UserManager.GetValidTwoFactorProvidersAsync(user.Id).WithCurrentCulture<IList<string>>()).Count > 0 && !(await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(text).WithCurrentCulture<bool>()))
			{
				ClaimsIdentity claimsIdentity = new ClaimsIdentity("TwoFactorCookie");
				claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", text));
				this.AuthenticationManager.SignIn(new ClaimsIdentity[]
				{
					claimsIdentity
				});
				result = SignInStatus.RequiresVerification;
			}
			else
			{
				await this.SignInAsync(user, isPersistent, false).WithCurrentCulture();
				result = SignInStatus.Success;
			}
			return result;
		}

		public virtual async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
		{
			SignInStatus result;
			if (this.UserManager == null)
			{
				result = SignInStatus.Failure;
			}
			else
			{
				TUser tUser = await this.UserManager.FindByNameAsync(userName).WithCurrentCulture<TUser>();
				if (tUser == null)
				{
					result = SignInStatus.Failure;
				}
				else if (await this.UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture<bool>())
				{
					result = SignInStatus.LockedOut;
				}
				else if (await this.UserManager.CheckPasswordAsync(tUser, password).WithCurrentCulture<bool>())
				{
					await this.UserManager.ResetAccessFailedCountAsync(tUser.Id).WithCurrentCulture<IdentityResult>();
					result = await this.SignInOrTwoFactor(tUser, isPersistent).WithCurrentCulture<SignInStatus>();
				}
				else
				{
					if (shouldLockout)
					{
						await this.UserManager.AccessFailedAsync(tUser.Id).WithCurrentCulture<IdentityResult>();
						if (await this.UserManager.IsLockedOutAsync(tUser.Id).WithCurrentCulture<bool>())
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
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
