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
				throw new ArgumentNullException(nameof(options));
			}
			if (options.Provider == null)
			{
				throw new ArgumentNullException(nameof(options.Provider));
			}
			Options = options;
		}

		public override async Task Invoke(IOwinContext context)
		{
			TOptions options = Options;
			TResult tResult = options.Provider.Create(Options, context);
			try
			{
				context.Set(tResult);
				if (Next != null)
				{
					await Next.Invoke(context);
				}
			}
			finally
			{
                Options.Provider.Dispose(Options, tResult);
			}
		}
	}
}
