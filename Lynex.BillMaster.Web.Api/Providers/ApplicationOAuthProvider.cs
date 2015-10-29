using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Web.Api.IoC;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Lynex.BillMaster.Web.Api.Models;
using Lynex.Common.Extension;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.Status;
using Lynex.Common.Service.Interface;
using Microsoft.AspNet.Identity.Owin;

namespace Lynex.BillMaster.Web.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        private IAuthenticationService _authenticationService;

        private IAuthenticationService AuthenticationService
        {
            get
            {
                if (_authenticationService == null)
                {
                    _authenticationService = IoCContainer.Resolve<IAuthenticationService>();
                }
                return _authenticationService;
            }
        }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null)
            {
                allowedOrigin = "*";
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", TokenStatus.Invalid.ToString());
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName, context.ClientId);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId, secret;
            if (context.TryGetFormCredentials(out clientId, out secret))
            {
                var client = AuthenticationService.FindClient(clientId);
                if (client == null)
                {
                    context.SetError("invalid_grant", TokenStatus.InvalidClientId.ToString());
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(secret))
                    {
                        context.SetError("invalid_grant", TokenStatus.MissingClientSecret.ToString());
                    }
                    else if(client.Secret != StringHelper.GetHash(secret))
                    {
                        context.SetError("invalid_grant", TokenStatus.InvalidClientSecret.ToString());
                    }
                    else if (!client.Active)
                    {
                        context.SetError("invalid_grant", TokenStatus.DisabledClient.ToString());
                    }
                    else
                    {
                        context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
                        context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
                        context.Validated();
                    }
                }
            }
            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];

            if (originalClient != context.ClientId)
            {
                context.SetError("invalid_grant", TokenStatus.InvalidClientId.ToString());
                
            }
            else
            {
                var newId = new ClaimsIdentity(context.Ticket.Identity);
                var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
                context.Validated(newTicket);
            }

            
            
            return Task.FromResult<object>(null); ;
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "as:client_id", clientId }
            };
            return new AuthenticationProperties(data);
        }
    }
}