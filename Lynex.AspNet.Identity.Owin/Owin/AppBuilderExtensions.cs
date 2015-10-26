using System;
using System.Linq;
using System.Threading.Tasks;
using Lynex.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;

namespace Owin
{
	public static class AppBuilderExtensions
	{
		private class ApplicationOAuthBearerProvider : OAuthBearerAuthenticationProvider
		{
			public override Task ValidateIdentity(OAuthValidateIdentityContext context)
			{
				if (context == null)
				{
					throw new ArgumentNullException(nameof(context));
				}
				if (context.Ticket.Identity.Claims.Any(c => c.Issuer != "LOCAL AUTHORITY"))
				{
					context.Rejected();
				}
				return Task.FromResult<object>(null);
			}
		}

		private class ExternalOAuthBearerProvider : OAuthBearerAuthenticationProvider
		{
			public override Task ValidateIdentity(OAuthValidateIdentityContext context)
			{
				if (context == null)
				{
					throw new ArgumentNullException(nameof(context));
				}
				if (!context.Ticket.Identity.Claims.Any())
				{
					context.Rejected();
				}
				else if (context.Ticket.Identity.Claims.All(c => c.Issuer == "LOCAL AUTHORITY"))
				{
					context.Rejected();
				}
				return Task.FromResult<object>(null);
			}
		}

		private const string CookiePrefix = ".AspNet.";

		public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<T> createCallback) where T : class, IDisposable
		{
			return app.CreatePerOwinContext((IdentityFactoryOptions<T> options, IOwinContext context) => createCallback());
		}

		public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<IdentityFactoryOptions<T>, IOwinContext, T> createCallback) where T : class, IDisposable
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
			return app.CreatePerOwinContext(createCallback, delegate(IdentityFactoryOptions<T> options, T instance)
			{
				instance.Dispose();
			});
		}

		public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<IdentityFactoryOptions<T>, IOwinContext, T> createCallback, Action<IdentityFactoryOptions<T>, T> disposeCallback) where T : class, IDisposable
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
			if (createCallback == null)
			{
				throw new ArgumentNullException(nameof(createCallback));
			}
			if (disposeCallback == null)
			{
				throw new ArgumentNullException(nameof(disposeCallback));
			}
			app.Use(typeof(IdentityFactoryMiddleware<T, IdentityFactoryOptions<T>>), new IdentityFactoryOptions<T>
			{
			    DataProtectionProvider = app.GetDataProtectionProvider(),
			    Provider = new IdentityFactoryProvider<T>
			    {
			        OnCreate = createCallback,
			        OnDispose = disposeCallback
			    }
			});
			return app;
		}

		public static void UseExternalSignInCookie(this IAppBuilder app)
		{
			app.UseExternalSignInCookie("ExternalCookie");
		}

		public static void UseExternalSignInCookie(this IAppBuilder app, string externalAuthenticationType)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
			app.SetDefaultSignInAsAuthenticationType(externalAuthenticationType);
		    CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions
		    {
		        AuthenticationType = externalAuthenticationType,
		        AuthenticationMode = AuthenticationMode.Passive,
		        CookieName = CookiePrefix + externalAuthenticationType,
		        ExpireTimeSpan = TimeSpan.FromMinutes(5.0)
		    };
		    app.UseCookieAuthentication(cookieAuthenticationOptions);
		}

		public static void UseTwoFactorSignInCookie(this IAppBuilder app, string authenticationType, TimeSpan expires)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
		    CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions
		    {
		        AuthenticationType = authenticationType,
		        AuthenticationMode = AuthenticationMode.Passive,
		        CookieName = CookiePrefix + authenticationType,
		        ExpireTimeSpan = expires
		    };
		    app.UseCookieAuthentication(cookieAuthenticationOptions);
		}

		public static void UseTwoFactorRememberBrowserCookie(this IAppBuilder app, string authenticationType)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
		    CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions
		    {
		        AuthenticationType = authenticationType,
		        AuthenticationMode = AuthenticationMode.Passive,
		        CookieName = CookiePrefix + authenticationType
		    };
		    app.UseCookieAuthentication(cookieAuthenticationOptions);
		}

		public static void UseOAuthBearerTokens(this IAppBuilder app, OAuthAuthorizationServerOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			app.UseOAuthAuthorizationServer(options);
		    OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions = new OAuthBearerAuthenticationOptions
		    {
		        AccessTokenFormat = options.AccessTokenFormat,
		        AccessTokenProvider = options.AccessTokenProvider,
		        AuthenticationMode = options.AuthenticationMode,
		        AuthenticationType = options.AuthenticationType,
		        Description = options.Description,
		        Provider = new ApplicationOAuthBearerProvider(),
		        SystemClock = options.SystemClock
		    };
		    app.UseOAuthBearerAuthentication(oAuthBearerAuthenticationOptions);
		    OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions2 = new OAuthBearerAuthenticationOptions
		    {
		        AccessTokenFormat = options.AccessTokenFormat,
		        AccessTokenProvider = options.AccessTokenProvider,
		        AuthenticationMode = AuthenticationMode.Passive,
		        AuthenticationType = "ExternalBearer",
		        Description = options.Description,
		        Provider = new ExternalOAuthBearerProvider(),
		        SystemClock = options.SystemClock
		    };
		    app.UseOAuthBearerAuthentication(oAuthBearerAuthenticationOptions2);
		}
	}
}
