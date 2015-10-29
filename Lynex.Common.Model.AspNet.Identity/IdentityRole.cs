using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;


namespace Lynex.Common.Model.AspNet.Identity
{
	public class IdentityRole : IRole
	{
		[Key]
		public virtual string Id
		{
			get;
			set;
		}

		public virtual string Name
		{
			get;
			set;
		}

        private static Dictionary<string, IdentityRole> _roles;

        public static Dictionary<string, IdentityRole> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new Dictionary<string, IdentityRole>();
                }
                return _roles;
            }
        }

        protected IdentityRole()
        {

        }

        protected IdentityRole(string roleName, string id)
        {
            Id = id;
            Name = roleName;
        }

        public static IdentityRole User
        {
            get
            {
                if (!Roles.ContainsKey("User"))
                {
                    Roles.Add("User", new IdentityRole("User", "1"));
                }
                return Roles["User"];
            }
        }

        public static IdentityRole Administrator
        {
            get
            {
                if (!Roles.ContainsKey("Administrator"))
                {
                    Roles.Add("Administrator", new IdentityRole("Administrator", "2"));
                }
                return Roles["Administrator"];
            }
        }


    }
}