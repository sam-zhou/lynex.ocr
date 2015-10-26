using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Lynex.AspNet.Identity.Owin;

namespace Microsoft.Owin.Security
{
	public static class AuthenticationManagerExtensions
	{
		public static IEnumerable<AuthenticationDescription> GetExternalAuthenticationTypes(this IAuthenticationManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return manager.GetAuthenticationTypes(d => d.Properties != null && d.Properties.ContainsKey("Caption"));
		}

		public static async Task<ClaimsIdentity> GetExternalIdentityAsync(this IAuthenticationManager manager, string externalAuthenticationType)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AuthenticateResult authenticateResult = await manager.AuthenticateAsync(externalAuthenticationType).WithCurrentCulture();
			ClaimsIdentity result;
			if (authenticateResult != null && authenticateResult.Identity != null && authenticateResult.Identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") != null)
			{
				result = authenticateResult.Identity;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static ClaimsIdentity GetExternalIdentity(this IAuthenticationManager manager, string externalAuthenticationType)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetExternalIdentityAsync(externalAuthenticationType));
		}

		private static ExternalLoginInfo GetExternalLoginInfo(AuthenticateResult result)
		{
			if (result == null || result.Identity == null)
			{
				return null;
			}
			Claim claim = result.Identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
			if (claim == null)
			{
				return null;
			}
			string text = result.Identity.Name;
			if (text != null)
			{
				text = text.Replace(" ", "");
			}
			string email = result.Identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
			return new ExternalLoginInfo
			{
				ExternalIdentity = result.Identity,
				Login = new UserLoginInfo(claim.Issuer, claim.Value),
				DefaultUserName = text,
				Email = email
			};
		}

		public static async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(this IAuthenticationManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return GetExternalLoginInfo(await manager.AuthenticateAsync("ExternalCookie").WithCurrentCulture());
		}

		public static ExternalLoginInfo GetExternalLoginInfo(this IAuthenticationManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(manager.GetExternalLoginInfoAsync);
		}

		public static ExternalLoginInfo GetExternalLoginInfo(this IAuthenticationManager manager, string xsrfKey, string expectedValue)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetExternalLoginInfoAsync(xsrfKey, expectedValue));
		}

		public static async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(this IAuthenticationManager manager, string xsrfKey, string expectedValue)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AuthenticateResult authenticateResult = await manager.AuthenticateAsync("ExternalCookie").WithCurrentCulture();
			ExternalLoginInfo result;
			if (authenticateResult != null && authenticateResult.Properties != null && authenticateResult.Properties.Dictionary != null && authenticateResult.Properties.Dictionary.ContainsKey(xsrfKey) && authenticateResult.Properties.Dictionary[xsrfKey] == expectedValue)
			{
				result = GetExternalLoginInfo(authenticateResult);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static async Task<bool> TwoFactorBrowserRememberedAsync(this IAuthenticationManager manager, string userId)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AuthenticateResult authenticateResult = await manager.AuthenticateAsync("TwoFactorRememberBrowser").WithCurrentCulture();
			return authenticateResult != null && authenticateResult.Identity != null && authenticateResult.Identity.GetUserId() == userId;
		}

		public static bool TwoFactorBrowserRemembered(this IAuthenticationManager manager, string userId)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.TwoFactorBrowserRememberedAsync(userId));
		}

		public static ClaimsIdentity CreateTwoFactorRememberBrowserIdentity(this IAuthenticationManager manager, string userId)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity("TwoFactorRememberBrowser");
			claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));
			return claimsIdentity;
		}
	}
}
