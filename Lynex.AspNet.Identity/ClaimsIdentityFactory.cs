using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class ClaimsIdentityFactory<TUser, TKey> : IClaimsIdentityFactory<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		internal const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

		internal const string DefaultIdentityProviderClaimValue = "ASP.NET Identity";

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
			this.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			this.UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
			this.UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
			this.SecurityStampClaimType = "AspNet.Identity.SecurityStamp";
		}

		public virtual async Task<ClaimsIdentity> CreateAsync(UserManager<TUser, TKey> manager, TUser user, string authenticationType)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(authenticationType, this.UserNameClaimType, this.RoleClaimType);
			claimsIdentity.AddClaim(new Claim(this.UserIdClaimType, this.ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(this.UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
			if (manager.SupportsUserSecurityStamp)
			{
				claimsIdentity.AddClaim(new Claim(this.SecurityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>()));
			}
			if (manager.SupportsUserRole)
			{
				IList<string> list = await manager.GetRolesAsync(user.Id).WithCurrentCulture<IList<string>>();
				foreach (string current in list)
				{
					claimsIdentity.AddClaim(new Claim(this.RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
				}
			}
			if (manager.SupportsUserClaim)
			{
				claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture<IList<Claim>>());
			}
			return claimsIdentity;
		}

		public virtual string ConvertIdToString(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return key.ToString();
		}
	}
	public class ClaimsIdentityFactory<TUser> : ClaimsIdentityFactory<TUser, string> where TUser : class, IUser<string>
	{
	}
}
