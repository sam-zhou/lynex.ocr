using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Lynex.AspNet.Identity;
using Lynex.AspNet.Identity.Owin;
using Lynex.BillMaster.Api.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Lynex.BillMaster.Api.Controllers
{
    [RoutePrefix("Authentication")]
    //[IdentityBasicAuthentication] // Enable authentication via an ASP.NET Identity user name and password
    public class AuthenticationController : BaseApiController
    {
        public AuthenticationController()
        {
        }

        public AuthenticationController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            ILog log):base(userManager, accessTokenFormat, log)
        {
            
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("SignIn")]
        public async Task<IHttpActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    var userIdentity = await user.GenerateUserIdentityAsync(UserManager, OAuthDefaults.AuthenticationType);
                    Authentication.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, userIdentity);
                    return Ok(user);
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return Unauthorized();
        }

        // POST Account/Logout
        [Route("SignOut")]
        public IHttpActionResult SignOut()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }
    }
}
