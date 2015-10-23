using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Criterion;

namespace Lynex.BillMaster.Api.Filters
{
    public class AuthenticateAttribute : AuthorizeAttribute
    {
        public AuthenticateAttribute()
        {
            Order = 0;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IAuthorizationFilter filter = ServiceLocator.Current.GetInstance<AuthenticateFilter>();
            filter.OnAuthorization(filterContext);
        }
    }
}
