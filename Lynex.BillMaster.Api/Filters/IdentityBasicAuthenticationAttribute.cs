using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Lynex.Common.Model.AspNet.Identity;

namespace Lynex.BillMaster.Api.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(ApplicationUserManager userManager, string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.
            var user = await userManager.FindAsync(userName, password);

            if (user == null)
            {
                // No user with userName/password exists.
                return null;
            }

            // Create a ClaimsIdentity with all the claims for this user.
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, IClaimsIdenityFactory doesn't support CancellationTokens.
            ClaimsIdentity identity = await userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, "Basic");
            return new ClaimsPrincipal(identity);
        }

    }
}
