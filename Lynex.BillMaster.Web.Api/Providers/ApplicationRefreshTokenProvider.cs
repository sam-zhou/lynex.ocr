using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Lynex.BillMaster.Web.Api.IoC;
using Lynex.Common.Extension;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Service.Interface;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace Lynex.BillMaster.Web.Api.Providers
{
    public class ApplicationRefreshTokenProvider: IAuthenticationTokenProvider
    {
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

        public void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var guid = Guid.NewGuid().ToString();

            // maybe only create a handle the first time, then re-use for same client
            var refreshToken = Crypto.Hash(guid);


            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                Id = refreshToken,
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();
            var result = AuthenticationService.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshToken);
            }
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Create(context);
            return Task.FromResult<object>(null); ;
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var refreshToken = AuthenticationService.FindRefreshToken(context.Token);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = AuthenticationService.RemoveRefreshToken(context.Token);
            }
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            Receive(context);
            return Task.FromResult<object>(null);
        }
    }
}
