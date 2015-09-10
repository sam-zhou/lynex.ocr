using System;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class UserChallenge : BaseEntity
    {
        public virtual string Challenge { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? VerifiedAt { get; set; }

        public virtual int TryCount { get; set; }

        public virtual DateTime LocalCreatedAt
        {
            get { return CreatedAt.ToLocalTime(); }
        }

        public UserChallenge(string challenge)
        {
            Challenge = challenge;
        }

        public UserChallenge()
        {
            
        }
    }
}
