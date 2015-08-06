using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class BillingCompany:BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}
