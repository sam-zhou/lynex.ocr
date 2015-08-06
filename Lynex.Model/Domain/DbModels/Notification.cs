using System;
using Lynex.BillMaster.Model.Enum;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
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
