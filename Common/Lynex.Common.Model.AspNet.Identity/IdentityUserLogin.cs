using System;
using System.ComponentModel.DataAnnotations;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class IdentityUserLogin
	{
		[Key]
		public virtual string Id
		{
			get;
			set;
		}

		public virtual string LoginProvider
		{
			get;
			set;
		}

		public virtual string ProviderKey
		{
			get;
			set;
		}

		public virtual IdentityUser User
		{
			get;
			set;
		}

		public IdentityUserLogin()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}