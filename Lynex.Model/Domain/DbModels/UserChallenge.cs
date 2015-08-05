using System;

namespace Lynex.Model.Domain.DbModels
{
    public class UserChallenge : BaseEntity
    {
        public virtual string Challenge { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual int TryCount { get; set; }

        public virtual DateTime LocalCreatedAt
        {
            get { return CreatedAt.ToLocalTime(); }
        }
    }
}
