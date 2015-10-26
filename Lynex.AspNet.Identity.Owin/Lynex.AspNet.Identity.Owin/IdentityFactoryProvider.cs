using System;
using Microsoft.Owin;

namespace Lynex.AspNet.Identity.Owin
{
	public class IdentityFactoryProvider<T> : IIdentityFactoryProvider<T> where T : class, IDisposable
	{
		public Func<IdentityFactoryOptions<T>, IOwinContext, T> OnCreate
		{
			get;
			set;
		}

		public Action<IdentityFactoryOptions<T>, T> OnDispose
		{
			get;
			set;
		}

		public IdentityFactoryProvider()
		{
			OnDispose = delegate(IdentityFactoryOptions<T> options, T instance)
			{ if (options == null) throw new ArgumentNullException(nameof(options)); };
			OnCreate = ((options, context) => default(T));
		}

		public virtual T Create(IdentityFactoryOptions<T> options, IOwinContext context)
		{
			return OnCreate(options, context);
		}


	    public virtual void Dispose(IdentityFactoryOptions<T> options, T instance)
		{
			OnDispose(options, instance);
		}
	}
}
