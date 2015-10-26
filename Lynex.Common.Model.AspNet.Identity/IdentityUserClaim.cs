using System;
using System.ComponentModel.DataAnnotations;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class IdentityUserClaim
	{
		public virtual string ClaimType
		{
			get;
			set;
		}

		public virtual string ClaimValue
		{
			get;
			set;
		}

		[Key]
		public virtual string Id
		{
			get;
			set;
		}

		public virtual IdentityUser User
		{
			get;
			set;
		}

		public IdentityUserClaim()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}