using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Model.AspNet.Identity;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class UserRole : IdentityRole, IDbModel
    {
        private static Dictionary<string, UserRole> _roles;

        public static Dictionary<string, UserRole> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new Dictionary<string, UserRole>();
                }
                return _roles;
            }
        }

        protected UserRole()
        {

        }

        public UserRole(string roleName, string id) : base(roleName, id)
        {

        }

        public static UserRole User
        {
            get
            {
                if (!Roles.ContainsKey("User"))
                {
                    Roles.Add("User", new UserRole("User", "1"));
                }
                return Roles["User"];
            }
        }

        public static UserRole Administrator
        {
            get
            {
                if (!Roles.ContainsKey("Administrator"))
                {
                    Roles.Add("Administrator", new UserRole("Administrator", "2"));
                }
                return Roles["Administrator"];
            }
        }
    }
}
