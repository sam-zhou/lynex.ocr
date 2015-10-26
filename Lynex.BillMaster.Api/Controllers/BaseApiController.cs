using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using log4net;
using Lynex.AspNet.Identity;
using Lynex.AspNet.Identity.Owin;
using Lynex.BillMaster.Api.Models;
using Lynex.BillMaster.Api.Providers;
using Lynex.BillMaster.Api.Results;
using Lynex.BillMaster.Exception.UserException;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Exception;
using Lynex.Common.Model.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using NHibernate.Util;

namespace Lynex.BillMaster.Api.Controllers
{
    [Authorize]
    //[ExceptionHandlingFilter]
    public abstract class BaseApiController : ApiController
    {
        private readonly ILog _log;
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public BaseApiController()
        {
        }

        public BaseApiController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            ILog log)
        {
            _log = log;
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }


        protected IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        
    }
}
