using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class IdentityRole : IRole
	{
		[Key]
		public virtual string Id
		{
			get;
			set;
		}

		public virtual string Name
		{
			get;
			set;
		}

		public IdentityRole() : this("")
		{
		}

		public IdentityRole(string roleName)
		{
			this.Id = Guid.NewGuid().ToString();
			this.Name = roleName;
		}
	}
}