using System;
using Lynex.Model.Enum;

namespace Lynex.Model.Domain.DbModels
{
    public class Notification : BaseEntity
    {
        public virtual DateTime NotifiedAt { get; set; }

        public virtual NotificationType Type { get; set; }

        public virtual DateTime LocalNotifiedAt
        {
            get { return NotifiedAt.ToLocalTime(); }
        }
    }
}
