using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Lynex.AspNet.Identity.Owin;
using Lynex.BillMaster.Api.IoC;
using Microsoft.Owin;
using Lynex.BillMaster.Api.Models;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using Lynex.Common.Model;
using Lynex.Common.Model.AspNet.Identity;

using NHibernate;


namespace Lynex.BillMaster.Api
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(IoCContainer.Container.Resolve<IDatabaseService>().Session));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }


    public class UserRoleManager : RoleManager<IdentityRole>
    {
        public UserRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }

        public static UserRoleManager Create()
        {
            var manager = new UserRoleManager(new RoleStore<IdentityRole>(IoCContainer.Container.Resolve<IDatabaseService>().Session));
            // Configure validation logic for usernames
            manager.RoleValidator = new RoleValidator<IdentityRole>(manager);
            return manager;
        }
    }
}
