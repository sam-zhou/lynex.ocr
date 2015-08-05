using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Model.Enum;
using Lynex.Model.Enum.Mapable;

namespace Lynex.Model.Domain.DbModels
{
    public class Bill : BaseEntity
    {
        public virtual double Amount { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual BillingCompany Company { get; set; }

        public virtual BillType BillType { get; set; }

        public virtual User User { get; set; }
    }
}
