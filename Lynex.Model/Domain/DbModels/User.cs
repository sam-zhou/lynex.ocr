using System;
using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.DbModel.Interface;
using Microsoft.AspNet.Identity;
using NHibernate.Identity;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class User : IdentityUser, IAddressable
    {
        protected User()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (property.SetMethod != null)
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null)
                    {
                        property.SetValue(this, Activator.CreateInstance(property.PropertyType));
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(this, DateTime.UtcNow);
                    }
                }

            }
        }

        public virtual string LastName { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string Title { get; set; }

        public virtual string Email { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Salt { get; set; }

        public virtual string Hash { get; set; }

        public virtual int LoginAttempt { get; set; }

        public virtual bool IsVerified { get; set; }

        public virtual bool Active { get; set; }

        public virtual Address Address { get; set; }

        public virtual Wallet Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual UserChallenge UserChallenge { get; set; }

        public virtual PermissionRole PermissionRole { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual DateTime? LastFailedLogin { get; set; }

        public virtual DateTime? LocalLastFailedLogin
        {
            get { return LastFailedLogin != null ? (DateTime?)LastFailedLogin.Value.ToLocalTime() : null; }
        }

        public virtual DateTime? LocalLastLogin
        {
            get { return LastLogin != null ? (DateTime?)LastLogin.Value.ToLocalTime() : null; }
        }

        public virtual DateTime LocalCreatedAt
        {
            get { return CreatedAt.ToLocalTime(); }
        }

        
    }
}
