using System;
using Microsoft.Owin.Security.DataProtection;

namespace Lynex.AspNet.Identity.Owin
{
	public class IdentityFactoryOptions<T> where T : IDisposable
	{
		public IDataProtectionProvider DataProtectionProvider
		{
			get;
			set;
		}

		public IIdentityFactoryProvider<T> Provider
		{
			get;
			set;
		}
	}
}
