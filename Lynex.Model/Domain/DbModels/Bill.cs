using System;
using System.Data;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class Bill : BaseEntity
    {
        public virtual double Amount { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual bool IsPaid { get; set; }

        public virtual DateTime? PaidAt { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual BillingCompany Company { get; set; }

        public virtual BillType BillType { get; set; }

        public virtual User User { get; set; }
    }
}
