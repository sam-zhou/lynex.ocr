using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace Lynex.AspNet.Identity
{
	public static class IdentityExtensions
	{
		public static string GetUserName(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				return claimsIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
			}
			return null;
		}

		public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				string text = claimsIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
				if (text != null)
				{
					return (T)((object)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture));
				}
			}
			return default(T);
		}

		public static string GetUserId(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				return claimsIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
			}
			return null;
		}

		public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			Claim claim = identity.FindFirst(claimType);
			if (claim == null)
			{
				return null;
			}
			return claim.Value;
		}
	}
}
