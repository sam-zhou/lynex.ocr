using System;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class User : BaseEntity
    {
        public virtual long MedWayUserId { get; set; }

        public virtual string Email { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Salt { get; set; }

        public virtual string Hash { get; set; }

        public virtual bool IsVerified { get; set; }

        public virtual bool Active { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual Address PostalAddress { get; set; }

        public virtual Wallet Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual UserChallenge UserChallenge { get; set; }

        public virtual PermissionRole PermissionRole { get; set; }

        public virtual DateTime? LastLogin { get; set; }

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
