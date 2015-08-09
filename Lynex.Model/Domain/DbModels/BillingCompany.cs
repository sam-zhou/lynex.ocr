using Lynex.BillMaster.Model.Domain.DbModels.Interface;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class BillingCompany:BaseEntity, IAddressable
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual BillType BillTypes { get; set; }
    }
}
