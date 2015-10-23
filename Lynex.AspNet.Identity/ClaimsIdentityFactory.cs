using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class ClaimsIdentityFactory<TUser, TKey> : IClaimsIdentityFactory<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		internal const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

		internal const string DefaultIdentityProviderClaimValue = "Lynex Identity";

		public string RoleClaimType
		{
			get;
			set;
		}

		public string UserNameClaimType
		{
			get;
			set;
		}

		public string UserIdClaimType
		{
			get;
			set;
		}

		public string SecurityStampClaimType
		{
			get;
			set;
		}

		public ClaimsIdentityFactory()
		{
			RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
			UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
			SecurityStampClaimType = "AspNet.Identity.SecurityStamp";
		}

		public virtual async Task<ClaimsIdentity> CreateAsync(UserManager<TUser, TKey> manager, TUser user, string authenticationType)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);
			claimsIdentity.AddClaim(new Claim(UserIdClaimType, ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Lynex Identity", "http://www.w3.org/2001/XMLSchema#string"));
            if (manager.SupportsUserSecurityStamp)
			{
				claimsIdentity.AddClaim(new Claim(SecurityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture()));
			}
			if (manager.SupportsUserRole)
			{
				IList<string> list = await manager.GetRolesAsync(user.Id).WithCurrentCulture();
				foreach (string current in list)
				{
					claimsIdentity.AddClaim(new Claim(RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
				}
			}
			if (manager.SupportsUserClaim)
			{
				claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture());
			}
			return claimsIdentity;
		}

		public virtual string ConvertIdToString(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			return key.ToString();
		}
	}
	public class ClaimsIdentityFactory<TUser> : ClaimsIdentityFactory<TUser, string> where TUser : class, IUser<string>
	{
	}
}
