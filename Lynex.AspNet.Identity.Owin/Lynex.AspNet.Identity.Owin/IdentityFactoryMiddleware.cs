using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Lynex.AspNet.Identity.Owin
{
	public class IdentityFactoryMiddleware<TResult, TOptions> : OwinMiddleware where TResult : IDisposable where TOptions : IdentityFactoryOptions<TResult>
	{
		public TOptions Options
		{
			get;
			private set;
		}

		public IdentityFactoryMiddleware(OwinMiddleware next, TOptions options) : base(next)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (options.Provider == null)
			{
				throw new ArgumentNullException("options.Provider");
			}
			this.Options = options;
		}

		public override async Task Invoke(IOwinContext context)
		{
			TOptions options = this.Options;
			TResult tResult = options.Provider.Create(this.Options, context);
			try
			{
				context.Set(tResult);
				if (base.Next != null)
				{
					await base.Next.Invoke(context);
				}
			}
			finally
			{
				TOptions var_7_FB = this.Options;
				var_7_FB.Provider.Dispose(this.Options, tResult);
			}
		}
	}
}
