using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lynex.AspNet.Identity;
using Lynex.BillMaster.Service.Interface;

namespace Lynex.BillMaster.Api.Filters
{
    public class AuthenticateFilter : IAuthorizationFilter
    {
        private readonly ApplicationUserManager _userManager;


        public AuthenticateFilter(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;

            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var username = context.User.Identity.Name;
                var user = _userManager.FindByName(username);

                if (user != null)
                {

                    var identity = _userManager.CreateIdentity(user, "local");
                    var pricipal = new GenericPrincipal(identity, _userManager.GetRoles(user.Id).ToArray());
                    AuthenticateAs(context, pricipal);

                    return;
                }
            }

            AuthenticateAs(context, new GenericPrincipal(new GenericIdentity("1"), new[] {"user"}));
        }

        private static void AuthenticateAs(HttpContextBase context, IPrincipal user)
        {
            Thread.CurrentPrincipal = context.User = user;
        }
    }
}
