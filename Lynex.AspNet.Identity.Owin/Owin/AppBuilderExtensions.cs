using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.AspNet.Identity.Owin;

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
					throw new ArgumentNullException("context");
				}
				if (context.Ticket.Identity.Claims.Any((Claim c) => c.Issuer != "LOCAL AUTHORITY"))
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
					throw new ArgumentNullException("context");
				}
				if (context.Ticket.Identity.Claims.Count<Claim>() == 0)
				{
					context.Rejected();
				}
				else if (context.Ticket.Identity.Claims.All((Claim c) => c.Issuer == "LOCAL AUTHORITY"))
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
				throw new ArgumentNullException("app");
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
				throw new ArgumentNullException("app");
			}
			if (createCallback == null)
			{
				throw new ArgumentNullException("createCallback");
			}
			if (disposeCallback == null)
			{
				throw new ArgumentNullException("disposeCallback");
			}
			app.Use(typeof(IdentityFactoryMiddleware<T, IdentityFactoryOptions<T>>), new object[]
			{
				new IdentityFactoryOptions<T>
				{
					DataProtectionProvider = app.GetDataProtectionProvider(),
					Provider = new IdentityFactoryProvider<T>
					{
						OnCreate = createCallback,
						OnDispose = disposeCallback
					}
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
				throw new ArgumentNullException("app");
			}
			AppBuilderSecurityExtensions.SetDefaultSignInAsAuthenticationType(app, externalAuthenticationType);
			CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions();
			cookieAuthenticationOptions.AuthenticationType = externalAuthenticationType;
			cookieAuthenticationOptions.AuthenticationMode = AuthenticationMode.Passive;
			cookieAuthenticationOptions.CookieName = ".AspNet." + externalAuthenticationType;
			cookieAuthenticationOptions.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
			CookieAuthenticationExtensions.UseCookieAuthentication(app, cookieAuthenticationOptions);
		}

		public static void UseTwoFactorSignInCookie(this IAppBuilder app, string authenticationType, TimeSpan expires)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions();
			cookieAuthenticationOptions.AuthenticationType = authenticationType;
            cookieAuthenticationOptions.AuthenticationMode = AuthenticationMode.Passive;
            cookieAuthenticationOptions.CookieName = ".AspNet." + authenticationType;
			cookieAuthenticationOptions.ExpireTimeSpan = expires;
			CookieAuthenticationExtensions.UseCookieAuthentication(app, cookieAuthenticationOptions);
		}

		public static void UseTwoFactorRememberBrowserCookie(this IAppBuilder app, string authenticationType)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions();
			cookieAuthenticationOptions.AuthenticationType = authenticationType;
			cookieAuthenticationOptions.AuthenticationMode = AuthenticationMode.Passive;
			cookieAuthenticationOptions.CookieName = ".AspNet." + authenticationType;
			CookieAuthenticationExtensions.UseCookieAuthentication(app, cookieAuthenticationOptions);
		}

		public static void UseOAuthBearerTokens(this IAppBuilder app, OAuthAuthorizationServerOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			OAuthAuthorizationServerExtensions.UseOAuthAuthorizationServer(app, options);
			OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions = new OAuthBearerAuthenticationOptions();
			oAuthBearerAuthenticationOptions.AccessTokenFormat = options.AccessTokenFormat;
			oAuthBearerAuthenticationOptions.AccessTokenProvider = options.AccessTokenProvider;
			oAuthBearerAuthenticationOptions.AuthenticationMode = options.AuthenticationMode;
			oAuthBearerAuthenticationOptions.AuthenticationType = options.AuthenticationType;
			oAuthBearerAuthenticationOptions.Description = options.Description;
			oAuthBearerAuthenticationOptions.Provider = new AppBuilderExtensions.ApplicationOAuthBearerProvider();
			oAuthBearerAuthenticationOptions.SystemClock = options.SystemClock;
			OAuthBearerAuthenticationExtensions.UseOAuthBearerAuthentication(app, oAuthBearerAuthenticationOptions);
			OAuthBearerAuthenticationOptions oAuthBearerAuthenticationOptions2 = new OAuthBearerAuthenticationOptions();
			oAuthBearerAuthenticationOptions2.AccessTokenFormat = options.AccessTokenFormat;
			oAuthBearerAuthenticationOptions2.AccessTokenProvider = options.AccessTokenProvider;
			oAuthBearerAuthenticationOptions2.AuthenticationMode = AuthenticationMode.Passive;
			oAuthBearerAuthenticationOptions2.AuthenticationType = "ExternalBearer";
			oAuthBearerAuthenticationOptions2.Description = options.Description;
			oAuthBearerAuthenticationOptions2.Provider = new AppBuilderExtensions.ExternalOAuthBearerProvider();
			oAuthBearerAuthenticationOptions2.SystemClock = options.SystemClock;
			OAuthBearerAuthenticationExtensions.UseOAuthBearerAuthentication(app, oAuthBearerAuthenticationOptions2);
		}
	}
}
