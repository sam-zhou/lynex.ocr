using System;
using Lynex.Common.Model.Enum.Mapable;

namespace Lynex.Common.Model.DbModel
{
    public class UserChallenge : BaseEntity
    {
        public virtual string Challenge { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? VerifiedAt { get; set; }

        public virtual int TryCount { get; set; }

        public virtual UserChallengeStatus Status { get; set; }

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
