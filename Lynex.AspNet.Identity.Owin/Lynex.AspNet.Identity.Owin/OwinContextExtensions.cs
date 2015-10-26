using System;
using Microsoft.Owin;

namespace Lynex.AspNet.Identity.Owin
{
	public static class OwinContextExtensions
	{
		private static readonly string IdentityKeyPrefix = "AspNet.Identity.Owin:";

		private static string GetKey(Type t)
		{
			return IdentityKeyPrefix + t.AssemblyQualifiedName;
		}

		public static IOwinContext Set<T>(this IOwinContext context, T value)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}
			return context.Set(GetKey(typeof(T)), value);
		}

		public static T Get<T>(this IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}
			return context.Get<T>(GetKey(typeof(T)));
		}

		public static TManager GetUserManager<TManager>(this IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}
			return context.Get<TManager>();
		}
	}
}
