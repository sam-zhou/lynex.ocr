using System.Security.Claims;
using Lynex.AspNet.Identity;

namespace Lynex.AspNet.Identity.Owin
{
	public class ExternalLoginInfo
	{
		public UserLoginInfo Login
		{
			get;
			set;
		}

		public string DefaultUserName
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public ClaimsIdentity ExternalIdentity
		{
			get;
			set;
		}
	}
}
