using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;


namespace Lynex.Common.Model.AspNet.Identity
{
	public class IdentityUser : IUser
	{
		public virtual IList<IdentityUserClaim> Claims
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

		public virtual IList<IdentityUserLogin> Logins
		{
			get;
			set;
		}

		public virtual string PasswordHash
		{
			get;
			set;
		}

		public virtual IList<IdentityRole> Roles
		{
			get;
			set;
		}

		public virtual string SecurityStamp
		{
			get;
			set;
		}

		public virtual string UserName
		{
			get;
			set;
		}

		public IdentityUser()
		{
			Id = Guid.NewGuid().ToString();
			Claims = new List<IdentityUserClaim>();
			Roles = new List<IdentityRole>();
			Logins = new List<IdentityUserLogin>();
		}

		public IdentityUser(string userName) : this()
		{
			UserName = userName;
		}
	}
}